using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Commands.Test;
using AuctionHouse.Messages.Events;
using AuctionHouse.Persistence;
using AuctionHouse.QueryHandling;
using AuctionHouse.Web;
using AuctionHouse.Web.CodeGen;
using AuctionHouse.Web.Cqrs;
using AuctionHouse.Web.EventSourcing;
using AuctionHouse.Web.Hubs;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using Owin;
using ConnectionSettings = EventStore.ClientAPI.ConnectionSettings;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

[assembly: OwinStartup(typeof(Startup))]

namespace AuctionHouse.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            var hubConfig = new HubConfiguration
            {
                EnableDetailedErrors = true
            };

            httpConfig.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            var container = SetupDependencyInjection(httpConfig, hubConfig);
            httpConfig.MapHttpAttributeRoutes();

            httpConfig.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}"
                );

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseAutofacMiddleware(container);
            app.UseWebApi(httpConfig);
            app.MapSignalR(hubConfig);
            StartSignalRNServiceBusEndpoint(container);

	        if (Web.Configuration.PopulatingDatabaseWithTestDataOnStartupIsEnabled)
	        {
		        container.Resolve<ICommandQueue>()
			        .QueueCommand(new PopulateDatabaseWithTestDataCommand(), Guid.NewGuid(), "system");
	        }
        }

        private static IContainer SetupDependencyInjection(HttpConfiguration httpConfig, HubConfiguration hubConfig)
        {
            var dynamicCqrsApiControllersAssembly = CqrsApiControllerTypesEmitter.EmitCqrsApiControllersAssembly();
            var httpControllerTypeResolver = new DynamicAssemblyControllerTypeResolver(dynamicCqrsApiControllersAssembly);
            httpConfig.Services.Replace(typeof(IHttpControllerTypeResolver), httpControllerTypeResolver);

            var builder = new ContainerBuilder();

            foreach (var dynamicCqrsApiControllerType in dynamicCqrsApiControllersAssembly.GetTypes())
            {
                builder.RegisterType(dynamicCqrsApiControllerType).InstancePerRequest();
            }

            builder.RegisterAssemblyTypes(typeof(QueryHandlingAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces().SingleInstance();

            var nServiceBusEndpoint = StartWebApiNServiceBusEndpoint();

            builder.RegisterInstance(nServiceBusEndpoint)
                .As<IEndpointInstance>()
                .SingleInstance()
                .OnRelease(instance => instance.Stop().Wait());

            builder.RegisterType<NServiceBusCommandQueue>().As<ICommandQueue>().SingleInstance();
            RegisterEventStoreConnection(builder);

            builder.RegisterType<EventStoreEventsDatabase>().As<IEventsDatabase>().InstancePerLifetimeScope();

            builder.RegisterType<EventsAppliedToReadModelTracker>()
                .As<IEventsAppliedToReadModelTracker>()
                .SingleInstance();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.Register(
                c =>
                    hubConfig.Resolver.Resolve<IConnectionManager>()
                        .GetHubContext<CommandHandlingFeedbackHub, ICommandHandlingFeedbackHubClient>())
                .As<IHubContext<ICommandHandlingFeedbackHubClient>>()
                .ExternallyOwned();

            builder.Register(
                c =>
                    hubConfig.Resolver.Resolve<IConnectionManager>()
                        .GetHubContext<EventAppliedToReadModelNotificationHub, IEventAppliedToReadModelNotificationHubClient>())
                .As<IHubContext<IEventAppliedToReadModelNotificationHubClient>>()
                .ExternallyOwned();

            var jsonSerializer =
                JsonSerializer.Create(new JsonSerializerSettings
                {
                    ContractResolver = new SignalRContractResolver()
                });

            builder.RegisterInstance(jsonSerializer).As<JsonSerializer>();
	        RegisterElasticSearchClient(builder);

			var container = builder.Build();
            httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            var signalRDependencyResolver = new AutofacDependencyResolver(container);
            hubConfig.Resolver = signalRDependencyResolver;

            return container;
        }

        private static IEndpointInstance StartWebApiNServiceBusEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.WebApi");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<NServiceBus.JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendOnly();

            endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
                .DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

            return Endpoint.Start(endpointConfiguration).Result;
        }

        private static void RegisterEventStoreConnection(ContainerBuilder containerBuilder)
        {
            //TODO: Read from config
            const int defaultPort = 1113;
            var settings = ConnectionSettings.Create();
            settings.KeepReconnecting().SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
            var endpoint = new IPEndPoint(IPAddress.Loopback, defaultPort);
            var connection = EventStoreConnection.Create(settings, endpoint);
            connection.ConnectAsync().Wait();
            containerBuilder.RegisterInstance(connection).As<IEventStoreConnection>();
        }

        private static IEndpointInstance StartSignalRNServiceBusEndpoint(IContainer container)
        {
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.SignalR");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<NServiceBus.JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var routing = endpointConfiguration.UseTransport<MsmqTransport>().Routing();

	        routing.RegisterPublisher(typeof(EventsAssemblyMarker).Assembly,
		        Web.Configuration.NServiceBusCommandHandlingDestination);

			routing.RegisterPublisher(typeof(EventsAssemblyMarker).Assembly,
				"AuctionHouse.ReadModel.EventsApplyingService");

			endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
                .DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations => { customizations.ExistingLifetimeScope(container); });

            return Endpoint.Start(endpointConfiguration).Result;
        }

	    private static void RegisterElasticSearchClient(ContainerBuilder containerBuilder)
	    {
			const string indexName = "auctionhouse";

			var connectionSettings =
				new Nest.ConnectionSettings().DefaultIndex(indexName).ThrowExceptions().MaximumRetries(int.MaxValue);

			var elasticClient = new ElasticClient(connectionSettings);
			containerBuilder.RegisterInstance(elasticClient).As<IElasticClient>();
		}

        private class DynamicAssemblyControllerTypeResolver : DefaultHttpControllerTypeResolver
        {
            private readonly Assembly _dynamicAssembly;

            public DynamicAssemblyControllerTypeResolver(Assembly dynamicAssembly)
            {
                if (dynamicAssembly == null)
                {
                    throw new ArgumentNullException(nameof(dynamicAssembly));
                }

                _dynamicAssembly = dynamicAssembly;
            }

            public override ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
            {
                var controllerTypes = base.GetControllerTypes(assembliesResolver);
                var dynamicControllerTypes = _dynamicAssembly.GetTypes();

                foreach (var dynamicControllerType in dynamicControllerTypes)
                {
                    controllerTypes.Add(dynamicControllerType);
                }

                return controllerTypes;
            }
        }

        public class SignalRContractResolver : IContractResolver
        {
            private readonly Assembly _assembly;
            private readonly IContractResolver _camelCaseContractResolver;
            private readonly IContractResolver _defaultContractSerializer;

            public SignalRContractResolver()
            {
                _defaultContractSerializer = new DefaultContractResolver();
                _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
                _assembly = typeof(Connection).Assembly;
            }

            public JsonContract ResolveContract(Type type)
            {
                return type.Assembly.Equals(_assembly)
                    ? _defaultContractSerializer.ResolveContract(type)
                    : _camelCaseContractResolver.ResolveContract(type);
            }
        }
    }
}
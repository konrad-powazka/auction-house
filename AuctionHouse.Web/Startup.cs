using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using Owin;
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

            builder.RegisterAssemblyTypes(typeof(QueryHandlingAssemblyMarker).Assembly)
                .As<IEventSourcedEntity>();

            var nServiceBusEndpoint = StartWebApiNServiceBusEndpoint();

            builder.RegisterInstance(nServiceBusEndpoint)
                .As<IEndpointInstance>()
                .SingleInstance()
                .OnRelease(instance => instance.Stop().Wait());

            builder.RegisterType<NServiceBusCommandQueue>().As<ICommandQueue>().SingleInstance();
            RegisterEventStoreConnection(builder);

            builder.RegisterType<EventStoreEventsDatabase>().As<IEventsDatabase>().InstancePerLifetimeScope();

            builder.RegisterType<ContinuousEventSourcedEntitiesBuilder>()
                .As<IContinuousEventSourcedEntitiesBuilder>()
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

            var container = builder.Build();
            httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            container.Resolve<IContinuousEventSourcedEntitiesBuilder>().Start();

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

            endpointConfiguration.UseTransport<MsmqTransport>()
                .Routing()
                .RegisterPublisher(typeof(EventsAssemblyMarker).Assembly,
                    Web.Configuration.NServiceBusCommandHandlingDestination);

            endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
                .DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations => { customizations.ExistingLifetimeScope(container); });

            return Endpoint.Start(endpointConfiguration).Result;
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
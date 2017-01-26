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
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using EventStore.ClientAPI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using Owin;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;

[assembly: OwinStartup(typeof(Startup))]

namespace AuctionHouse.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var container = SetupDependencyInjection(config);

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}"
                );

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            app.UseAutofacMiddleware(container);
            app.UseWebApi(config);

            app.MapSignalR(new HubConfiguration
            {
                EnableDetailedErrors = true,
                Resolver = new AutofacDependencyResolver(container)
            });

            StartSignalRNServiceBusEndpoint();
        }

        public static void Register(HttpConfiguration config)
        {
            SetupDependencyInjection(config);

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{action}"
                );
        }

        private static IContainer SetupDependencyInjection(HttpConfiguration config)
        {
            var dynamicCqrsApiControllersAssembly = CqrsApiControllerTypesEmitter.EmitCqrsApiControllersAssembly();
            var httpControllerTypeResolver = new DynamicAssemblyControllerTypeResolver(dynamicCqrsApiControllersAssembly);
            config.Services.Replace(typeof(IHttpControllerTypeResolver), httpControllerTypeResolver);

            var builder = new ContainerBuilder();

            foreach (var dynamicCqrsApiControllerType in dynamicCqrsApiControllersAssembly.GetTypes())
            {
                builder.RegisterType(dynamicCqrsApiControllerType).InstancePerRequest();
            }

            builder.RegisterAssemblyTypes(typeof(QueryHandlingAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(IQueryHandler<,>)).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(QueryHandlingAssemblyMarker).Assembly)
                .As<IEventSourcedEntity>();

            var nServiceBusEndpoint = CreateNServiceBusEndpoint();

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

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            container.Resolve<IContinuousEventSourcedEntitiesBuilder>().Start();

            return container;
        }

        private static IEndpointInstance CreateNServiceBusEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.WebApi");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
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
            containerBuilder.Register(c =>
            {
                //TODO: Read from config
                const int defaultPort = 1113;
                var settings = ConnectionSettings.Create();
                settings.KeepReconnecting();
                var endpoint = new IPEndPoint(IPAddress.Loopback, defaultPort);
                var connection = EventStoreConnection.Create(settings, endpoint);
                connection.ConnectAsync().Wait();

                return connection;
            }).As<IEventStoreConnection>();
        }

        private static IEndpointInstance StartSignalRNServiceBusEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.SignalR");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.UseTransport<MsmqTransport>()
                .Routing()
                .RegisterPublisher(typeof(EventsAssemblyMarker).Assembly,
                    Web.Configuration.NServiceBusCommandHandlingDestination);

            endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
                .DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

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
    }
}
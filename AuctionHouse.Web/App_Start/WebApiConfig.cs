using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using AuctionHouse.Application;
using AuctionHouse.Core.Messaging;
using AuctionHouse.QueryHandling;
using AuctionHouse.Web.Cqrs;
using Autofac;
using Autofac.Integration.WebApi;
using EventStore.ClientAPI;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;

namespace AuctionHouse.Web
{
    public static class WebApiConfig
    {
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
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
                );
        }

        private static void SetupDependencyInjection(HttpConfiguration config)
        {
            //typeof(WebApiConfig).Assembly.LoadAllReferencedAssemblies();
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

            var nServiceBusEndpoint = CreateNServiceBusEndpoint();

            builder.RegisterInstance(nServiceBusEndpoint)
                .As<IEndpointInstance>()
                .SingleInstance()
                .OnRelease(instance => instance.Stop().Wait());

            builder.RegisterType<NServiceBusCommandQueue>().As<ICommandQueue>().SingleInstance();
            RegisterEventStoreConnection(builder);
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IEndpointInstance CreateNServiceBusEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.Web");
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
                var endpoint = new IPEndPoint(IPAddress.Loopback, defaultPort);
                var connection = EventStoreConnection.Create(settings, endpoint);

                return connection;
            }).As<IEventStoreConnection>();
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
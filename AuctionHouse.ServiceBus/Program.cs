using System;
using System.Net;
using AuctionHouse.Application;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Persistence;
using Autofac;
using EventStore.ClientAPI;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;

namespace AuctionHouse.ServiceBus
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "AuctionHouse.ServiceBus";
            var endpointConfiguration = new EndpointConfiguration("AuctionHouse.ServiceBus");          
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointConfiguration.Conventions()
                .DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
                .DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
                .DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

            endpointConfiguration
                .Recoverability()
                .Delayed(delayed => { delayed.NumberOfRetries(0); })
                .Immediate(immediate => { immediate.NumberOfRetries(0); });

            endpointConfiguration.Pipeline.Register(typeof(CommandHandlingFeedbackBehavior),
                "Publishes an event indicating the result of command handling");

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(typeof(ApplicationAssemblyMarker).Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>)).AsImplementedInterfaces();

            RegisterEventStoreConnection(containerBuilder);

	        containerBuilder.RegisterAssemblyTypes(typeof(PersistenceAssemblyMarker).Assembly)
		        .AsClosedTypesOf(typeof(IRepository<>))
		        .InstancePerLifetimeScope();

            containerBuilder.RegisterType<EventStoreEventsDatabase>()
                .Named<IEventsDatabase>("EventsDatabase")
                .InstancePerLifetimeScope();

            containerBuilder.RegisterDecorator<IEventsDatabase>(
                (context, eventsDatabaseToDecorate) =>
                    new TrackingEventsDatabase(eventsDatabaseToDecorate), "EventsDatabase")
                    .As<IEventsDatabase>()
                    .As<ITrackingEventsDatabase>()
                    .InstancePerLifetimeScope();

	        containerBuilder.RegisterType<TimeProvider>().As<ITimeProvider>();
            var container = containerBuilder.Build();

            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations => { customizations.ExistingLifetimeScope(container); });

            var endpointInstance = Endpoint.Start(endpointConfiguration).Result;
            var updaterContainerBuilder = new ContainerBuilder();
            updaterContainerBuilder.RegisterInstance(endpointInstance).As<IEndpointInstance>();
            updaterContainerBuilder.Update(container);

            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                endpointInstance.Stop().Wait();
            }
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
                connection.ConnectAsync().Wait();

                return connection;
            }).As<IEventStoreConnection>();
        }
    }
}
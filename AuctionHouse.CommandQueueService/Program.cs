using System;
using System.Linq;
using System.Net;
using AuctionHouse.Application;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Messages.Events;
using AuctionHouse.Persistence;
using AuctionHouse.CommandQueueService.Behaviors;
using AuctionHouse.CommandQueueService.Handlers;
using Autofac;
using EventStore.ClientAPI;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;

namespace AuctionHouse.CommandQueueService
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "AuctionHouse.CommandQueueService";
            var endpointConfiguration = new EndpointConfiguration(Constants.EndpointName);          
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

	        endpointConfiguration.Pipeline.Register(typeof(PublishPersistedEventsBehavior),
		        "Publishes all persisted events.");

			endpointConfiguration.UseTransport<MsmqTransport>()
				.Routing()
				.RegisterPublisher(typeof(EventsAssemblyMarker).Assembly,
					Constants.EndpointName);

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
	        containerBuilder.RegisterType<NServiceBusEventPublisher>().As<IEventPublisher>();

			containerBuilder.RegisterAssemblyTypes(typeof(ApplicationAssemblyMarker).Assembly)
				.AsClosedTypesOf(typeof(IEventHandler<>)).AsImplementedInterfaces();

			containerBuilder.RegisterType<NServiceBusCommandQueue>().As<ICommandQueue>();
	        containerBuilder.RegisterType<NsbCommandQueueConfiguration>().As<INServiceBusCommandQueueConfiguration>();
			var container = containerBuilder.Build();
			RegisterNsbEventHandlers(endpointConfiguration, container);

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

	    private static void RegisterNsbEventHandlers(EndpointConfiguration endpointConfiguration,
		    IComponentContext container)
	    {
		    var eventHandlerTypesData =
			    EventsAssemblyMarker.GetEventTypes()
				    .Select(t => new {EventHandlerType = typeof(IEventHandler<>).MakeGenericType(t), EventType = t})
				    .Where(t => container.IsRegistered(t.EventHandlerType));

		    var nsbEventHandlerTypes =
			    eventHandlerTypesData.Select(
				    t => typeof(NServiceBusEventMessageHandler<,>).MakeGenericType(t.EventHandlerType, t.EventType));

		    endpointConfiguration.ExecuteTheseHandlersFirst(nsbEventHandlerTypes);
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
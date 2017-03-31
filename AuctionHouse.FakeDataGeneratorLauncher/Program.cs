using System;
using System.Net;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Persistence;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Nito.AsyncEx;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;
using IEvent = AuctionHouse.Core.Messaging.IEvent;
using IMessage = AuctionHouse.Core.Messaging.IMessage;

namespace AuctionHouse.FakeDataGeneratorLauncher
{
	public class Program
	{
		public static void Main()
		{
			AsyncContext.Run(MainAsync);
		}

		private static async Task MainAsync()
		{
			Console.Title = "AuctionHouse.FakeDataGeneratorLauncher";

			var timeProvider = new TimeProvider();
			var endpointInstance = await StartNServiceBusEndpoint();

			try
			{
				var commandQueue = new NServiceBusCommandQueue(endpointInstance, timeProvider,
					new NServiceBusCommandQueueConfiguration());

				using (var eventStoreConnection = await CreateEventStoreConnection())
				{
					var fakeDataGenerator = new FakeDataGenerator(timeProvider,
						new EventStoreEventsDatabase(eventStoreConnection), commandQueue);

					await fakeDataGenerator.GenerateFakeData();
				}

				Console.WriteLine("Press any key to exit");
				Console.ReadKey();
			}
			finally
			{
				await endpointInstance.Stop();
			}
		}

		private static async Task<IEventStoreConnection> CreateEventStoreConnection()
		{
			const int defaultPort = 1113;
			var settings = ConnectionSettings.Create();
			settings.KeepReconnecting().SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
			var endpoint = new IPEndPoint(IPAddress.Loopback, defaultPort);
			var connection = EventStoreConnection.Create(settings, endpoint);
			await connection.ConnectAsync();
			return connection;
		}

		private static async Task<IEndpointInstance> StartNServiceBusEndpoint()
		{
			var endpointConfiguration = new EndpointConfiguration("AuctionHouse.FakeDataGeneratorLaunchers");
			endpointConfiguration.SendFailedMessagesTo("error");
			endpointConfiguration.UseSerialization<JsonSerializer>();
			endpointConfiguration.UsePersistence<InMemoryPersistence>();
			endpointConfiguration.UseTransport<MsmqTransport>();

			endpointConfiguration.Conventions()
				.DefiningCommandsAs(t => typeof(ICommand).IsAssignableFrom(t))
				.DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t))
				.DefiningMessagesAs(t => typeof(IMessage).IsAssignableFrom(t));

			return await Endpoint.Start(endpointConfiguration);
		}

		private class NServiceBusCommandQueueConfiguration : INServiceBusCommandQueueConfiguration
		{
			public string NServiceBusCommandHandlingDestination => "AuctionHouse.CommandQueueService";
		}
	}
}
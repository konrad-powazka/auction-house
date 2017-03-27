using System;
using System.Net;
using System.Threading.Tasks;
using AuctionHouse.Core.Time;
using AuctionHouse.Persistence;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Nito.AsyncEx;

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
			Console.Title = "AuctionHouse.FakeDataGeneratorLaunchers";

			using (var eventStoreConnection = await CreateEventStoreConnection())
			{
				var fakeDataGenerator = new FakeDataGenerator(new TimeProvider(), new EventStoreEventsDatabase(eventStoreConnection));
				await fakeDataGenerator.GenerateFakeData();
			}
		}

		private static async Task <IEventStoreConnection> CreateEventStoreConnection()
		{
			const int defaultPort = 1113;
			var settings = ConnectionSettings.Create();
			settings.KeepReconnecting().SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
			var endpoint = new IPEndPoint(IPAddress.Loopback, defaultPort);
			var connection = EventStoreConnection.Create(settings, endpoint);
			await connection.ConnectAsync();
			return connection;
		}
	}
}
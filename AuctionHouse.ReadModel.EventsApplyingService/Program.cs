using System;
using System.Net;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Persistence;
using AuctionHouse.ReadModel.Repositories;
using Autofac;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Nest;
using NServiceBus;
using ConnectionSettings = Nest.ConnectionSettings;
using IEvent = AuctionHouse.Core.Messaging.IEvent;

namespace AuctionHouse.ReadModel.EventsApplyingService
{
	public class Program
	{
		private static void Main()
		{
			Console.Title = "AuctionHouse.ReadModel.EventsApplyingService";
			
			using (var autofacContainer = CreateAutofacContainer())
			{
				var continuousEventPropagator = autofacContainer.Resolve<ReadModelEventsApplier>();
				continuousEventPropagator.Start().Wait();

				Console.WriteLine("Press any key to exit");
				Console.ReadKey();

				continuousEventPropagator.Stop().Wait();
			}
		}

		private static IContainer CreateAutofacContainer()
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterAssemblyTypes(typeof(ReadModelAssemblyMarker).Assembly).As<IReadModelBuilder>();
			containerBuilder.RegisterType<ElasticsearchReadModelDbContext>().As<IReadModelDbContext>();
			containerBuilder.RegisterType<EventStoreEventsDatabase>().As<IEventsDatabase>();
			RegisterElasticsearchClient(containerBuilder);
			RegisterEventStoreConnection(containerBuilder);
			RegisterNServiceBusEndpoint(containerBuilder);
			containerBuilder.RegisterType<ReadModelEventsApplier>().AsSelf();
			return containerBuilder.Build();
		}


		private static void RegisterElasticsearchClient(ContainerBuilder containerBuilder)
		{
			const string indexName = "auctionhouse";

			var connectionSettings =
				new ConnectionSettings().DefaultIndex(indexName)
					.ThrowExceptions()
					.RequestTimeout(TimeSpan.FromSeconds(30))
					.MaxRetryTimeout(TimeSpan.FromSeconds(30))
					.MaximumRetries(2);

			var elasticClient = new ElasticClient(connectionSettings);

			// Timeouts an retry settings do not seem to be working for cases when the
			// elasticsearch server is offline, so we need this ugly workaround
			bool elasticsearchStarted;

			do
			{
				elasticsearchStarted = true;

				try
				{
					elasticClient.ClusterHealth();
				}
				catch
				{
					elasticsearchStarted = false;
				}
			} while (!elasticsearchStarted);

			// Rebuilding an index each time the service restarts is not optimal, but creating an infrastructure to persist
			// last processed event and build new/corrupted read models from scratch would be costly
			if (elasticClient.IndexExists(indexName).Exists)
			{
				elasticClient.DeleteByQueryAsync(new DeleteByQueryRequest(indexName, Types.All)).Wait();
			}
			else
			{
				elasticClient.CreateIndexAsync(indexName).Wait();
			}

			containerBuilder.RegisterInstance(elasticClient).As<IElasticClient>();
		}

		private static void RegisterEventStoreConnection(ContainerBuilder containerBuilder)
		{
			//TODO: Read from config
			const int defaultPort = 1113;
			var settings = EventStore.ClientAPI.ConnectionSettings.Create();
			settings.KeepReconnecting().SetDefaultUserCredentials(new UserCredentials("admin", "changeit"));
			var endpoint = new IPEndPoint(IPAddress.Loopback, defaultPort);
			var connection = EventStoreConnection.Create(settings, endpoint);
			connection.ConnectAsync().Wait();
			containerBuilder.RegisterInstance(connection).As<IEventStoreConnection>();
		}

		private static void RegisterNServiceBusEndpoint(ContainerBuilder containerBuilder)
		{
			var endpointConfiguration = new EndpointConfiguration("AuctionHouse.ReadModel.EventsApplyingService");
			endpointConfiguration.SendFailedMessagesTo("error");
			endpointConfiguration.UseSerialization<JsonSerializer>();
			endpointConfiguration.UsePersistence<InMemoryPersistence>();
			endpointConfiguration.UseTransport<MsmqTransport>();

			endpointConfiguration.Conventions()
				.DefiningEventsAs(t => typeof(IEvent).IsAssignableFrom(t));

			var nServiceBusEndpointInstance = Endpoint.Start(endpointConfiguration).Result;

			containerBuilder.RegisterInstance(nServiceBusEndpointInstance)
				.OnRelease(e => e.Stop().Wait())
				.As<IEndpointInstance>();
		}
	}
}
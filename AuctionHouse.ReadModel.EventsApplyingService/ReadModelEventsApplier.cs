using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Messages.Events.Technical;
using AuctionHouse.Persistence;
using AuctionHouse.ReadModel.Repositories;
using Nest;
using NServiceBus;
using IEvent = AuctionHouse.Core.Messaging.IEvent;

namespace AuctionHouse.ReadModel.EventsApplyingService
{
	// TODO: Updating read model should be greately optimized. Events should be processed in batches and read model repository should be cached on client side.
	public class ReadModelEventsApplier
	{
		private readonly IEventsDatabase _eventsDatabase;
		private readonly IEndpointInstance _nServiceBusEndpointInstance;
		private readonly IEnumerable<IReadModelBuilder> _readModelBuilders;
		private readonly IReadModelRepository _readModelRepository;
		private IDisposable _eventsSubscription;

		public ReadModelEventsApplier(IEventsDatabase eventsDatabase, IElasticClient elasticClient,
			IEnumerable<IReadModelBuilder> readModelBuilders, IEndpointInstance nServiceBusEndpointInstance)
		{
			_eventsDatabase = eventsDatabase;
			_readModelBuilders = readModelBuilders;
			_nServiceBusEndpointInstance = nServiceBusEndpointInstance;
			_readModelRepository = new ElasticsearchReadModelRepository(elasticClient);
		}

		public async Task Start()
		{
			_eventsSubscription =
				await _eventsDatabase.ReadAllExistingEventsAndSubscribe(async e => await HandleEventEnvelope(e));
		}

		private async Task HandleEventEnvelope(MessageEnvelope<IEvent> eventEnvelope)
		{
			// In real life this would be handled by a decorator
			if (Configuration.EventsApplicationToReadModelDelayInMilliseconds.HasValue)
			{
				Thread.Sleep(Configuration.EventsApplicationToReadModelDelayInMilliseconds.Value);
			}

			foreach (var readModelBuilder in _readModelBuilders)
			{
				await readModelBuilder.Apply(eventEnvelope.Message, _readModelRepository);
			}

			var eventsAppliedToReadModelEvent = new EventsAppliedToReadModelEvent
			{
				AppliedEventIds = new[] {eventEnvelope.MessageId}
			};

			// TODO: Only live events should be included
			await _nServiceBusEndpointInstance.Publish(eventsAppliedToReadModelEvent);
		}

		public async Task Stop()
		{
			_eventsSubscription?.Dispose();
		}
	}
}
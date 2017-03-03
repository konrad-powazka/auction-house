using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
	public class ReadModelEventsApplier : IDisposable
	{
		private const int BatchConstructionTimeoutMilliseconds = 1000; // TODO: Move to cfg
		private readonly IEventsDatabase _eventsDatabase;

		private readonly BlockingCollection<MessageEnvelope<IEvent>> _eventsQueue =
			new BlockingCollection<MessageEnvelope<IEvent>>();

		private readonly IEndpointInstance _nServiceBusEndpointInstance;
		private readonly CancellationTokenSource _processingStoppedCancellationTokenSource = new CancellationTokenSource();
		private readonly IEnumerable<IReadModelBuilder> _readModelBuilders;
		private readonly IReadModelDbContext _readModelDbContext;
		private IDisposable _eventsSubscription;

		public ReadModelEventsApplier(IEventsDatabase eventsDatabase, IElasticClient elasticClient,
			IEnumerable<IReadModelBuilder> readModelBuilders, IEndpointInstance nServiceBusEndpointInstance)
		{
			_eventsDatabase = eventsDatabase;
			_readModelBuilders = readModelBuilders;
			_nServiceBusEndpointInstance = nServiceBusEndpointInstance;
			_readModelDbContext = new ElasticsearchReadModelDbContext(elasticClient);
		}

		public void Dispose()
		{
			Stop().Wait();
		}

		public async Task Start()
		{
			_eventsSubscription =
				await _eventsDatabase.ReadAllExistingEventsAndSubscribe(e => _eventsQueue.Add(e));

			Task.Run(() => ProcessQueue());
		}

		private void ProcessQueue()
		{
			var appliedEventIds = new HashSet<Guid>();
			var appliedEventIdsInCurrentBatch = new HashSet<Guid>();

			while (true)
			{
				var eventEnvelope = _eventsQueue.Take(_processingStoppedCancellationTokenSource.Token);

				if (appliedEventIds.Contains(eventEnvelope.MessageId))
				{
					continue;
				}

				var batchConstructionStopwatch = new Stopwatch();
				batchConstructionStopwatch.Start();

				while (true)
				{
					if (appliedEventIds.Contains(eventEnvelope.MessageId))
					{
						continue;
					}

					HandleEventEnvelope(eventEnvelope);
					appliedEventIds.Add(eventEnvelope.MessageId);
					appliedEventIdsInCurrentBatch.Add(eventEnvelope.MessageId);

					var millisecondsLeftForBatchConstruction = BatchConstructionTimeoutMilliseconds -
					                                           batchConstructionStopwatch.ElapsedMilliseconds;

					var batchConstructionTimedOut = millisecondsLeftForBatchConstruction <= 0;

					var batchShouldBeFinished = batchConstructionTimedOut ||
					                            !_eventsQueue.TryTake(out eventEnvelope, 0,
						                            _processingStoppedCancellationTokenSource.Token);

					if (!batchShouldBeFinished)
					{
						continue;
					}

					_readModelDbContext.SaveChanges().Wait();
					Console.WriteLine(appliedEventIdsInCurrentBatch.Count); // TODO: Create a logger

					var eventsAppliedToReadModelEvent = new EventsAppliedToReadModelEvent
					{
						AppliedEventIds = appliedEventIdsInCurrentBatch.ToList()
					};

					// TODO: Only live events should be included
					_nServiceBusEndpointInstance.Publish(eventsAppliedToReadModelEvent).Wait();
					appliedEventIdsInCurrentBatch.Clear();
					break;
				}
			}
		}

		private void HandleEventEnvelope(MessageEnvelope<IEvent> eventEnvelope)
		{
			// In real life this would be handled by a decorator
			if (Configuration.EventsApplicationToReadModelDelayInMilliseconds.HasValue)
			{
				Thread.Sleep(Configuration.EventsApplicationToReadModelDelayInMilliseconds.Value);
			}

			foreach (var readModelBuilder in _readModelBuilders)
			{
				readModelBuilder.Apply(eventEnvelope.Message, _readModelDbContext).Wait();
			}
		}

		public async Task Stop()
		{
			_eventsSubscription?.Dispose();
			_processingStoppedCancellationTokenSource.Cancel();
		}
	}
}
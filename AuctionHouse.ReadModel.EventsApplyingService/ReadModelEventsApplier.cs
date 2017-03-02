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
		private readonly IEventsDatabase _eventsDatabase;
		private readonly BlockingCollection<MessageEnvelope<IEvent>> _eventsQueue =
			new BlockingCollection<MessageEnvelope<IEvent>>();

		private readonly IEndpointInstance _nServiceBusEndpointInstance;
		private readonly CancellationTokenSource _processingStoppedCancellationTokenSource = new CancellationTokenSource();
		private readonly IEnumerable<IReadModelBuilder> _readModelBuilders;
		private readonly IReadModelDbContext _readModelDbContext;
		private readonly Stopwatch _stopwatch = new Stopwatch();
		private IDisposable _eventsSubscription;
		private int _numberOfEventsProcessed;

		public ReadModelEventsApplier(IEventsDatabase eventsDatabase, IElasticClient elasticClient,
			IEnumerable<IReadModelBuilder> readModelBuilders, IEndpointInstance nServiceBusEndpointInstance)
		{
			_eventsDatabase = eventsDatabase;
			_readModelBuilders = readModelBuilders;
			_nServiceBusEndpointInstance = nServiceBusEndpointInstance;
			_readModelDbContext = new ElasticsearchReadModelDbContext(elasticClient);
			_stopwatch.Start();
		}

		public async Task Start()
		{
			_eventsSubscription =
				await _eventsDatabase.ReadAllExistingEventsAndSubscribe(e => _eventsQueue.Add(e));

			Task.Run(() => ProcessQueue());
		}

		private void ProcessQueue()
		{
			while (true)
			{
				var eventEnvelope = _eventsQueue.Take(_processingStoppedCancellationTokenSource.Token);
				const int maxBatchSize = 10; // TODO: Make larger after tests
				var eventEnvelopesBatch = new List<MessageEnvelope<IEvent>>(maxBatchSize);
				var batchConstructionTimeout = new TimeSpan(0, 0, 0, 1000);
				var batchConstructionStopwatch = new Stopwatch();
				batchConstructionStopwatch.Start();

				while (true)
				{
					eventEnvelopesBatch.Add(eventEnvelope);
					var timeLeftForBatchConstruction = batchConstructionTimeout - batchConstructionStopwatch.Elapsed;

					if (eventEnvelopesBatch.Count >= maxBatchSize || timeLeftForBatchConstruction < TimeSpan.Zero)
					{
						ProcessEventEnvelopesBatch(eventEnvelopesBatch);
						break;
					}

					const int maximumDelayInMillisecondsBetweenEventsInBatch = 100;

					var nextEventDequeueTimeoutInMilliseconds = Math.Min(maximumDelayInMillisecondsBetweenEventsInBatch,
						timeLeftForBatchConstruction.Milliseconds);

					var eventEnvelopeDequeueingTimedOut =
						!_eventsQueue.TryTake(out eventEnvelope, nextEventDequeueTimeoutInMilliseconds,
							_processingStoppedCancellationTokenSource.Token);

					if (eventEnvelopeDequeueingTimedOut)
					{
						ProcessEventEnvelopesBatch(eventEnvelopesBatch);
						break;
					}
				}
			}
		}

		private void ProcessEventEnvelopesBatch(IReadOnlyCollection<MessageEnvelope<IEvent>> eventEnvelopesBatch)
		{
			Console.WriteLine(eventEnvelopesBatch.Count());

			// TODO na cacheowane repo
			foreach (var eventEnvelope in eventEnvelopesBatch)
			{
				HandleEventEnvelope(eventEnvelope);
			}

			_readModelDbContext.SaveChanges().Wait();
			var eventsAppliedToReadModelEvent = new EventsAppliedToReadModelEvent
			{
				AppliedEventIds = eventEnvelopesBatch.Select(e => e.MessageId).ToList()
			};

			// TODO: Only live events should be included
			_nServiceBusEndpointInstance.Publish(eventsAppliedToReadModelEvent).Wait();
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

			_numberOfEventsProcessed++;
			Console.WriteLine(
				$"{_numberOfEventsProcessed.ToString("D8")} | {_stopwatch.Elapsed.ToString("c")} | TID: {Thread.CurrentThread.ManagedThreadId} | {eventEnvelope.MessageId}");
			_stopwatch.Restart();
		}

		public async Task Stop()
		{
			_eventsSubscription?.Dispose();
			_processingStoppedCancellationTokenSource.Cancel();
		}

		public void Dispose()
		{
			Stop().Wait();
		}
	}
}
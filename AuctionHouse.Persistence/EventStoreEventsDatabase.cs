using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace AuctionHouse.Persistence
{
	public class EventStoreEventsDatabase : IEventsDatabase
	{
		private readonly IEventStoreConnection _eventStoreConnection;

		public EventStoreEventsDatabase(IEventStoreConnection eventStoreConnection)
		{
			_eventStoreConnection = eventStoreConnection;
		}

		public async Task AppendToStream(string streamName, IEnumerable<IEventEnvelope<IEvent>> eventEnvelopesToAppend,
			ExpectedStreamVersion expectedStreamVersion, int? specificExpectedStreamVersion)
		{
			if (eventEnvelopesToAppend == null)
			{
				throw new ArgumentNullException(nameof(eventEnvelopesToAppend));
			}

			var eventDataList =
				eventEnvelopesToAppend.Select(e =>
				{
					var serializedEvent = SerializeEvent(e.Event);
					return new EventData(e.EventId, e.Event.GetType().Name, true, serializedEvent, null);
				});

			var eventStoreExpectedStreamVersion = GetEventStoreExpectedStreamVersion(expectedStreamVersion,
				specificExpectedStreamVersion);

			await
				_eventStoreConnection.AppendToStreamAsync(streamName,
					eventStoreExpectedStreamVersion,
					eventDataList);
		}

		public async Task<IEnumerable<PersistedEventEnvelope>> ReadStream(string streamName)
		{
			var eventEnvelopes = new List<PersistedEventEnvelope>();

			StreamEventsSlice eventsSlice;
			var nextSliceStart = StreamPosition.Start;

			do
			{
				eventsSlice =
					await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 400, false);

				nextSliceStart = eventsSlice.NextEventNumber;

				var sliceEventEnvelopes = eventsSlice.Events.Select(e => MapToEventEnvelope(e.Event)).ToList();
				eventEnvelopes.AddRange(sliceEventEnvelopes);
			} while (!eventsSlice.IsEndOfStream);

			return eventEnvelopes;
		}

		public async Task<IDisposable> ReadAllExistingEventsAndSubscribe(
			Action<PersistedEventEnvelope> handleEventEnvelopeCallback)
		{
			if (handleEventEnvelopeCallback == null)
			{
				throw new ArgumentNullException(nameof(handleEventEnvelopeCallback));
			}

			EventStoreCatchUpSubscription subscription = null;
			SubscribeToAllFrom(AllCheckpoint.AllStart, s => subscription = s, handleEventEnvelopeCallback);
			return Disposable.Create(() => subscription.Stop());
		}

		// TODO: Is it thread safe?
		private void SubscribeToAllFrom(Position? subscriptionCheckpoint,
			Action<EventStoreAllCatchUpSubscription> setSubscription, Action<PersistedEventEnvelope> handleEventEnvelopeCallback)
		{
			var subscription = _eventStoreConnection.SubscribeToAllFrom(subscriptionCheckpoint,
				CatchUpSubscriptionSettings.Default,
				(s, e) =>
				{
					subscriptionCheckpoint = e.OriginalPosition;

					if (CheckIfIsInternalEventStoreEvent(e))
					{
						return;
					}

					var eventEnvelope = MapToEventEnvelope(e.Event);
					handleEventEnvelopeCallback(eventEnvelope);
				}, subscriptionDropped: (droppedSubscription, subscriptionDropReason, exception) =>
				{
					droppedSubscription.Stop();
					SubscribeToAllFrom(subscriptionCheckpoint, setSubscription, handleEventEnvelopeCallback);
				});

			setSubscription(subscription);
		}

		private static int GetEventStoreExpectedStreamVersion(ExpectedStreamVersion expectedStreamVersion,
			int? specificExpectedStreamVersion)
		{
			switch (expectedStreamVersion)
			{
				case ExpectedStreamVersion.AnyExisting:
					return ExpectedVersion.StreamExists;
				case ExpectedStreamVersion.NotExisting:
					return ExpectedVersion.NoStream;
				case ExpectedStreamVersion.SpecificExisting:
				{
					if (!specificExpectedStreamVersion.HasValue)
					{
						throw new ArgumentNullException(nameof(expectedStreamVersion));
					}

					return specificExpectedStreamVersion.Value;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(expectedStreamVersion));
			}
		}

		private static PersistedEventEnvelope MapToEventEnvelope(RecordedEvent recordedEvent)
		{
			var eventType =
				EventsAssemblyMarker.GetEventTypes().Single(t => t.Name == recordedEvent.EventType);

			var @event = DeserializeEvent(recordedEvent.Data, eventType);

			var eventEnvelope = new PersistedEventEnvelope(@event, recordedEvent.EventId, recordedEvent.EventStreamId,
				recordedEvent.EventNumber);

			return eventEnvelope;
		}

		private static bool CheckIfIsInternalEventStoreEvent(ResolvedEvent eventStoreEvent)
		{
			return eventStoreEvent.Event.EventType.StartsWith("$");
		}

		private static byte[] SerializeEvent(IEvent eventToSerialize)
		{
			var textSerializedEvent = JsonConvert.SerializeObject(eventToSerialize);
			var binarySerializedEvent = Encoding.UTF8.GetBytes(textSerializedEvent);

			return binarySerializedEvent;
		}

		private static IEvent DeserializeEvent(byte[] eventBytes, Type eventType)
		{
			var textSerializedEvent = Encoding.UTF8.GetString(eventBytes);
			var @event = (IEvent) JsonConvert.DeserializeObject(textSerializedEvent, eventType);

			return @event;
		}
	}
}
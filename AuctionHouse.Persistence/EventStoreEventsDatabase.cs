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

		public async Task AppendToStream(string streamName, IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend,
			ExpectedStreamVersion expectedStreamVersion, int? specificExpectedStreamVersion)
		{
			if (eventEnvelopesToAppend == null)
			{
				throw new ArgumentNullException(nameof(eventEnvelopesToAppend));
			}

			var eventDataList =
				eventEnvelopesToAppend.Select(e =>
				{
					var serializedEvent = SerializeEvent(e.Message);
					return new EventData(e.MessageId, e.Message.GetType().Name, true, serializedEvent, null);
				});

			var eventStoreExpectedStreamVersion = GetEventStoreExpectedStreamVersion(expectedStreamVersion,
				specificExpectedStreamVersion);

			await
				_eventStoreConnection.AppendToStreamAsync(streamName,
					eventStoreExpectedStreamVersion,
					eventDataList);
		}

		public async Task<IEnumerable<IMessageEnvelope<IEvent>>> ReadStream(string streamName)
		{
			var eventEnvelopes = new List<MessageEnvelope<IEvent>>();

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
			Action<MessageEnvelope<IEvent>> handleEventEnvelopeCallback)
		{
			if (handleEventEnvelopeCallback == null)
			{
				throw new ArgumentNullException(nameof(handleEventEnvelopeCallback));
			}

			// TODO: Handle subscription dropped
			var subscription = _eventStoreConnection.SubscribeToAllFrom(null, CatchUpSubscriptionSettings.Default,
				(s, e) =>
				{
					if (CheckIfIsInternalEventStoreEvent(e))
					{
						return;
					}

					var eventEnvelope = MapToEventEnvelope(e.Event);
					handleEventEnvelopeCallback(eventEnvelope);
				}, subscriptionDropped: SubscriptionDropped);

			return Disposable.Create(() => subscription.Stop());
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

		private MessageEnvelope<IEvent> MapToEventEnvelope(RecordedEvent recordedEvent)
		{
			var eventType =
				EventsAssemblyMarker.GetEventTypes().Single(t => t.Name == recordedEvent.EventType);

			var @event = DeserializeEvent(recordedEvent.Data, eventType);
			var eventEnvelope = new MessageEnvelope<IEvent>(@event, recordedEvent.EventId);
			return eventEnvelope;
		}

		private void SubscriptionDropped(EventStoreCatchUpSubscription eventStoreCatchUpSubscription,
			SubscriptionDropReason subscriptionDropReason, Exception arg3)
		{
			//TODO: Resume subscription
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
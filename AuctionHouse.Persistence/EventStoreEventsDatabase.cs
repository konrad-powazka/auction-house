using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.DynamicTypeScanning;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
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

        public async Task AppendToStream(string streamName, int? expectedStreamVersion,
            IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend)
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

            await
                _eventStoreConnection.AppendToStreamAsync(streamName,
                    expectedStreamVersion ?? ExpectedVersion.NoStream,
                    eventDataList);
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

                    var eventType =
                        DynamicTypeScanner.GetEventTypes().Single(t => t.Name == e.Event.EventType);

                    var @event = DeserializeEvent(e.Event.Data, eventType);
                    var eventEnvelope = new MessageEnvelope<IEvent>(e.Event.EventId, @event);
                    handleEventEnvelopeCallback(eventEnvelope);
                }, subscriptionDropped: SubscriptionDropped);

            return Disposable.Create(() => subscription.Stop());
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription eventStoreCatchUpSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            //TODO: Resume subscription
            throw new NotImplementedException();
        }

        private bool CheckIfIsInternalEventStoreEvent(ResolvedEvent eventStoreEvent)
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
            var @event = (IEvent)JsonConvert.DeserializeObject(textSerializedEvent, eventType);

            return @event;
        }
    }
}
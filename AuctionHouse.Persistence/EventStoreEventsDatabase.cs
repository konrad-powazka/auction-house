using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
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

        public async Task AppendToStream(string streamName, int? expectedStreamVersion, IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend)
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

        private static byte[] SerializeEvent(IEvent eventToSerialize)
        {
            var textSerializedEvent = JsonConvert.SerializeObject(eventToSerialize);
            var binarySerializedEvent = Encoding.UTF8.GetBytes(textSerializedEvent);

            return binarySerializedEvent;
        }
    }
}
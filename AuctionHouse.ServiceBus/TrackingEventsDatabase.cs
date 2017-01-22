using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Persistence;

namespace AuctionHouse.ServiceBus
{
    public class TrackingEventsDatabase : ITrackingEventsDatabase
    {
        private readonly IEventsDatabase _eventsDatabase;
        private readonly List<MessageEnvelope<IEvent>> _writtenEventEnvelopes = new List<MessageEnvelope<IEvent>>();

        public TrackingEventsDatabase(IEventsDatabase eventsDatabase)
        {
            if (eventsDatabase == null)
            {
                throw new ArgumentNullException(nameof(eventsDatabase));
            }

            _eventsDatabase = eventsDatabase;
        }

        public IReadOnlyCollection<MessageEnvelope<IEvent>> WrittenEventEnvelopes => _writtenEventEnvelopes.ToList();

        public async Task AppendToStream(string streamName, int? expectedStreamVersion,
            IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend)
        {
            eventEnvelopesToAppend = eventEnvelopesToAppend.ToList();
            await _eventsDatabase.AppendToStream(streamName, expectedStreamVersion, eventEnvelopesToAppend);
            _writtenEventEnvelopes.AddRange(eventEnvelopesToAppend);
        }
    }
}
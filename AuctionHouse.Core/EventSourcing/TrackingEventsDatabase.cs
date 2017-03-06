using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
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

        public async Task AppendToStream(string streamName, IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend, ExpectedStreamVersion expectedStreamVersion, int? specificExpectedStreamVersion)
        {
            eventEnvelopesToAppend = eventEnvelopesToAppend.ToList();
            await _eventsDatabase.AppendToStream(streamName, eventEnvelopesToAppend, expectedStreamVersion, specificExpectedStreamVersion);
            _writtenEventEnvelopes.AddRange(eventEnvelopesToAppend);
        }

	    public Task<IEnumerable<IMessageEnvelope<IEvent>>> ReadStream(string streamName)
	    {
		    return _eventsDatabase.ReadStream(streamName);
	    }

	    public Task<IDisposable> ReadAllExistingEventsAndSubscribe(
            Action<MessageEnvelope<IEvent>> handleEventEnvelopeCallback)
        {
            return _eventsDatabase.ReadAllExistingEventsAndSubscribe(handleEventEnvelopeCallback);
        }
    }
}
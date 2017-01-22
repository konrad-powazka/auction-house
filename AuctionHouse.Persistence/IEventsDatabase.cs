using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Persistence
{
    public interface IEventsDatabase
    {
        Task AppendToStream(string streamName, int? expectedStreamVersion,
            IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend);

        Task<IDisposable> ReadAllExistingEventsAndSubscribe(Action<MessageEnvelope<IEvent>> handleEventEnvelopeCallback);
    }
}
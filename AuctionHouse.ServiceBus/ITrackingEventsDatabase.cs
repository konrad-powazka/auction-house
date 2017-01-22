using System.Collections.Generic;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Persistence;

namespace AuctionHouse.ServiceBus
{
    public interface ITrackingEventsDatabase : IEventsDatabase
    {
        IReadOnlyCollection<MessageEnvelope<IEvent>> WrittenEventEnvelopes { get; }
    }
}
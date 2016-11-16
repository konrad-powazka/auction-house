using System;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain;

namespace AuctionHouse.Persistence
{
    public class EventStoreRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot, new()
    {
        public TAggregateRoot Get(Guid aggregateRootId)
        {
            var eventsToReplay = new IEvent[0]; //TODO: Get from ES
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.ReplayEvents(eventsToReplay);

            return aggregateRoot;
        }

        public void Save(TAggregateRoot aggregateRoot, int previousAggregateRootVersion)
        {
            throw new NotImplementedException();
        }
    }
}
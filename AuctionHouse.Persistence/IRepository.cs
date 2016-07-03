using System;
using AuctionHouse.Domain;

namespace AuctionHouse.Persistence
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        TAggregateRoot Get(Guid aggregateRootId);

        void Save(TAggregateRoot aggregateRoot, int previousAggregateRootVersion);
    }
}
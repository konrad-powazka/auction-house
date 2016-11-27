using System;
using System.Threading.Tasks;
using AuctionHouse.Domain;

namespace AuctionHouse.Persistence
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        Task Create(TAggregateRoot aggregateRoot);
        Task<TAggregateRoot> Get(Guid aggregateRootId);
        Task Save(TAggregateRoot aggregateRoot, int previousAggregateRootVersion);
    }
}
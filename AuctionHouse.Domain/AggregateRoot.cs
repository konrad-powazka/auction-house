using System;

namespace AuctionHouse.Domain
{
    public class AggregateRoot : Entity
    {
        public AggregateRoot(Guid id) : base(id)
        {
        }
    }
}
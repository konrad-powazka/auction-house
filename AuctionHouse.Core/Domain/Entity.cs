using System;

namespace AuctionHouse.Core.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
    }
}
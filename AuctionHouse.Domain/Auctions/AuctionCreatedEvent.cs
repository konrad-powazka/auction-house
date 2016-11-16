using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Domain.Auctions
{
    public class AuctionCreatedEvent : IEvent
    {
        public Guid AuctionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
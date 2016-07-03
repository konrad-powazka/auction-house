using System;

namespace AuctionHouse.Domain.Auctions
{
    public class AuctionCreatedEvent : IEvent
    {
        public Guid AuctionId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}
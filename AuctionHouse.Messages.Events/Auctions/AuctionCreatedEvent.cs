using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.Auctions
{
    public class AuctionCreatedEvent : IEvent
    {
        public Guid Id { get; set;  }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
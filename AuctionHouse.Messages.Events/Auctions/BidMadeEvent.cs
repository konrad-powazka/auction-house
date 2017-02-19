using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.Auctions
{
    public class BidMadeEvent : IEvent
    {
        public Guid AuctionId { get; set; }
        public string BidderUserName { get; set; }
        public decimal BidPrice { get; set; }
        public string HighestBidderUserName { get; set; }
        public decimal HighestBidPrice { get; set; }
        public decimal MinimalPriceForNextBidder { get; set; }
    }
}
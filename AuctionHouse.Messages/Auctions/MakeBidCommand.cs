using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Auctions
{
    public class MakeBidCommand : ICommand
    {
        public Guid AuctionId { get; set; }

        public decimal Price { get; set; }
    }
}
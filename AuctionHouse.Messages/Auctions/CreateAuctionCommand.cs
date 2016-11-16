using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Auctions
{
    public class CreateAuctionCommand : ICommand
    {
        public Guid AuctionId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}
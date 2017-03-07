using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Commands.Auctions
{
    public class MakeBidCommand : ICommand
    {
        public Guid AuctionId { get; set; }
        public decimal Price { get; set; }
        public int ExpectedAuctionVersion { get; set; }
    }
}
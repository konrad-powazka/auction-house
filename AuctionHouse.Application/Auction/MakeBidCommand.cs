using System;

namespace AuctionHouse.Application.Auction
{
    public class MakeBidCommand : ICommand
    {
        public Guid AuctionId { get; set; }

        public decimal Price { get; set; }
    }
}
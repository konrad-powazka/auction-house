using System;

namespace AuctionHouse.Application.Auction
{
    public class CreateAuctionCommand : ICommand
    {
        public Guid AuctionId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}
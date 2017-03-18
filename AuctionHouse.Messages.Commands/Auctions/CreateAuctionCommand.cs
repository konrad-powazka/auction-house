using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Commands.Auctions
{
    public class CreateAuctionCommand : ICommand
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal? BuyNowPrice { get; set; }

        public DateTime EndDate { get; set; }
    }
}
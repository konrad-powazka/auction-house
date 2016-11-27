using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Commands.Auctions
{
    public class CancelAuctionCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
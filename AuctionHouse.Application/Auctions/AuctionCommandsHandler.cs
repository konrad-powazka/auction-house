using System;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Auctions;

namespace AuctionHouse.Application.Auctions
{
    public class AuctionCommandsHandler : ICommandHandler<CreateAuctionCommand>
    {
        public void Handle(CreateAuctionCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
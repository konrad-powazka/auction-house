using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Auctions;

namespace AuctionHouse.Application.Auctions
{
    public class AuctionCommandsHandler : ICommandHandler<CreateAuctionCommand>
    {
        public async Task Handle(CreateAuctionCommand command)
        {
            Console.WriteLine("AuctionCommandsHandler executed");
        }
    }
}
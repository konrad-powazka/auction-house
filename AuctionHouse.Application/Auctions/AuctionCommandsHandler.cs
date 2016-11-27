using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
    public class AuctionCommandsHandler : ICommandHandler<CreateAuctionCommand>
    {
        private readonly IRepository<Auction> _auctionsRepository;

        public AuctionCommandsHandler(IRepository<Auction> auctionsRepository)
        {
            _auctionsRepository = auctionsRepository;
        }

        public async Task Handle(CreateAuctionCommand command)
        {
            var createdAuction = Auction.Create(command.AuctionId, command.Title, command.Description, command.EndDate,
                command.StartingPrice, command.BuyNowPrice);

            await _auctionsRepository.Create(createdAuction);
        }
    }
}
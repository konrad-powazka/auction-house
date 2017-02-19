using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
    public class AuctionsService : ICommandHandler<CreateAuctionCommand>
    {
        private readonly IRepository<Auction> _auctionsRepository;

        public AuctionsService(IRepository<Auction> auctionsRepository)
        {
            _auctionsRepository = auctionsRepository;
        }

        public async Task Handle(CommandEnvelope<CreateAuctionCommand> commandEnvelope)
        {
            var command = commandEnvelope.Message;
            var createdAuction = Auction.Create(command.AuctionId, command.Title, command.Description, command.EndDate,
                command.StartingPrice, command.BuyNowPrice, commandEnvelope.SenderUserName);

            await _auctionsRepository.Create(createdAuction);
        }
    }
}
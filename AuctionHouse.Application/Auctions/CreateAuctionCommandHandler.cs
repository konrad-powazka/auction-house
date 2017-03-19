using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
	public class CreateAuctionCommandHandler : ICommandHandler<CreateAuctionCommand>
	{
		private readonly IRepository<Auction> _auctionsRepository;
		private readonly ITimeProvider _timeProvider;

		public CreateAuctionCommandHandler(IRepository<Auction> auctionsRepository, ITimeProvider timeProvider)
		{
			_auctionsRepository = auctionsRepository;
			_timeProvider = timeProvider;
		}

		public async Task Handle(CommandEnvelope<CreateAuctionCommand> commandEnvelope)
		{
			var command = commandEnvelope.Message;

			var createdAuction = Auction.Create(command.Id, command.Title, command.Description, command.EndDate,
				command.StartingPrice, command.BuyNowPrice, commandEnvelope.SenderUserName, _timeProvider);

			await _auctionsRepository.Create(createdAuction, commandEnvelope.MessageId.ToString());
		}
	}
}
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
	public class AuctionsService :
		ICommandHandler<CreateAuctionCommand>,
		ICommandHandler<FinishAuctionCommand>
	{
		private readonly IRepository<Auction> _auctionsRepository;
		private readonly ITimeProvider _timeProvider;

		public AuctionsService(IRepository<Auction> auctionsRepository, ITimeProvider timeProvider)
		{
			_auctionsRepository = auctionsRepository;
			_timeProvider = timeProvider;
		}

		public async Task Handle(CommandEnvelope<CreateAuctionCommand> commandEnvelope)
		{
			var command = commandEnvelope.Message;
			var createdAuction = Auction.Create(command.AuctionId, command.Title, command.Description, command.EndDate,
				command.StartingPrice, command.BuyNowPrice, commandEnvelope.SenderUserName, _timeProvider);

			await _auctionsRepository.Create(createdAuction);
		}

		public async Task Handle(CommandEnvelope<FinishAuctionCommand> commandEnvelope)
		{
			var auction = await _auctionsRepository.Get(commandEnvelope.Message.Id);
			auction.Finish();
		}
	}
}
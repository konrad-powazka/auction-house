using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
	public class MakeBidCommandHandler : ICommandHandler<MakeBidCommand>
	{
		private readonly IRepository<Auction> _auctionsRepository;

		public MakeBidCommandHandler(IRepository<Auction> auctionsRepository)
		{
			_auctionsRepository = auctionsRepository;
		}

		public async Task Handle(CommandEnvelope<MakeBidCommand> commandEnvelope)
		{
			var makeBidCommand = commandEnvelope.Message;
			var auction = await _auctionsRepository.Get(makeBidCommand.AuctionId);
			auction.MakeBid(commandEnvelope.SenderUserName, makeBidCommand.Price);

			await
				_auctionsRepository.Save(auction, commandEnvelope.MessageId.ToString(), ExpectedAggregateRootVersion.Specific,
					makeBidCommand.ExpectedAuctionVersion);
		}
	}
}
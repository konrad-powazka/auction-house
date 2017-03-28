using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
	public class FinishAuctionCommandHandler : ICommandHandler<FinishAuctionCommand>
	{
		private readonly IRepository<Auction> _auctionsRepository;

		public FinishAuctionCommandHandler(IRepository<Auction> auctionsRepository)
		{
			_auctionsRepository = auctionsRepository;
		}

		public async Task Handle(ICommandEnvelope<FinishAuctionCommand> commandEnvelope)
		{
			var auction = await _auctionsRepository.Get(commandEnvelope.Command.Id);

			if (auction.WasFinished)
			{
				return;
			}

			auction.Finish();
			await _auctionsRepository.Save(auction, commandEnvelope.CommandId.ToString(), ExpectedAggregateRootVersion.Any);
		}
	}
}
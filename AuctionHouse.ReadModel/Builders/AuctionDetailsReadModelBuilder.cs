using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions.Details;

namespace AuctionHouse.ReadModel.Builders
{
	public class AuctionDetailsReadModelBuilder : IReadModelBuilder
	{
		public async Task Apply(IEvent @event, IReadModelDbContext readModelDbContext)
		{
			TypeSwitch.Do(@event, TypeSwitch.Case<AuctionCreatedEvent>(auctionCreatedEvent =>
			{
				var auctionDetails = new AuctionDetailsReadModel
				{
					Id = auctionCreatedEvent.Id,
					Title = auctionCreatedEvent.Title,
					Description = auctionCreatedEvent.Description,
					EndDate = auctionCreatedEvent.EndDateTime,
					BuyNowPrice = null, // TODO
					WasFinished = false,
					StartingPrice = auctionCreatedEvent.StartingPrice,
					MinimalPriceForNextBidder = auctionCreatedEvent.MinimalPriceForNextBidder
				};

				readModelDbContext.CreateOrOverwrite(auctionDetails, auctionCreatedEvent.Id);
			}), TypeSwitch.Case<AuctionFinishedEvent>(auctionFinishedEvent =>
			{
				//TODO async
				var auctionDetails = readModelDbContext.Get<AuctionDetailsReadModel>(auctionFinishedEvent.AuctionId).Result;
				auctionDetails.WasFinished = true;
				readModelDbContext.CreateOrOverwrite(auctionDetails, auctionFinishedEvent.AuctionId);
			}));
		}
	}
}
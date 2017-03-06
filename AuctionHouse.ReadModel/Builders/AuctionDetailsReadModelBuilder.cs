using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions.Details;

namespace AuctionHouse.ReadModel.Builders
{
	// TODO: Not all data exposed by this RM should be visible to all users
	public class AuctionDetailsReadModelBuilder : IReadModelBuilder
	{
		public async Task Apply(IEvent @event, IReadModelDbContext readModelDbContext)
		{
			//TODO async
			TypeSwitch.Do(@event,
				TypeSwitch.Case<AuctionCreatedEvent>(auctionCreatedEvent =>
				{
					var auctionDetails = new AuctionDetailsReadModel
					{
						Id = auctionCreatedEvent.Id,
						CreatedByUserName = auctionCreatedEvent.CreatedByUserName,
						Title = auctionCreatedEvent.Title,
						Description = auctionCreatedEvent.Description,
						EndDate = auctionCreatedEvent.EndDateTime,
						BuyNowPrice = auctionCreatedEvent.BuyNowPrice,
						WasFinished = false,
						StartingPrice = auctionCreatedEvent.StartingPrice,
						MinimalPriceForNextBidder = auctionCreatedEvent.MinimalPriceForNextBidder,
						HighestBidderUserName = null,
						NumberOfBids = 0
					};

					readModelDbContext.CreateOrOverwrite(auctionDetails, auctionCreatedEvent.Id);
				}),
				TypeSwitch.Case<BidMadeEvent>(bidMadeEvent =>
				{
					var auctionDetails = readModelDbContext.Get<AuctionDetailsReadModel>(bidMadeEvent.AuctionId).Result;
					auctionDetails.HighestBidderUserName = bidMadeEvent.HighestBidderUserName;
					auctionDetails.MinimalPriceForNextBidder = bidMadeEvent.MinimalPriceForNextBidder;
					readModelDbContext.CreateOrOverwrite(auctionDetails, bidMadeEvent.AuctionId);
				}),
				TypeSwitch.Case<AuctionFinishedEvent>(auctionFinishedEvent =>
				{
					var auctionDetails = readModelDbContext.Get<AuctionDetailsReadModel>(auctionFinishedEvent.AuctionId).Result;
					auctionDetails.WasFinished = true;
					readModelDbContext.CreateOrOverwrite(auctionDetails, auctionFinishedEvent.AuctionId);
				}));
		}
	}
}
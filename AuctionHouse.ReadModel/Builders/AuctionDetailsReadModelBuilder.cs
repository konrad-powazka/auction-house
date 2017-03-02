using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions.Details;

namespace AuctionHouse.ReadModel.Builders
{
	public class AuctionDetailsReadModelBuilder : IReadModelBuilder
	{
		public async Task Apply(IEvent @event, IReadModelRepository readModelRepository)
		{
			if (@event is AuctionCreatedEvent)
			{
				var auctionDetails = new AuctionDetailsReadModel();
				var auctionCreatedEvent = (AuctionCreatedEvent)@event;
				auctionDetails.Id = auctionCreatedEvent.Id;
				auctionDetails.Title = auctionCreatedEvent.Title;
				auctionDetails.Description = auctionCreatedEvent.Description;
				await readModelRepository.Create(auctionDetails, auctionCreatedEvent.Id);
			}
		}
	}
}
﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Persistence.Shared;
using AuctionHouse.ReadModel.Dtos.Auctions;

namespace AuctionHouse.ReadModel.Builders
{
	// TODO: Not all data exposed by this RM should be visible to all users
	public class AuctionDetailsReadModelBuilder : IReadModelBuilder
	{
		public async Task Apply(PersistedEventEnvelope eventEnvelope, IReadModelDbContext readModelDbContext)
		{
			Guid auctionId;

			if (StreamNameGenerator.TryExtractAuctionId(eventEnvelope.StreamName, out auctionId))
			{
				var readModel = await readModelDbContext.TryGet<AuctionDetailsReadModel>(auctionId.ToString());

				if (readModel != null)
				{
					readModel.Version = eventEnvelope.StreamVersion;
				}
			}

			//TODO async
			TypeSwitch.Do(eventEnvelope.Event,
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
						CurrentPrice = auctionCreatedEvent.CurrentPrice,
						HighestBidderUserName = null,
						NumberOfBids = 0,
						CreatedDateTime = auctionCreatedEvent.CreatedDateTime
					};

					readModelDbContext.CreateOrOverwrite(auctionDetails, auctionCreatedEvent.Id.ToString());
				}),
				TypeSwitch.Case<BidMadeEvent>(bidMadeEvent =>
				{
					var auctionDetails = readModelDbContext.Get<AuctionDetailsReadModel>(bidMadeEvent.AuctionId.ToString()).Result;
					auctionDetails.HighestBidderUserName = bidMadeEvent.HighestBidderUserName;
					auctionDetails.MinimalPriceForNextBidder = bidMadeEvent.MinimalPriceForNextBidder;
					auctionDetails.NumberOfBids++;
					auctionDetails.BiddersUserNames.Add(bidMadeEvent.BidderUserName);
					auctionDetails.CurrentPrice = bidMadeEvent.CurrentPrice;
					readModelDbContext.CreateOrOverwrite(auctionDetails, bidMadeEvent.AuctionId.ToString());
				}),
				TypeSwitch.Case<AuctionFinishedEvent>(auctionFinishedEvent =>
				{
					var auctionDetails =
						readModelDbContext.Get<AuctionDetailsReadModel>(auctionFinishedEvent.AuctionId.ToString()).Result;

					auctionDetails.WasFinished = true;
					auctionDetails.FinishedDateTime = auctionFinishedEvent.FinishedDateTime;
					readModelDbContext.CreateOrOverwrite(auctionDetails, auctionFinishedEvent.AuctionId.ToString());
				}));
		}
	}
}
using System;
using AuctionHouse.Core.Domain;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.Money;
using AuctionHouse.Messages.Events.Auctions;

namespace AuctionHouse.Domain.Auctions
{
	public class Auction : AggregateRoot
	{
		private readonly ITimeProvider _timeProvider;

		public Auction(ITimeProvider timeProvider)
		{
			_timeProvider = timeProvider;
		}

		public string Description { get; private set; }

		public string Title { get; private set; }

		public decimal StartingPrice { get; private set; }

		public decimal MinimalPriceForNextBidder { get; private set; }

		public decimal? HighestBidPrice { get; private set; }

		public string HighestBidderUserName { get; private set; }

		public string CreatedByUserName { get; private set; }

		public DateTime EndDateTime { get; private set; }

		public bool IsInProgress => !WasFinished && _timeProvider.Now < EndDateTime;

		public bool WasFinished { get; private set; }

		public static Auction Create(Guid id, string title, string description, DateTime endDate, decimal startingPrice,
			decimal? buyNowPrice, string createdByUserName, ITimeProvider timeProvider)
		{
			MoneyValidator.ValidateMoneyAmount(startingPrice);

			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
			}

			if (endDate <= timeProvider.Now)
			{
				throw new ArgumentOutOfRangeException(nameof(endDate));
			}

			if (buyNowPrice.HasValue)
			{
				MoneyValidator.ValidateMoneyAmount(buyNowPrice.Value);

				if (buyNowPrice < startingPrice)
				{
					throw new ArgumentOutOfRangeException(nameof(buyNowPrice));
				}
			}

			var auction = new Auction(timeProvider);

			var auctionCreatedEvent = new AuctionCreatedEvent
			{
				Id = id,
				Title = title,
				Description = description,
				StartingPrice = startingPrice,
				MinimalPriceForNextBidder = startingPrice,
				CreatedByUserName = createdByUserName,
				EndDateTime = endDate,
				BuyNowPrice = buyNowPrice
			};

			auction.ApplyChange(auctionCreatedEvent);

			return auction;
		}

		public void MakeBid(string bidderUserName, decimal bidPrice)
		{
			MoneyValidator.ValidateMoneyAmount(bidPrice);

			if (!IsInProgress)
			{
				throw new InvalidOperationException("An auction must be in progress in order to bid.");
			}

			if (bidPrice < MinimalPriceForNextBidder)
			{
				throw new ArgumentOutOfRangeException(nameof(bidPrice), "Bid price is too low.");
			}

			if (bidderUserName == CreatedByUserName)
			{
				throw new ArgumentException(nameof(bidderUserName), "A user cannot make bids in his own auction.");
			}

			var newBidIsHighest = !HighestBidPrice.HasValue || bidPrice > HighestBidPrice;
			var newHighestBidPrice = newBidIsHighest ? bidPrice : HighestBidPrice.Value;
			var newHighestBidderUserName = newBidIsHighest ? bidderUserName : HighestBidderUserName;

			var newMinimalPriceForNextBidderReferencePrice = newBidIsHighest
				? (HighestBidPrice ?? StartingPrice)
				: bidPrice;

			var bidMadeEvent = new BidMadeEvent
			{
				BidderUserName = bidderUserName,
				MinimalPriceForNextBidder = GetNewMinimalPriceForNextBidder(newMinimalPriceForNextBidderReferencePrice),
				HighestBidderUserName = newHighestBidderUserName,
				HighestBidPrice = newHighestBidPrice,
				AuctionId = Id,
				BidPrice = bidPrice
			};

			ApplyChange(bidMadeEvent);
		}

		private static decimal GetNewMinimalPriceForNextBidder(decimal referencePrice)
		{
			// TODO: Implement progressive price increase
			return referencePrice + 0.01m;
		}

		protected override void RegisterEventAppliers()
		{
			RegisterEventApplier<AuctionCreatedEvent>(Apply);
			RegisterEventApplier<BidMadeEvent>(Apply);
			RegisterEventApplier<AuctionFinishedEvent>(Apply);
		}

		private void Apply(AuctionCreatedEvent auctionCreatedEvent)
		{
			Id = auctionCreatedEvent.Id;
			Title = auctionCreatedEvent.Title;
			Description = auctionCreatedEvent.Description;
			StartingPrice = auctionCreatedEvent.StartingPrice;
			CreatedByUserName = auctionCreatedEvent.CreatedByUserName;
			EndDateTime = auctionCreatedEvent.EndDateTime;
		}

		private void Apply(BidMadeEvent bidMadeEvent)
		{
			MinimalPriceForNextBidder = bidMadeEvent.MinimalPriceForNextBidder;
			HighestBidPrice = bidMadeEvent.HighestBidPrice;
			HighestBidderUserName = bidMadeEvent.HighestBidderUserName;
		}

		private void Apply(AuctionFinishedEvent auctionFinishedEvent)
		{
			WasFinished = true;
		}

		public void Finish()
		{
			if (WasFinished)
			{
				throw new InvalidOperationException("The auction has already been finished.");
			}

			ApplyChange(new AuctionFinishedEvent
			{
				AuctionId = Id
			});
		}
	}
}
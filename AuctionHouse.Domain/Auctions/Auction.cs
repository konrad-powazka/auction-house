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

		public decimal CurrentPrice { get; set; }

		public decimal MinimalPriceForNextBidder { get; private set; }

		public decimal? HighestBidPrice { get; private set; }

		public string HighestBidderUserName { get; private set; }

		public string CreatedByUserName { get; private set; }

		public DateTime EndDateTime { get; private set; }

		public bool IsInProgress => !WasFinished && _timeProvider.UtcNow < EndDateTime;

		public bool WasFinished { get; private set; }

		public decimal? BuyNowPrice { get; private set; }

		public static Auction Create(Guid id, string title, string description, DateTime endDate, decimal startingPrice,
			decimal? buyNowPrice, string createdByUserName, ITimeProvider timeProvider)
		{
			MoneyValidator.ValidateMoneyAmount(startingPrice);

			if (string.IsNullOrWhiteSpace(title))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
			}

			if (endDate <= timeProvider.UtcNow)
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
				MinimalPriceForNextBidder = Math.Max(startingPrice, 0.01m),
				CurrentPrice = startingPrice,
				CreatedByUserName = createdByUserName,
				EndDateTime = endDate,
				BuyNowPrice = buyNowPrice,
				CreatedDateTime = timeProvider.UtcNow
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
				throw new ArgumentException("A user cannot make bids at his own auction.", nameof(bidderUserName));
			}

			var newBidIsHighest = !HighestBidPrice.HasValue || bidPrice > HighestBidPrice;
			var newHighestBidPrice = newBidIsHighest ? bidPrice : HighestBidPrice.Value;
			var newHighestBidderUserName = newBidIsHighest ? bidderUserName : HighestBidderUserName;
			decimal newMinimalPriceForNextBidder;
			decimal newCurrentPrice;

			if (bidderUserName == HighestBidderUserName)
			{
				newMinimalPriceForNextBidder = MinimalPriceForNextBidder;
				newCurrentPrice = CurrentPrice;
			}
			else
			{
				newCurrentPrice = newBidIsHighest
					? (HighestBidPrice ?? StartingPrice)
					: bidPrice;

				if (BuyNowPrice.HasValue)
				{
					newCurrentPrice = Math.Min(newCurrentPrice, BuyNowPrice.Value);
				}

				newMinimalPriceForNextBidder = GetNewMinimalPriceForNextBidder(newCurrentPrice);
			}

			var bidMadeEvent = new BidMadeEvent
			{
				BidderUserName = bidderUserName,
				MinimalPriceForNextBidder = newMinimalPriceForNextBidder,
				HighestBidderUserName = newHighestBidderUserName,
				HighestBidPrice = newHighestBidPrice,
				AuctionId = Id,
				BidPrice = bidPrice,
				CurrentPrice = newCurrentPrice,
				BidDateTime = _timeProvider.UtcNow
			};

			ApplyChange(bidMadeEvent);

			if (BuyNowPrice.HasValue && HighestBidPrice >= BuyNowPrice)
			{
				ApplyChange(new AuctionFinishedEvent
				{
					AuctionId = Id,
					FinishedDateTime = _timeProvider.UtcNow
				});
			}
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
			BuyNowPrice = auctionCreatedEvent.BuyNowPrice;
			CurrentPrice = auctionCreatedEvent.CurrentPrice;
		}

		private void Apply(BidMadeEvent bidMadeEvent)
		{
			MinimalPriceForNextBidder = bidMadeEvent.MinimalPriceForNextBidder;
			HighestBidPrice = bidMadeEvent.HighestBidPrice;
			HighestBidderUserName = bidMadeEvent.HighestBidderUserName;
			CurrentPrice = bidMadeEvent.CurrentPrice;
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
				AuctionId = Id,
				FinishedDateTime = EndDateTime
			});
		}
	}
}
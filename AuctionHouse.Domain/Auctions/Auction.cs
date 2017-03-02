using System;
using AuctionHouse.Core.Time;
using AuctionHouse.Messages.Events.Auctions;

namespace AuctionHouse.Domain.Auctions
{
    public class Auction : AggregateRoot
    {
        public static Auction Create(Guid id, string title, string description, DateTime endDate, decimal startingPrice,
            decimal? buyNowPrice, string createdByUserName, ITimeProvider timeProvider)
        {
	        if (string.IsNullOrWhiteSpace(title))
	        {
		        throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
	        }

            //TODO: Mockable date
            if (endDate <= timeProvider.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate));
            }

	        var auction = new Auction();

            var auctionCreatedEvent = new AuctionCreatedEvent
            {
                Id = id,
                Title = title,
                Description = description,
                StartingPrice = startingPrice,
                MinimalPriceForNextBidder = startingPrice,
                CreatedByUserName = createdByUserName
            };

            auction.ApplyChange(auctionCreatedEvent);

            return auction;
        }

        public string Description { get; private set; }

        public string Title { get; private set; }

        public decimal StartingPrice { get; private set; }

        public AuctionState State { get; private set; }

        public decimal MinimalPriceForNextBidder { get; private set; }

        public decimal? HighestBidPrice { get; private set; }

        public string HighestBidderUserName { get; private set; }

        public string CreatedByUserName { get; private set; }

        public void MakeBid(string bidderUserName, decimal bidPrice)
        {
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
        }

        private void Apply(AuctionCreatedEvent auctionCreatedEvent)
        {
            Id = auctionCreatedEvent.Id;
            Title = auctionCreatedEvent.Title;
            Description = auctionCreatedEvent.Description;
            StartingPrice = auctionCreatedEvent.StartingPrice;
            CreatedByUserName = auctionCreatedEvent.CreatedByUserName;
        }

        private void Apply(BidMadeEvent bidMadeEvent)
        {
            MinimalPriceForNextBidder = bidMadeEvent.MinimalPriceForNextBidder;
            HighestBidPrice = bidMadeEvent.HighestBidPrice;
            HighestBidderUserName = bidMadeEvent.HighestBidderUserName;
        }
    }
}
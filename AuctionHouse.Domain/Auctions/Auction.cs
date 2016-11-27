using System;
using AuctionHouse.Messages.Events.Auctions;

namespace AuctionHouse.Domain.Auctions
{
    public class Auction : AggregateRoot
    {
        public static Auction Create(Guid id, string title, string description, DateTime endDate, decimal startingPrice,
            decimal? buyNowPrice)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException(null, nameof(title));
            }

            //TODO: Mockable date
            if (endDate <= DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate));
            }

            var auction = new Auction();

            var auctionCreatedEvent = new AuctionCreatedEvent
            {
                AuctionId = id,
                Title = title,
                Description = description,
                Price = startingPrice
            };

            auction.ApplyChange(auctionCreatedEvent);

            return auction;
        }

        public string Description { get; private set; }

        public string Title { get; private set; }

        public AuctionState State { get; private set; }

        protected override void RegisterEventAppliers()
        {
            RegisterEventApplier<AuctionCreatedEvent>(Apply);
        }

        private void Apply(AuctionCreatedEvent auctionCreatedEvent)
        {
            Id = auctionCreatedEvent.AuctionId;
            Title = auctionCreatedEvent.Title;
            Description = auctionCreatedEvent.Description;
        }
    }
}
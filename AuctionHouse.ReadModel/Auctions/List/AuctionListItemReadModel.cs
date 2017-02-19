using System;

namespace AuctionHouse.ReadModel.Auctions.List
{
    public class AuctionListItemReadModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EndDate;
        public decimal StartingPrice;
        public decimal? BuyNowPrice;
        public decimal? MinimalPriceForNextBidder;
    }
}
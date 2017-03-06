using System;

namespace AuctionHouse.ReadModel.Dtos.Auctions.List
{
    public class AuctionListItemReadModel
    {
        public Guid Id { get; set; }
		public string CreatedByUserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
		public decimal? BuyNowPrice { get; set; }
		public decimal? MinimalPriceForNextBidder { get; set; }
		public bool WasFinished { get; set; }
		public int NumberOfBids { get; set; }
    }
}
using AuctionHouse.ReadModel.Dtos.Auctions.List;

namespace AuctionHouse.ReadModel.Dtos.Auctions.Details
{
    public class AuctionDetailsReadModel : AuctionListItemReadModel
    {
		public decimal StartingPrice { get; set; }
		public string HighestBidderUserName { get; set; }
	}
}
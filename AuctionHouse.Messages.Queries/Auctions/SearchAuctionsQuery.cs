using AuctionHouse.Core.Messaging;
using AuctionHouse.ReadModel.Dtos.Auctions.List;

namespace AuctionHouse.Messages.Queries.Auctions
{
	public class SearchAuctionsQuery : IQuery<AuctionsListReadModel>
	{
		public string QueryString { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}
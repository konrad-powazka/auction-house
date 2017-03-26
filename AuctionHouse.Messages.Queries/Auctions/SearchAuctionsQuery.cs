using AuctionHouse.Core.Paging;
using AuctionHouse.ReadModel.Dtos.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions.List;

namespace AuctionHouse.Messages.Queries.Auctions
{
	public class SearchAuctionsQuery : IPagedQuery<AuctionsListReadModel, AuctionListItemReadModel>
	{
		public string QueryString { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}
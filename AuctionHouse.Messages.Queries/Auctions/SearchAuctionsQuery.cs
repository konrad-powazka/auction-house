using AuctionHouse.Core.Messaging;
using AuctionHouse.ReadModel.Dtos.Auctions.List;

namespace AuctionHouse.Messages.Queries.Auctions
{
	public class SearchAuctionsQuery : IQuery<AuctionsListReadModel>
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}
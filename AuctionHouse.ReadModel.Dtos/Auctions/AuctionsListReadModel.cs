using System.Collections.Generic;
using AuctionHouse.Core.Paging;

namespace AuctionHouse.ReadModel.Dtos.Auctions
{
	public class AuctionsListReadModel : IPagedResult<AuctionListItemReadModel>
	{
		public int PageNumber { get; set; }
		public long TotalPagesCount { get; set; }
		public long TotalItemsCount { get; set; }
		public IReadOnlyCollection<AuctionListItemReadModel> PageItems { get; set; }
		public int PageSize { get; set; }
	}
}
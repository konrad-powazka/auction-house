using System.Collections.Generic;

namespace AuctionHouse.Core.Paging
{
	public interface IPagedResult<TItem>
	{
		int PageNumber { get; set; }
		long TotalPagesCount { get; set; }
		long TotalItemsCount { get; set; }
		IReadOnlyCollection<TItem> PageItems { get; set; }
		int PageSize { get; set; }
	}
}
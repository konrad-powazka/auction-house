using System;
using System.Collections.Generic;
using System.Linq;

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
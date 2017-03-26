using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.Paging
{
	public interface IPagedQuery<TResult, TItem> : IQuery<TResult> where TResult : IPagedResult<TItem>
	{
		int PageSize { get; set; }
		int PageNumber { get; set; }
	}
}
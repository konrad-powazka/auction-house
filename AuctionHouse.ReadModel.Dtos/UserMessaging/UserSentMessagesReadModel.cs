using System.Collections.Generic;
using AuctionHouse.Core.Paging;

namespace AuctionHouse.ReadModel.Dtos.UserMessaging
{
	public class UserSentMessagesReadModel : IPagedResult<UserMessageReadModel>
	{
		public int PageNumber { get; set; }
		public long TotalPagesCount { get; set; }
		public long TotalItemsCount { get; set; }
		public IReadOnlyCollection<UserMessageReadModel> PageItems { get; set; }
		public int PageSize { get; set; }
	}
}
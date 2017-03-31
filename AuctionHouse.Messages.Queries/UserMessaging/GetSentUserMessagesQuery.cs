using AuctionHouse.Core.Paging;
using AuctionHouse.ReadModel.Dtos.UserMessaging;

namespace AuctionHouse.Messages.Queries.UserMessaging
{
	public class GetSentUserMessagesQuery : IPagedQuery<UserSentMessagesReadModel, UserMessageReadModel>
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}
using AuctionHouse.Core.Paging;
using AuctionHouse.ReadModel.Dtos.Auctions;

namespace AuctionHouse.Messages.Queries.Auctions
{
	public class GetAuctionsInvolvingUserQuery : IPagedQuery<AuctionsListReadModel, AuctionListItemReadModel>
	{
		public string QueryString { get; set; }
		public UserInvolvementIntoAuction? UserInvolvementIntoAuction { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}
}
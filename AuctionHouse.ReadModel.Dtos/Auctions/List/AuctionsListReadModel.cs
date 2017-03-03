using System.Collections.Generic;

namespace AuctionHouse.ReadModel.Dtos.Auctions.List
{
    public class AuctionsListReadModel
    {
		public int PageNumber { get; set; }
		public long TotalPagesCount { get; set; }
		public long TotalItemsCount { get; set; }
        public IReadOnlyCollection<AuctionListItemReadModel> PageItems { get; set; }
	    public int PageSize { get; set; }
    }
}
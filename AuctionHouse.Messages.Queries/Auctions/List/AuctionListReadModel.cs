using System.Collections.Generic;

namespace AuctionHouse.Messages.Queries.Auctions.List
{
    public class AuctionListReadModel
    {
        public IReadOnlyCollection<AuctionListItemReadModel> Auctions { get; set; }
    }
}
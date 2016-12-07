using System.Collections.Generic;

namespace AuctionHouse.ReadModel.Auctions.List
{
    public class AuctionListReadModel
    {
        public IReadOnlyCollection<AuctionListItemReadModel> Auctions { get; set; }
    }
}
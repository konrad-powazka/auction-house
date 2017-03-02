using System.Collections.Generic;

namespace AuctionHouse.ReadModel.Dtos.Auctions.List
{
    public class AuctionListReadModel
    {
        public IReadOnlyCollection<AuctionListItemReadModel> Auctions { get; set; }
    }
}
using System;
using AuctionHouse.Core.Messaging;
using AuctionHouse.ReadModel.Auctions.Details;

namespace AuctionHouse.Messages.Queries.Auctions
{
    public class GetAuctionDetailsQuery : IQuery<AuctionDetailsReadModel>
    {
        public Guid Id { get; set; }
    }
}

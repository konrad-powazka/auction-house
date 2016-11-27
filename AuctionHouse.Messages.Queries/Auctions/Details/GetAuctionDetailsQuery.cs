using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Queries.Auctions.Details
{
    public class GetAuctionDetailsQuery : IQuery<AuctionDetailsReadModel>
    {
        public Guid Id { get; set; }
    }
}

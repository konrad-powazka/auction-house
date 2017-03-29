using System;
using AuctionHouse.Core.Messaging;
using AuctionHouse.ReadModel.Dtos.Auctions;

namespace AuctionHouse.Messages.Queries.Auctions
{
    public class GetAuctionDetailsQuery : IQuery<AuctionDetailsReadModel>
    {
        public Guid Id { get; set; }
    }
}
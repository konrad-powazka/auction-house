using System;

namespace AuctionHouse.Web.Models
{
    public class NotifyOnQueryResultChangedResponse
    {
        public NotifyOnQueryResultChangedResponse(Guid subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }

        public Guid SubscriptionId { get; }
    }
}
using System;

namespace AuctionHouse.Web.Models
{
    public class NotifyOnEventsAppliedToReadModelResponse
    {
        public NotifyOnEventsAppliedToReadModelResponse(bool wereAllEventsAlreadyApplied, Guid? subscriptionId)
        {
            if (!wereAllEventsAlreadyApplied && !subscriptionId.HasValue)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }

            if (wereAllEventsAlreadyApplied && subscriptionId.HasValue)
            {
                throw new ArgumentException(nameof(subscriptionId));
            }

            SubscriptionId = subscriptionId;
            WereAllEventsAlreadyApplied = wereAllEventsAlreadyApplied;
        }

        public Guid? SubscriptionId { get; }
        public bool WereAllEventsAlreadyApplied { get; }
    }
}
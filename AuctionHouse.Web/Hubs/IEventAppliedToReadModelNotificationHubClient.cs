using System;
using System.Threading.Tasks;

namespace AuctionHouse.Web.Hubs
{
    public interface IEventAppliedToReadModelNotificationHubClient
    {
        Task HandleEventsAppliedToReadModel(Guid subscriptionId);
    }
}
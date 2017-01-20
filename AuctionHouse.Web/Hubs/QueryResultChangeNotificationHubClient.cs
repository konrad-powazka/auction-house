using System;

namespace AuctionHouse.Web.Hubs
{
    public interface IQueryResultChangeNotificationHubClient
    {
        void HandleQueryResultChanged<TQueryResult>(Guid subscriptionId, TQueryResult queryResult);
    }
}
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.DynamicTypeScanning;
using AuctionHouse.Web.Models;
using Autofac;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

// Code below is thread safe if an assumption is true that for a given connection OnDisconnected can be invoked only
// if there aren't any public hub methods being invoked simultaneously

namespace AuctionHouse.Web.Hubs
{
    public class QueryResultChangeNotificationHub : Hub<IQueryResultChangeNotificationHubClient>
    {
        private static readonly IHubContext<IQueryResultChangeNotificationHubClient> HubContext =
            GlobalHost.ConnectionManager
                .GetHubContext<CommandHandlingFeedbackHub, IQueryResultChangeNotificationHubClient>();

        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, IDisposable>>
            ConnectionIdToSubscriptionIdToSubscriptionMapMap =
                new ConcurrentDictionary<string, ConcurrentDictionary<Guid, IDisposable>>();

        private readonly IComponentContext _componentContext;

        public QueryResultChangeNotificationHub(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        // TODO: This is ugly and slow, try to use dynamic generics as with controllers
        public NotifyOnQueryResultChangedResponse NotifyOnResultChanged(string queryName, string serializedQuery)
        {
            var queryType = DynamicTypeScanner.GetQueryTypes().Single(t => t.Name == queryName);
            var query = JsonConvert.DeserializeObject(serializedQuery, queryType);

            var queryResultType =
                queryType.GetInterfaces()
                    .Single(
                        i =>
                            i.GetGenericTypeDefinition() == typeof(IEventAppliedNotifyingQueryHandler<,>) &&
                            i.GetGenericArguments().Any() && i.GetGenericArguments().First() == queryType)
                    .GetGenericArguments()
                    .First();

            var notifyOnResultChangedMethod = GetType()
                .GetMethod(nameof(NotifyOnResultChanged),
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .MakeGenericMethod(queryType, queryResultType);

            return (NotifyOnQueryResultChangedResponse) notifyOnResultChangedMethod.Invoke(this, new[] {query});
        }

        private NotifyOnQueryResultChangedResponse NotifyOnResultChanged<TQuery, TQueryResult>(TQuery query)
            where TQuery : IQuery<TQueryResult>
        {
            var resultChangedNotifyingQueryHandler =
                _componentContext.Resolve<IEventAppliedNotifyingQueryHandler<TQuery, TQueryResult>>();

            var connectionId = Context.ConnectionId;
            var subscriptionId = Guid.NewGuid();
            IDisposable subscription = null;

            try
            {
                subscription = resultChangedNotifyingQueryHandler.AddQueryResultChangedHandler(query,
                    queryResult =>
                    {
                        var client = HubContext.Clients.Client(connectionId);
                        client.HandleQueryResultChanged(subscriptionId, queryResult);
                    });

                var clientSubscriptionIdToSubscriptionMap =
                    ConnectionIdToSubscriptionIdToSubscriptionMapMap[connectionId];

                if (!clientSubscriptionIdToSubscriptionMap.TryAdd(subscriptionId, subscription))
                {
                    throw new InvalidOperationException();
                }
            }
            catch
            {
                subscription?.Dispose();
                throw;
            }

            return new NotifyOnQueryResultChangedResponse(subscriptionId);
        }

        public void CancelNotificationOnResultChanged(Guid subscriptionId)
        {
            var clientSubscriptionIdToSubscriptionMap =
                ConnectionIdToSubscriptionIdToSubscriptionMapMap[Context.ConnectionId];

            IDisposable subscription;

            if (!clientSubscriptionIdToSubscriptionMap.TryRemove(subscriptionId, out subscription))
            {
                throw new ArgumentException(nameof(subscriptionId));
            }

            subscription.Dispose();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await base.OnDisconnected(stopCalled);

            ConcurrentDictionary<Guid, IDisposable> clientSubscriptionIdToSubscriptionMap;

            if (
                !ConnectionIdToSubscriptionIdToSubscriptionMapMap.TryRemove(Context.ConnectionId,
                    out clientSubscriptionIdToSubscriptionMap))
            {
                throw new InvalidOperationException();
            }

            // If it's possible for NotifyOnResultChanged to be invoked simultaneously
            // then newly added subscription will never be disposed
            foreach (var subscription in clientSubscriptionIdToSubscriptionMap.Values)
            {
                subscription.Dispose();
            }
        }

        public override async Task OnConnected()
        {
            await base.OnConnected();

            if (
                !ConnectionIdToSubscriptionIdToSubscriptionMapMap.TryAdd(Context.ConnectionId,
                    new ConcurrentDictionary<Guid, IDisposable>()))
            {
                throw new InvalidOperationException();
            }
        }
    }
}
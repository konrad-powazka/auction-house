using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Web.EventSourcing;
using AuctionHouse.Web.Models;
using Microsoft.AspNet.SignalR;

// Code below is thread safe if an assumption is true that for a given connection OnDisconnected can be invoked only
// if there aren't any public hub methods being invoked simultaneously
namespace AuctionHouse.Web.Hubs
{
    public class EventAppliedToReadModelNotificationHub : Hub<IEventAppliedToReadModelNotificationHubClient>
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Subscription>>
            ConnectionIdToSubscriptionIdToSubscriptionMapMap =
                new ConcurrentDictionary<string, ConcurrentDictionary<Guid, Subscription>>();

	    private readonly IEventsAppliedToReadModelTracker _eventsAppliedToReadModelTracker;
	    private readonly IHubContext<IEventAppliedToReadModelNotificationHubClient> _hubContext;

        public EventAppliedToReadModelNotificationHub(
			IEventsAppliedToReadModelTracker eventsAppliedToReadModelTracker,
			IHubContext<IEventAppliedToReadModelNotificationHubClient> hubContext)
        {
	        _eventsAppliedToReadModelTracker = eventsAppliedToReadModelTracker;
	        _hubContext = hubContext;
        }

	    public NotifyOnEventsAppliedToReadModelResponse NotifyOnEventsApplied(
            IReadOnlyCollection<Guid> eventIds)
        {
            if (eventIds == null)
            {
                throw new ArgumentNullException(nameof(eventIds));
            }

            var unappliedEventIds =
                eventIds.Where(i => !_eventsAppliedToReadModelTracker.CheckIfEventWasApplied(i)).ToArray();

            if (!unappliedEventIds.Any())
            {
                return new NotifyOnEventsAppliedToReadModelResponse(true, null);
            }

            var connectionId = Context.ConnectionId;
            var subscriptionId = Guid.NewGuid();

			// TODO: There is a race condition
            var subscription = new Subscription(subscriptionId, connectionId, unappliedEventIds,
				_eventsAppliedToReadModelTracker, _hubContext);

            var clientSubscriptionIdToSubscriptionMap =
                ConnectionIdToSubscriptionIdToSubscriptionMapMap[connectionId];

            if (!clientSubscriptionIdToSubscriptionMap.TryAdd(subscriptionId, subscription))
            {
                throw new InvalidOperationException();
            }

            subscription.Start(() =>
            {
                Subscription unused;

                if (!clientSubscriptionIdToSubscriptionMap.TryRemove(subscriptionId, out unused))
                {
                    throw new InvalidOperationException();
                }
            });

            return new NotifyOnEventsAppliedToReadModelResponse(false, subscriptionId);
        }

        public void CancelNotificationOnEventsApplied(Guid subscriptionId)
        {
            var clientSubscriptionIdToSubscriptionMap =
                ConnectionIdToSubscriptionIdToSubscriptionMapMap[Context.ConnectionId];

            Subscription subscription;

            if (!clientSubscriptionIdToSubscriptionMap.TryRemove(subscriptionId, out subscription))
            {
                throw new ArgumentException(nameof(subscriptionId));
            }

            subscription.Stop();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            await base.OnDisconnected(stopCalled);

            ConcurrentDictionary<Guid, Subscription> clientSubscriptionIdToSubscriptionMap;

            if (
                !ConnectionIdToSubscriptionIdToSubscriptionMapMap.TryRemove(Context.ConnectionId,
                    out clientSubscriptionIdToSubscriptionMap))
            {
	            return;
            }

            // If it's possible for NotifyOnEventsAppliedToReadModel to be invoked simultaneously
            // then newly added subscription will not be stopped
            foreach (var subscription in clientSubscriptionIdToSubscriptionMap.Values)
            {
                subscription.Stop();
            }
        }

        public override async Task OnConnected()
        {
            await base.OnConnected();

            if (
                !ConnectionIdToSubscriptionIdToSubscriptionMapMap.TryAdd(Context.ConnectionId,
                    new ConcurrentDictionary<Guid, Subscription>()))
            {
                throw new InvalidOperationException();
            }
        }

        private class Subscription : IDisposable
        {
            private readonly string _connectionId;
	        private readonly IEventsAppliedToReadModelTracker _eventsAppliedToReadModelTracker;
	        private readonly IHubContext<IEventAppliedToReadModelNotificationHubClient> _hubContext;
            private readonly ConcurrentDictionary<Guid, object> _eventIdsToNotifyOnReadModelApplication;
            private readonly Guid _id;
            private readonly object _startLock = new object();
            private readonly object _stopLock = new object();
            private bool _hasStopped;
            private bool _hasStarted;
            private Action _actionOnStopped;

            public Subscription(Guid id, string connectionId, IEnumerable<Guid> eventIdsToNotifyOnReadModelApplication,
				IEventsAppliedToReadModelTracker eventsAppliedToReadModelTracker, IHubContext<IEventAppliedToReadModelNotificationHubClient> hubContext)
            {
                _id = id;
                _connectionId = connectionId;
	            _eventsAppliedToReadModelTracker = eventsAppliedToReadModelTracker;
                _hubContext = hubContext;

                _eventIdsToNotifyOnReadModelApplication =
                    new ConcurrentDictionary<Guid, object>(eventIdsToNotifyOnReadModelApplication.ToDictionary(i => i,
                        i => (object) null));
            }

            public void Start(Action actionOnStopped)
            {
                if (_hasStarted)
                {
                    throw new InvalidOperationException();
                }

                lock (_startLock)
                {
                    if (_hasStarted)
                    {
                        throw new InvalidOperationException();
                    }

                    _actionOnStopped = actionOnStopped;
					_eventsAppliedToReadModelTracker.EventApplied += EventAppliedToReadModel;
                    _hasStarted = true;
                }
            }

            private void EventAppliedToReadModel(object sender, EventAppliedEventArgs eventAppliedEventArgs)
            {
                object value;

                if (
                    _eventIdsToNotifyOnReadModelApplication.TryRemove(
                        eventAppliedEventArgs.AppliedEventId, out value) &&
                    _eventIdsToNotifyOnReadModelApplication.Count == 0)
                {
                    Stop(false);
                }
            }

            public void Stop()
            {
                Stop(true);
            }

            private void Stop(bool isPremature)
            {
                if (!_hasStarted)
                {
                    lock (_startLock)
                    {
                        if (!_hasStarted)
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }

                if (_hasStopped)
                {
                    return;
                }

                lock (_stopLock)
                {
                    if (_hasStopped)
                    {
                        return;
                    }

					_eventsAppliedToReadModelTracker.EventApplied -= EventAppliedToReadModel;

                    if (!isPremature)
                    {
                        var client = _hubContext.Clients.Client(_connectionId);
                        client.HandleEventsAppliedToReadModel(_id);
                    }

                    _actionOnStopped?.Invoke();
                    _hasStopped = true;
                }
            }

            public void Dispose()
            {
                Stop();
            }
        }
    }
}
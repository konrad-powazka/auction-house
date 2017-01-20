using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace AuctionHouse.Web.Cqrs.Queries
{
    public class EventStoreSubscriber : IDisposable
    {
        private readonly IEnumerable<IEventSourcedBuilder> _eventSourcedBuilders;
        private readonly IEventStoreConnection _eventStoreConnection;
        private volatile bool _hasLiveProcessingStarted;
        private EventStoreAllCatchUpSubscription _subscription;

        public EventStoreSubscriber(IEventStoreConnection eventStoreConnection,
            IEnumerable<IEventSourcedBuilder> eventSourcedBuilders)
        {
            _eventStoreConnection = eventStoreConnection;
            _eventSourcedBuilders = eventSourcedBuilders;
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            _subscription = _eventStoreConnection.SubscribeToAllFrom(Position.Start, CatchUpSubscriptionSettings.Default,
                (s, e) => HandleEventStoreEvent(e),
                s => { _hasLiveProcessingStarted = true; });
        }

        private void HandleEventStoreEvent(ResolvedEvent eventStoreEvent)
        {
            var textSerializedEvent = Encoding.UTF8.GetString(eventStoreEvent.Event.Data);

            //TODO: Create a cached dictionary
            var eventType =
                typeof(EventsAssemblyMarker).Assembly.GetTypes()
                    .Single(t => t.Name == eventStoreEvent.Event.EventType);

            var @event = (IEvent) JsonConvert.DeserializeObject(textSerializedEvent, eventType);

            HandleEvent(@event);
        }

        private void HandleEvent(IEvent @event)
        {
            foreach (var eventSourcedBuilder in _eventSourcedBuilders)
            {
                eventSourcedBuilder.Apply(@event, _hasLiveProcessingStarted);
            }
        }

        public void Stop()
        {
            _subscription.Stop();
        }
    }
}
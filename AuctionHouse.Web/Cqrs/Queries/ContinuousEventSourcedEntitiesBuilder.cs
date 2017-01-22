using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Events;
using AuctionHouse.Persistence;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace AuctionHouse.Web.Cqrs.Queries
{
    public class ContinuousEventSourcedEntitiesBuilder : IContinuousEventSourcedEntitiesBuilder
    {
        private readonly IEventsDatabase _eventsDatabase;
        private readonly IEnumerable<IEventSourcedEntity> _eventSourcedBuilders;
        private IDisposable _subscription;
        private bool _wasDisposed = false;

        public event EventHandler<EventAppliedEventArgs> EventApplied;

        public ContinuousEventSourcedEntitiesBuilder(IEventsDatabase eventsDatabase,
            IEnumerable<IEventSourcedEntity> eventSourcedBuilders)
        {
            _eventsDatabase = eventsDatabase;
            _eventSourcedBuilders = eventSourcedBuilders;
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            if (_wasDisposed)
            {
                throw new InvalidOperationException();
            }

            _subscription = _eventsDatabase.ReadAllExistingEventsAndSubscribe(HandleEventEnvelope);
        }

        private void HandleEventEnvelope(MessageEnvelope<IEvent> eventEnvelope)
        {
            foreach (var eventSourcedBuilder in _eventSourcedBuilders)
            {
                eventSourcedBuilder.Apply(eventEnvelope.Message);
            }

            EventApplied?.Invoke(this, new EventAppliedEventArgs(eventEnvelope));
        }

        public void Stop()
        {
            _subscription?.Dispose();
            _wasDisposed = true;
        }
    }
}
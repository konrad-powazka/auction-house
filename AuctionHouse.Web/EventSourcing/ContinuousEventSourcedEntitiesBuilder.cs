using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Persistence;

namespace AuctionHouse.Web.EventSourcing
{
    public class ContinuousEventSourcedEntitiesBuilder : IContinuousEventSourcedEntitiesBuilder
    {
        private readonly IEventsDatabase _eventsDatabase;
        private readonly IEnumerable<IEventSourcedEntity> _eventSourcedBuilders;
        private IDisposable _subscription;
        private bool _wasDisposed;

        private readonly ConcurrentDictionary<Guid, object> _idsOfEventsAppliedToReadModel =
            new ConcurrentDictionary<Guid, object>();

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
            // In real life this would be handled by a decorator
            if (Configuration.EventsApplicationToReadModelDelayInMilliseconds.HasValue)
            {
                Thread.Sleep(Configuration.EventsApplicationToReadModelDelayInMilliseconds.Value);
            }

            foreach (var eventSourcedBuilder in _eventSourcedBuilders)
            {
                eventSourcedBuilder.Apply(eventEnvelope.Message);
            }

            if (!_idsOfEventsAppliedToReadModel.TryAdd(eventEnvelope.MessageId, null))
            {
                throw new ArgumentException(nameof(eventEnvelope));
            }

            EventApplied?.Invoke(this, new EventAppliedEventArgs(eventEnvelope));
        }

        public void Stop()
        {
            _wasDisposed = true;
            _subscription?.Dispose();
        }

        public bool CheckIfEventWasApplied(Guid eventId)
        {
            return _idsOfEventsAppliedToReadModel.ContainsKey(eventId);
        }
    }
}
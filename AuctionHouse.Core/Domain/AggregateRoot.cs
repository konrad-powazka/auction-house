using System;
using System.Collections.Generic;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.Domain
{
    public abstract class AggregateRoot : Entity
    {     
        private readonly Dictionary<Type, Action<IEvent>> _eventAppliers = new Dictionary<Type, Action<IEvent>>();
        private bool _wereEventsReplayed, _wereAppliersRegistered;
        private readonly List<IEvent> _changes = new List<IEvent>();

        public void ReplayEvents(IEnumerable<IEvent> eventsToReplay)
        {
            if (_wereEventsReplayed)
            {
                throw new InvalidOperationException("Events can be replayed only once.");
            }

            _wereEventsReplayed = true;

            if (eventsToReplay == null)
            {
                throw new ArgumentNullException(nameof(eventsToReplay));
            }

            foreach (var eventToReplay in eventsToReplay)
            {
                Apply(eventToReplay);
            }
        }

        public IReadOnlyCollection<IEvent> Changes => _changes.AsReadOnly();

        private void TryRegisterEventAppliers()
        {
            if (!_wereAppliersRegistered)
            {
                _wereAppliersRegistered = true;
                RegisterEventAppliers();
            }
        }

        protected abstract void RegisterEventAppliers();

        protected void RegisterEventApplier<TEvent>(Action<TEvent> eventApplier) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);

            if (_eventAppliers.ContainsKey(eventType))
            {
                throw new ArgumentException($"An applier for event type '{eventType}' has already been registered.");
            }

            _eventAppliers[eventType] = eventToApply => eventApplier((TEvent) eventToApply);
        }

        protected void ApplyChange(IEvent eventToApply)
        {
            Apply(eventToApply);
            _changes.Add(eventToApply);
        }

        private void Apply(IEvent eventToApply)
        {
            if (eventToApply == null)
            {
                throw new ArgumentNullException(nameof(eventToApply));
            }

			TryRegisterEventAppliers();
			Action<IEvent> applyEventAction;
            var eventType = eventToApply.GetType();

            if (!_eventAppliers.TryGetValue(eventType, out applyEventAction))
            {
                throw new ArgumentException($"No applier has been registered for events of type '{eventType}'");
            }

            applyEventAction(eventToApply);
        }
    }
}
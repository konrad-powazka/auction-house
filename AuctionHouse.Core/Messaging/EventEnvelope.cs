using System;

namespace AuctionHouse.Core.Messaging
{
	public class EventEnvelope<TEvent> : IEventEnvelope<TEvent> where TEvent : IEvent
	{
		public EventEnvelope(TEvent @event, Guid eventId)
		{
			if (@event == null)
			{
				throw new ArgumentNullException(nameof(@event));
			}

			Event = @event;
			EventId = eventId;
		}

		public TEvent Event { get; }
		public Guid EventId { get; }
	}
}
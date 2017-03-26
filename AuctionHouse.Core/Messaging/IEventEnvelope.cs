using System;

namespace AuctionHouse.Core.Messaging
{
	public interface IEventEnvelope<out TEvent> where TEvent : IEvent
	{
		TEvent Event { get; }
		Guid EventId { get; }
	}
}
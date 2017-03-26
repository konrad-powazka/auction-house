using System.Collections.Generic;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
	public interface ITrackingEventsDatabase : IEventsDatabase
	{
		IReadOnlyCollection<IEventEnvelope<IEvent>> WrittenEventEnvelopes { get; }
	}
}
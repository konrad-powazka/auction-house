using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
	public class PersistedEventEnvelope : EventEnvelope<IEvent>
	{
		public string StreamName { get; }
		public int StreamVersion { get; }

		public PersistedEventEnvelope(IEvent @event, Guid eventId, string streamName, int streamVersion)
			: base(@event, eventId)
		{
			StreamName = streamName;
			StreamVersion = streamVersion;
		}
	}
}
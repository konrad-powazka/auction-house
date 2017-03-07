using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
	public class PersistedEventEnvelope : MessageEnvelope<IEvent>
	{
		public string StreamName { get; }
		public int StreamVersion { get; }

		public PersistedEventEnvelope(IEvent message, Guid messageId, string streamName, int streamVersion)
			: base(message, messageId)
		{
			StreamName = streamName;
			StreamVersion = streamVersion;
		}
	}
}
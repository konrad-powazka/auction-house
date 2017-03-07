using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
	public interface IEventsDatabase
	{
		Task AppendToStream(string streamName, IEnumerable<MessageEnvelope<IEvent>> eventEnvelopesToAppend,
			ExpectedStreamVersion expectedStreamVersion, int? specificExpectedStreamVersion = null);

		Task<IEnumerable<PersistedEventEnvelope>> ReadStream(string streamName);

		Task<IDisposable> ReadAllExistingEventsAndSubscribe(Action<PersistedEventEnvelope> handleEventEnvelopeCallback);
	}
}
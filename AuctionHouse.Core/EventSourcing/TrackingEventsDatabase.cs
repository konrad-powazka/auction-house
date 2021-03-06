﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
	public class TrackingEventsDatabase : ITrackingEventsDatabase
	{
		private readonly IEventsDatabase _eventsDatabase;
		private readonly List<IEventEnvelope<IEvent>> _writtenEventEnvelopes = new List<IEventEnvelope<IEvent>>();

		public TrackingEventsDatabase(IEventsDatabase eventsDatabase)
		{
			if (eventsDatabase == null)
			{
				throw new ArgumentNullException(nameof(eventsDatabase));
			}

			_eventsDatabase = eventsDatabase;
		}

		public IReadOnlyCollection<IEventEnvelope<IEvent>> WrittenEventEnvelopes => _writtenEventEnvelopes.ToList();

		public Task<IEnumerable<PersistedEventEnvelope>> ReadStream(string streamName)
		{
			return _eventsDatabase.ReadStream(streamName);
		}

		public async Task AppendToStream(string streamName, IEnumerable<IEventEnvelope<IEvent>> eventEnvelopesToAppend,
			ExpectedStreamVersion expectedStreamVersion, int? specificExpectedStreamVersion)
		{
			eventEnvelopesToAppend = eventEnvelopesToAppend.ToList();
			await
				_eventsDatabase.AppendToStream(streamName, eventEnvelopesToAppend, expectedStreamVersion,
					specificExpectedStreamVersion);
			_writtenEventEnvelopes.AddRange(eventEnvelopesToAppend);
		}

		public Task<IDisposable> ReadAllExistingEventsAndSubscribe(
			Action<PersistedEventEnvelope> handleEventEnvelopeCallback)
		{
			return _eventsDatabase.ReadAllExistingEventsAndSubscribe(handleEventEnvelopeCallback);
		}
	}
}
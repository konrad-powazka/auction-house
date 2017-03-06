using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain;

namespace AuctionHouse.Persistence
{
	public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
		where TAggregateRoot : AggregateRoot
	{
		private static readonly Guid NamespaceGuid = new Guid("d5b5da81-7ee8-4716-9e33-2de549f9ebc2");
		private readonly IEventsDatabase _eventsDatabase;

		protected Repository(IEventsDatabase eventsDatabase)
		{
			_eventsDatabase = eventsDatabase;
		}

		public async Task Save(TAggregateRoot aggregateRoot, string changeId,
			ExpectedAggregateRootVersion expectedAggregateRootVersion, int? specificExpectedAggregateRootVersion)
		{
			var expectedStreamVersion = GetExpectedStreamversion(expectedAggregateRootVersion);

			if (expectedAggregateRootVersion == ExpectedAggregateRootVersion.Specific &&
			    !specificExpectedAggregateRootVersion.HasValue)
			{
				throw new ArgumentNullException(nameof(specificExpectedAggregateRootVersion));
			}

			var streamName = GetAggregateRootStreamName(aggregateRoot.Id);
			var eventEnvelopes = WrapAggregateRootChangesIntoEnvelopes(aggregateRoot, changeId).ToList();

			await
				_eventsDatabase.AppendToStream(streamName, eventEnvelopes, expectedStreamVersion,
					specificExpectedAggregateRootVersion);
		}

		public async Task Create(TAggregateRoot aggregateRoot, string changeId)
		{
			var streamName = GetAggregateRootStreamName(aggregateRoot.Id);
			var eventEnvelopes = WrapAggregateRootChangesIntoEnvelopes(aggregateRoot, changeId).ToList();

			await
				_eventsDatabase.AppendToStream(streamName, eventEnvelopes, ExpectedStreamVersion.NotExisting);
		}

		public async Task<TAggregateRoot> Get(Guid aggregateRootId)
		{
			var streamName = GetAggregateRootStreamName(aggregateRootId);
			var eventsToReplay = (await _eventsDatabase.ReadStream(streamName)).Select(e => e.Message).ToList();
			var aggregateRoot = CreateEmptyAggregateRootInstance();
			aggregateRoot.ReplayEvents(eventsToReplay);

			return aggregateRoot;
		}

		private static ExpectedStreamVersion GetExpectedStreamversion(
			ExpectedAggregateRootVersion expectedAggregateRootVersion)
		{
			switch (expectedAggregateRootVersion)
			{
				case ExpectedAggregateRootVersion.Specific:
					return ExpectedStreamVersion.SpecificExisting;
				case ExpectedAggregateRootVersion.Any:
					return ExpectedStreamVersion.AnyExisting;
				default:
					throw new ArgumentOutOfRangeException(nameof(expectedAggregateRootVersion));
			}
		}

		protected abstract TAggregateRoot CreateEmptyAggregateRootInstance();

		private static IEnumerable<MessageEnvelope<IEvent>> WrapAggregateRootChangesIntoEnvelopes(
			TAggregateRoot aggregateRoot, string changeId)
		{
			return aggregateRoot.Changes.Select((e, i) =>
			{
				var streamName = GetAggregateRootStreamName(aggregateRoot.Id);
				var eventGuidName = $"{streamName}-{changeId}-{i}";
				var deterministicEventId = GuidGenerator.GenerateDeterministicGuid(NamespaceGuid, eventGuidName);
				return new MessageEnvelope<IEvent>(e, deterministicEventId);
			});
		}

		private static string GetAggregateRootStreamName(Guid aggregateRootId)
		{
			return $"{typeof(TAggregateRoot).Name}-{aggregateRootId}";
		}
	}
}
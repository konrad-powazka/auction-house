using System;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core;
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

		public async Task Save(TAggregateRoot aggregateRoot, int previousAggregateRootVersion)
		{
			throw new NotImplementedException();
		}

		public async Task Create(TAggregateRoot aggregateRoot)
		{
			var eventEnvelopes = aggregateRoot.Changes.Select((e, i) =>
			{
				var eventGuidName = $"{GetAggregateRootStreamName(aggregateRoot)}-0-{i}";
				var deterministicEventId = GuidGenerator.CreateDeterministicGuid(NamespaceGuid, eventGuidName);
				return new MessageEnvelope<IEvent>(e, deterministicEventId);
			});

			await
				_eventsDatabase.AppendToStream(GetAggregateRootStreamName(aggregateRoot),
					null, eventEnvelopes);
		}

		public async Task<TAggregateRoot> Get(Guid aggregateRootId)
		{
			var eventsToReplay = new IEvent[0]; //TODO: Get from ES
			var aggregateRoot = CreateEmptyAggregateRootInstance();
			aggregateRoot.ReplayEvents(eventsToReplay);

			return aggregateRoot;
		}

		protected abstract TAggregateRoot CreateEmptyAggregateRootInstance();

		private string GetAggregateRootStreamName(TAggregateRoot aggregateRoot)
		{
			return $"{aggregateRoot.GetType().Name}-{aggregateRoot.Id}";
		}
	}
}
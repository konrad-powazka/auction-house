using System;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain;

namespace AuctionHouse.Persistence
{
    public class EventStoreRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IEventsDatabase _eventsDatabase;

        public EventStoreRepository(IEventsDatabase eventsDatabase)
        {
            _eventsDatabase = eventsDatabase;
        }

        public async Task Save(TAggregateRoot aggregateRoot, int previousAggregateRootVersion)
        {
            throw new NotImplementedException();
        }

        public async Task Create(TAggregateRoot aggregateRoot)
        {
            //TODO: Event GUIDs should be deterministic
            var eventEnvelopes = aggregateRoot.Changes.Select(e => new MessageEnvelope<IEvent>(e, Guid.NewGuid()));

            await
                _eventsDatabase.AppendToStream(GetAggregateRootStreamName(aggregateRoot),
                    null, eventEnvelopes);
        }

        public async Task<TAggregateRoot> Get(Guid aggregateRootId)
        {
            var eventsToReplay = new IEvent[0]; //TODO: Get from ES
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.ReplayEvents(eventsToReplay);

            return aggregateRoot;
        }

        private string GetAggregateRootStreamName(TAggregateRoot aggregateRoot)
        {
            return $"{aggregateRoot.GetType().Name}-{aggregateRoot.Id}";
        }
    }
}
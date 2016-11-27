using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Domain;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace AuctionHouse.Persistence
{
    public class EventStoreRepository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public EventStoreRepository(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task Save(TAggregateRoot aggregateRoot, int previousAggregateRootVersion)
        {
            throw new NotImplementedException();
        }

        public async Task Create(TAggregateRoot aggregateRoot)
        {
            // TODO: connect before injecting
            await _eventStoreConnection.ConnectAsync();

            var eventDataList =
                aggregateRoot.Changes.Select(e =>
                {
                    var eventId = Guid.NewGuid(); //TODO: Event GUIDs should be deterministic
                    var serializedEvent = SerializeEvent(e);
                    return new EventData(eventId, e.GetType().Name, true, serializedEvent, null);
                });

            await
                _eventStoreConnection.AppendToStreamAsync(GetAggregateRootStreamName(aggregateRoot),
                    ExpectedVersion.EmptyStream, eventDataList);
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

        private static byte[] SerializeEvent(IEvent eventToSerialize)
        {
            var textSerializedEvent = JsonConvert.SerializeObject(eventToSerialize);
            var binarySerializedEvent = Encoding.UTF8.GetBytes(textSerializedEvent);

            return binarySerializedEvent;
        }
    }
}
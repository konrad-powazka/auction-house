using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Messages.Queries.Auctions;
using AuctionHouse.ReadModel.Auctions.Details;
using Newtonsoft.Json;

namespace AuctionHouse.QueryHandling.Auctions
{
    // TODO: Extract base class
    // TODO: For fast prototyping automapping from events to RM could be introduced
    public class GetAuctionDetailsQueryHandler : IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>,
        IEventSourcedEntity
    {
        private readonly ConcurrentDictionary<Guid, CachedReadModel<AuctionDetailsReadModel>> _readModelsCache =
            new ConcurrentDictionary<Guid, CachedReadModel<AuctionDetailsReadModel>>();

        public void Apply(IEvent @event)
        {
            if (@event is AuctionCreatedEvent)
            {
                var auctionDetails = new AuctionDetailsReadModel();
                var auctionCreatedEvent = (AuctionCreatedEvent) @event;
                auctionDetails.Id = auctionCreatedEvent.Id;
                auctionDetails.Title = auctionCreatedEvent.Title;
                auctionDetails.Description = auctionCreatedEvent.Description;
                AddCachedReadModel(auctionCreatedEvent.Id, auctionDetails);
            }
        }

        public async Task<AuctionDetailsReadModel> Handle(GetAuctionDetailsQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            CachedReadModel<AuctionDetailsReadModel> cachedReadModel;

            return _readModelsCache.TryGetValue(query.Id, out cachedReadModel) ? cachedReadModel.GetReadModel() : null;
        }

        private void AddCachedReadModel(Guid id, AuctionDetailsReadModel readModel)
        {
            if (!_readModelsCache.TryAdd(id, new CachedReadModel<AuctionDetailsReadModel>(readModel)))
            {
                throw new InvalidOperationException();
            }
        }

        private class CachedReadModel<TReadModel> where TReadModel : class
        {
            // We do not lock on reads, because we expect eventual consistency
            private readonly object _writeLock = new object();
            private TReadModel _readModel;

            public CachedReadModel(TReadModel readModel)
            {
                _readModel = readModel;
            }

            public void Mutate(Action<TReadModel> mutator)
            {
                lock (_writeLock)
                {
                    var clonedReadModel = GetReadModel();
                    mutator(clonedReadModel);
                    Interlocked.Exchange(ref _readModel, clonedReadModel);
                }
            }

            public TReadModel GetReadModel()
            {
                return DeepClone(_readModel);
            }

            private static T DeepClone<T>(T source)
            {
                // This is not pretty, but does its job:
                var serialized = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<T>(serialized);
            }
        }
    }
}
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Events;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Messages.Queries.Auctions;
using AuctionHouse.ReadModel.Auctions.Details;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace AuctionHouse.QueryHandling.Auctions
{
    // TODO: Extract base class
    public class GetAuctionDetailsQueryHandler : IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public GetAuctionDetailsQueryHandler(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public async Task<AuctionDetailsReadModel> Handle(GetAuctionDetailsQuery query)
        {
            await _eventStoreConnection.ConnectAsync(); // TODO: connect before injecting
            var streamId = $"Auction-{query.Id}"; //TODO: To common code
            var auctionDetails = new AuctionDetailsReadModel();
            await ReadEventStream(streamId, @event =>
            {
                if (@event is AuctionCreatedEvent)
                {
                    var auctionCreatedEvent = (AuctionCreatedEvent) @event;
                    auctionDetails.Id = auctionCreatedEvent.Id;
                    auctionDetails.Title = auctionCreatedEvent.Title;
                    auctionDetails.Description = auctionCreatedEvent.Description;
                }
            });

            return auctionDetails;
        }

        private async Task ReadEventStream(string streamName, Action<IEvent> handleEvent)
        {
            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice =
                    await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, nextSliceStart,
                        200, false);

                nextSliceStart = currentSlice.NextEventNumber;

                foreach (var eventStoreEvent in currentSlice.Events)
                {
                    var textSerializedEvent = Encoding.UTF8.GetString(eventStoreEvent.Event.Data);

                    //TODO: Create a cached dictionary
                    var eventType =
                        typeof(EventsAssemblyMarker).Assembly.GetTypes()
                            .Single(t => t.Name == eventStoreEvent.Event.EventType);

                    var @event = (IEvent) JsonConvert.DeserializeObject(textSerializedEvent, eventType);
                    handleEvent(@event);
                }
            } while (!currentSlice.IsEndOfStream);
        }
    }
}
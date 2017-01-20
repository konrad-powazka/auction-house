using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
    public interface IEventSourcedBuilder
    {
        void Apply(IEvent @event, bool isLiveEvent);
    }
}
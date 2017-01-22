using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.EventSourcing
{
    public interface IEventSourcedEntity
    {
        void Apply(IEvent @event);
    }
}
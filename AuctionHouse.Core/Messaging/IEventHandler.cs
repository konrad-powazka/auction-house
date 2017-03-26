using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(IEventEnvelope<TEvent> eventEnvelope);
    }
}
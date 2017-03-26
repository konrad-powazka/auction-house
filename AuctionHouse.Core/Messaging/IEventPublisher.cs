using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
	public interface IEventPublisher
	{
		Task Publish(IEventEnvelope<IEvent> eventEnvelope);
	}
}
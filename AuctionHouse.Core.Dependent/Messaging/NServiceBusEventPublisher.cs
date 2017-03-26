using System.Threading.Tasks;
using NServiceBus;

namespace AuctionHouse.Core.Messaging
{
	public class NServiceBusEventPublisher : IEventPublisher
	{
		private readonly IEndpointInstance _endpoint;

		public NServiceBusEventPublisher(IEndpointInstance endpoint)
		{
			_endpoint = endpoint;
		}

		public async Task Publish(IEventEnvelope<IEvent> eventEnvelope)
		{
			var publishOptions = new PublishOptions();
			publishOptions.SetMessageId(eventEnvelope.EventId.ToString());
			await _endpoint.Publish(eventEnvelope.Event, publishOptions);
		}
	}
}
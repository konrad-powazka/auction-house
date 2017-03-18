using AuctionHouse.Core.Messaging;

namespace AuctionHouse.ServiceBus
{
	public class NsbCommandQueueConfiguration : INServiceBusCommandQueueConfiguration
	{
		public string NServiceBusCommandHandlingDestination => Constants.EndpointName;
	}
}
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.CommandQueueService
{
	public class NsbCommandQueueConfiguration : INServiceBusCommandQueueConfiguration
	{
		public string NServiceBusCommandHandlingDestination => Constants.EndpointName;
	}
}
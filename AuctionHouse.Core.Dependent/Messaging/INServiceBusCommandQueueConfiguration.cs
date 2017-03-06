namespace AuctionHouse.Core.Messaging
{
	public interface INServiceBusCommandQueueConfiguration
	{
		string NServiceBusCommandHandlingDestination { get; }
	}
}
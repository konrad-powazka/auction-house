using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.ServiceBus
{
	public class NsbCommandQueueConfiguration : INServiceBusCommandQueueConfiguration
	{
		public string NServiceBusCommandHandlingDestination => Constants.EndpointName;
	}
}

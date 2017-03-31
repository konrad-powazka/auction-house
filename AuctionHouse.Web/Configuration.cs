using System;
using System.Configuration;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Web
{
    public class Configuration : INServiceBusCommandQueueConfiguration
	{
        public string NServiceBusCommandHandlingDestination
            => ConfigurationManager.AppSettings["NServiceBusCommandHandlingDestination"];
	}
}
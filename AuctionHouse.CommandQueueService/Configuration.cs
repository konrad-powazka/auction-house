using System;
using System.Configuration;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.CommandQueueService
{
	public class Configuration : INServiceBusCommandQueueConfiguration
	{
		public string NServiceBusCommandHandlingDestination => Constants.EndpointName;

		public TimeSpan? DelayBeforeHandlingCommand
		{
			get
			{
				var delayBeforeHandlingCommandMillisecondsString = ConfigurationManager.AppSettings["DelayBeforeHandlingCommandMilliseconds"];

				int delayBeforeHandlingCommandMilliseconds;

				if (!int.TryParse(delayBeforeHandlingCommandMillisecondsString, out delayBeforeHandlingCommandMilliseconds))
				{
					return null;
				}

				return TimeSpan.FromMilliseconds(delayBeforeHandlingCommandMilliseconds);
			}
		}
	}
}
using System;
using System.Configuration;

namespace AuctionHouse.Web
{
    // TODO: This should be mockable
    public class Configuration
    {
        public static string NServiceBusCommandHandlingDestination
            => ConfigurationManager.AppSettings["NServiceBusCommandHandlingDestination"];

		public static bool PopulatingDatabaseWithTestDataOnStartupIsEnabled
		{
			get
			{
				{
					var rawValue = ConfigurationManager.AppSettings["PopulatingDatabaseWithTestDataOnStartupIsEnabled"];
					bool parsedValue;
					return bool.TryParse(rawValue, out parsedValue) && parsedValue;
				}
			}
		}
	}
}
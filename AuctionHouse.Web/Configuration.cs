using System.Configuration;

namespace AuctionHouse.Web
{
    // TODO: This should be mockable
    public class Configuration
    {
        public static string NServiceBusCommandHandlingDestination
            => ConfigurationManager.AppSettings["NServiceBusCommandHandlingDestination"];

        public static int? EventsApplicationToReadModelDelayInMilliseconds
        {
            get
            {
                {
                    var rawValue = ConfigurationManager.AppSettings["EventsApplicationToReadModelDelayInMilliseconds"];
                    if (string.IsNullOrEmpty(rawValue))
                    {
                        return null;
                    }

                    return int.Parse(rawValue);
                }
            }
        }
    }
}
using System.Configuration;

namespace AuctionHouse.Web
{
    // TODO: This should be mockable
    public class Configuration
    {
        public static string NServiceBusCommandHandlingDestination
            => ConfigurationManager.AppSettings["NServiceBusCommandHandlingDestination"];
    }
}
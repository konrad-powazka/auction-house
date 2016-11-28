using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AuctionHouse.Web
{
    // TODO: This should be mockable
    public class Configuration
    {
        public static string NServiceBusCommandHandlingDestination
            => ConfigurationManager.AppSettings["NServiceBusCommandHandlingDestination"];
    }
}
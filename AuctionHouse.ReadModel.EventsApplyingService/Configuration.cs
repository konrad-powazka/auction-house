using System.Configuration;

namespace AuctionHouse.ReadModel.EventsApplyingService
{
	// TODO: This should be mockable
	public class Configuration
	{
		public static int? EventsApplicationToReadModelDelayMilliseconds
		{
			get
			{
				{
					var rawValue = ConfigurationManager.AppSettings["EventsApplicationToReadModelDelayMilliseconds"];
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
using System.Configuration;

namespace AuctionHouse.ReadModel.EventsApplyingService
{
	// TODO: This should be mockable
	public class Configuration
	{
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
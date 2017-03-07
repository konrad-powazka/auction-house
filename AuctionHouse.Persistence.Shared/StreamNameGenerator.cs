using System;
using System.Linq;

namespace AuctionHouse.Persistence.Shared
{
	public static class StreamNameGenerator
	{
		public static string GenerateAuctionStreamName(Guid auctionId)
		{
			return $"Auction-{auctionId}";
		}

		public static bool TryExtractAuctionId(string streamName, out Guid auctionId)
		{
			auctionId = default(Guid);

			if (streamName == null)
			{
				return false;
			}

			var firstDashIndex = streamName.IndexOf("-", StringComparison.Ordinal);

			if (firstDashIndex < 0 || streamName.Substring(0, firstDashIndex) != "Auction")
			{
				return false;
			}

			var guidText = streamName.Substring(firstDashIndex + 1);
			return Guid.TryParse(guidText, out auctionId);
		}
	}
}
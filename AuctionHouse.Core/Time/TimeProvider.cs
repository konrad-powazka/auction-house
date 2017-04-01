using System;

namespace AuctionHouse.Core.Time
{
	public class TimeProvider : ITimeProvider
	{
		public DateTime UtcNow => DateTime.UtcNow;
	}
}
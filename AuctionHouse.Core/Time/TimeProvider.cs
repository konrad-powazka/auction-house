using System;

namespace AuctionHouse.Core.Time
{
	public class TimeProvider : ITimeProvider
	{
		public DateTime Now => DateTime.Now;
	}
}
using System;

namespace AuctionHouse.Core.Time
{
	public interface ITimeProvider
	{
		DateTime UtcNow { get; }
	}
}
using System;

namespace AuctionHouse.Core.Time
{
	public interface ITimeProvider
	{
		DateTime Now { get; }
	}
}
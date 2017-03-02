using System;
using System.Collections.Generic;

namespace AuctionHouse.Web.EventSourcing
{
	public interface IEventsAppliedToReadModelTracker
	{
		event EventHandler<EventAppliedEventArgs> EventApplied;

		bool CheckIfEventWasApplied(Guid eventId);
		void MarkEventsAsApplied(IEnumerable<Guid> appliedEventIds);
	}
}
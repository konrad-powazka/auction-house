using System;

namespace AuctionHouse.Web.EventSourcing
{
	public class EventAppliedEventArgs : EventArgs
	{
		public EventAppliedEventArgs(Guid appliedEventId)
		{
			AppliedEventId = appliedEventId;
		}

		public Guid AppliedEventId { get; }
	}
}
using System;
using System.Collections.Generic;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.Technical
{
	public class EventsAppliedToReadModelEvent : IEvent
	{
		public IReadOnlyCollection<Guid> AppliedEventIds { get; set; }
	}
}
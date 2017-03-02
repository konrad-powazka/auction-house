using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AuctionHouse.Web.EventSourcing
{
	public class EventsAppliedToReadModelTracker : IEventsAppliedToReadModelTracker
	{
		private readonly ConcurrentDictionary<Guid, object> _idsOfEventsAppliedToReadModel =
			new ConcurrentDictionary<Guid, object>();

		public event EventHandler<EventAppliedEventArgs> EventApplied;

		public void MarkEventsAsApplied(IEnumerable<Guid> appliedEventIds)
		{
			if (appliedEventIds == null)
			{
				throw new ArgumentNullException(nameof(appliedEventIds));
			}

			foreach (var appliedEventId in appliedEventIds)
			{
				if (_idsOfEventsAppliedToReadModel.TryAdd(appliedEventId, null))
				{
					EventApplied?.Invoke(this, new EventAppliedEventArgs(appliedEventId));
				}
			}
		}

		public bool CheckIfEventWasApplied(Guid eventId)
		{
			return _idsOfEventsAppliedToReadModel.ContainsKey(eventId);
		}
	}
}
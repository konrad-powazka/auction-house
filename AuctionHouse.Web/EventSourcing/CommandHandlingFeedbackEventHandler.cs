using System.Threading.Tasks;
using AuctionHouse.Messages.Events.Technical;
using NServiceBus;

namespace AuctionHouse.Web.EventSourcing
{
	public class EventsAppliedToReadModelEventHandler :
		IHandleMessages<EventsAppliedToReadModelEvent>
	{
		private readonly IEventsAppliedToReadModelTracker _eventsAppliedToReadModelTracker;

		public EventsAppliedToReadModelEventHandler(IEventsAppliedToReadModelTracker eventsAppliedToReadModelTracker)
		{
			_eventsAppliedToReadModelTracker = eventsAppliedToReadModelTracker;
		}

		public async Task Handle(EventsAppliedToReadModelEvent message, IMessageHandlerContext context)
		{
			_eventsAppliedToReadModelTracker.MarkEventsAsApplied(message.AppliedEventIds);
		}
	}
}
using System;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Domain.UserMessaging;

namespace AuctionHouse.Persistence
{
	public class SentUserMessagesRepository : Repository<SentUserMessage>
	{
		public SentUserMessagesRepository(IEventsDatabase eventsDatabase) : base(eventsDatabase)
		{
		}

		protected override SentUserMessage CreateEmptyAggregateRootInstance()
		{
			return new SentUserMessage();
		}

		protected override string GetAggregateRootStreamName(Guid aggregateRootId)
		{
			return $"SentUserMessage-{aggregateRootId}";
		}
	}
}
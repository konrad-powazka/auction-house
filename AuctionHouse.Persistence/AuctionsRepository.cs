using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.Auctions;

namespace AuctionHouse.Persistence
{
	public class AuctionsRepository : Repository<Auction>
	{
		private readonly ITimeProvider _timeProvider;

		public AuctionsRepository(IEventsDatabase eventsDatabase, ITimeProvider timeProvider) : base(eventsDatabase)
		{
			_timeProvider = timeProvider;
		}

		protected override Auction CreateEmptyAggregateRootInstance()
		{
			return new Auction(_timeProvider);
		}
	}
}
using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Domain;

namespace AuctionHouse.Persistence
{
	public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
	{
		Task Create(TAggregateRoot aggregateRoot, string changeId);
		Task<TAggregateRoot> Get(Guid aggregateRootId);

		Task Save(TAggregateRoot aggregateRoot, string changeId, ExpectedAggregateRootVersion expectedAggregateRootVersion,
			int? specificExpectedAggregateRootVersion = null);
	}
}
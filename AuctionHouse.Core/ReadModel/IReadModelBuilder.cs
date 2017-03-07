using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;

namespace AuctionHouse.Core.ReadModel
{
	public interface IReadModelBuilder
	{
		Task Apply(PersistedEventEnvelope @event, IReadModelDbContext readModelDbContext);
	}
}
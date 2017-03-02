using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Core.ReadModel
{
	public interface IReadModelBuilder
	{
		Task Apply(IEvent @event, IReadModelDbContext readModelDbContext);
	}
}
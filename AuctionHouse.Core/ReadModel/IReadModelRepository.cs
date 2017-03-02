using System;
using System.Threading.Tasks;

namespace AuctionHouse.Core.ReadModel
{
	public interface IReadModelRepository
	{
		Task<TReadModel> Get<TReadModel>(Guid id) where TReadModel : class;
		Task Create<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class;
		Task Update<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class;
	}
}
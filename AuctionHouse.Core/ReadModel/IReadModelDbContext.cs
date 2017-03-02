using System;
using System.Threading.Tasks;

namespace AuctionHouse.Core.ReadModel
{
	public interface IReadModelDbContext
	{
		Task<TReadModel> Get<TReadModel>(Guid id) where TReadModel : class;
		void CreateOrOverwrite<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class;
		Task SaveChanges();
	}
}
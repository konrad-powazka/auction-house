using System.Threading.Tasks;

namespace AuctionHouse.Core.ReadModel
{
	public interface IReadModelDbContext
	{
		Task<TReadModel> TryGet<TReadModel>(string id) where TReadModel : class;
		void CreateOrOverwrite<TReadModel>(TReadModel readModel, string id) where TReadModel : class;
		Task SaveChanges();
	}
}
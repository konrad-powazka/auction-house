using System;
using System.Threading.Tasks;
using AuctionHouse.Core.ReadModel;
using Elasticsearch.Net;
using Nest;

namespace AuctionHouse.ReadModel.Repositories
{
	public class ElasticsearchReadModelRepository : IReadModelRepository
	{
		private readonly IElasticClient _elasticClient;

		public ElasticsearchReadModelRepository(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		public async Task<TReadModel> Get<TReadModel>(Guid id) where TReadModel : class
		{
			return (await _elasticClient.GetAsync<TReadModel>(id)).Source;
		}

		public async Task Create<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class
		{
			await Index(readModel, id);
		}

		public async Task Update<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class
		{
			await Index(readModel, id);
		}

		private async Task Index<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class
		{
			await _elasticClient.IndexAsync(readModel, r => r.Id(id).Refresh(Refresh.WaitFor));
		}
	}
}
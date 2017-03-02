using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Queries.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions.Details;
using Nest;

namespace AuctionHouse.QueryHandling
{
	public class ElasticsearchAuctionQueriesHandler : IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>
	{
		private readonly IElasticClient _elasticClient;

		public ElasticsearchAuctionQueriesHandler(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		public async Task<AuctionDetailsReadModel> Handle(GetAuctionDetailsQuery query)
		{
			var response = await _elasticClient.GetAsync<AuctionDetailsReadModel>(query.Id);
			return response.Source;
		}
	}
}
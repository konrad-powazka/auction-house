using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Queries.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions.Details;
using AuctionHouse.ReadModel.Dtos.Auctions.List;
using Nest;

namespace AuctionHouse.QueryHandling
{
	public class ElasticsearchAuctionQueriesHandler :
		IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>,
		IQueryHandler<SearchAuctionsQuery, AuctionsListReadModel>
	{
		private readonly IElasticClient _elasticClient;

		public ElasticsearchAuctionQueriesHandler(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		public async Task<AuctionDetailsReadModel> Handle(
			IQueryEnvelope<GetAuctionDetailsQuery, AuctionDetailsReadModel> queryEnvelope)
		{
			var response = await _elasticClient.GetAsync<AuctionDetailsReadModel>(queryEnvelope.Query.Id);
			return response.Source;
		}

		public async Task<AuctionsListReadModel> Handle(
			IQueryEnvelope<SearchAuctionsQuery, AuctionsListReadModel> queryEnvelope)
		{
			return
				await
					_elasticClient
						.RunPagedQuery<SearchAuctionsQuery, AuctionsListReadModel, AuctionListItemReadModel, AuctionDetailsReadModel>(
							queryEnvelope.Query,
							q => q.MultiMatch(
								mq =>
									mq.Fields(f => f.Fields(a => a.Title, a => a.Description))
										.Query(queryEnvelope.Query.QueryString)
										.Fuzziness(Fuzziness.Auto)),
							sd => sd.Descending(a => a.EndDate));
		}
	}
}
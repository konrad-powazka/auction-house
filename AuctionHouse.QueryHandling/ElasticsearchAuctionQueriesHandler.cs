using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Queries.Auctions;
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

		public async Task<AuctionDetailsReadModel> Handle(GetAuctionDetailsQuery query)
		{
			var response = await _elasticClient.GetAsync<AuctionDetailsReadModel>(query.Id);
			return response.Source;
		}

		public async Task<AuctionsListReadModel> Handle(SearchAuctionsQuery query)
		{
			var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
			var firstAuctionIndex = (pageNumber - 1)*query.PageSize;

			var elasticResult =
				await
					_elasticClient.SearchAsync<AuctionDetailsReadModel>(
						s =>
							s.Query(
								q =>
									q.MultiMatch(
										mq =>
											mq.Fields(f => f.Fields(a => a.Title, a => a.Description))
												.Query(query.QueryString)
												.Fuzziness(Fuzziness.Auto)))
								.From(firstAuctionIndex)
								.Take(query.PageSize));

			return new AuctionsListReadModel
			{
				PageNumber = pageNumber,
				PageItems = elasticResult.Documents,
				TotalItemsCount = elasticResult.Total,
				TotalPagesCount = (elasticResult.Total + query.PageSize - 1)/query.PageSize,
				PageSize = query.PageSize
			};
		}
	}
}
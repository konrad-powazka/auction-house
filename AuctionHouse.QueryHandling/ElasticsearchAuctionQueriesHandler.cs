using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Queries.Auctions;
using AuctionHouse.ReadModel.Dtos.Auctions;
using Nest;

namespace AuctionHouse.QueryHandling
{
	public class ElasticsearchAuctionQueriesHandler :
		IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>,
		IQueryHandler<SearchAuctionsQuery, AuctionsListReadModel>,
		IQueryHandler<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>
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
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return
				await
					_elasticClient
						.RunPagedQuery
						<GetAuctionsInvolvingUserQuery, AuctionsListReadModel, AuctionListItemReadModel, AuctionDetailsReadModel>(
							queryEnvelope.Query,
							q => q.Term(tqd => tqd.Field(a => a.BiddersUserNames).Value(queryEnvelope.SenderUserName))
							     &&
							     q.MultiMatch(
								     mq => mq.Fields(f => f.Fields(a => a.Title, a => a.Description))
									     .Query(queryEnvelope.Query.QueryString)
									     .Fuzziness(Fuzziness.Auto)),
							sd => sd.Descending(a => a.EndDate));
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
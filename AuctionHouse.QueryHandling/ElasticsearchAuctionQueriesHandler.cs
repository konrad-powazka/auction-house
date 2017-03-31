using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
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
		private readonly ITimeProvider _timeProvider;

		public ElasticsearchAuctionQueriesHandler(IElasticClient elasticClient, ITimeProvider timeProvider)
		{
			_elasticClient = elasticClient;
			_timeProvider = timeProvider;
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
			var userInvolvementIntoAuctionToFilterFunctionsMap =
				new Dictionary<UserInvolvementIntoAuction, Func<QueryContainerDescriptor<AuctionDetailsReadModel>,
					IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>, QueryContainer>[]>
				{
					[UserInvolvementIntoAuction.Selling] = new[] {AsFilter(FilterCreatedByCurrentUser), AsFilter(FilterNotFinished)},

					[UserInvolvementIntoAuction.Sold] =
						new[] {AsFilter(FilterCreatedByCurrentUser), AsFilter(FilterFinished), AsFilter(FilterWithBidders)},

					[UserInvolvementIntoAuction.FailedToSell] =
						new[] {AsFilter(FilterCreatedByCurrentUser), AsFilter(FilterFinished), AsFilter(FilterWithoutBidders)},

					[UserInvolvementIntoAuction.Bidding] = new[] {AsFilter(FilterCurrentUserAmongBidders), AsFilter(FilterNotFinished)},
					[UserInvolvementIntoAuction.Bought] = new[] {AsFilter(FilterCurrentUserIsHighestBidder), AsFilter(FilterFinished)},

					[UserInvolvementIntoAuction.FailedToBuy] =
						new[]
						{AsFilter(FilterCurrentUserAmongBidders), AsFilter(FilterFinished), AsFilter(FilterCurrentUserIsNotHighestBidder)}
				};

			return
				await
					_elasticClient
						.RunPagedQuery
						<GetAuctionsInvolvingUserQuery, AuctionsListReadModel, AuctionListItemReadModel, AuctionDetailsReadModel>(
							queryEnvelope.Query,
							qcd =>
							{
								var currentFilter = qcd.MultiMatch(
									mq => mq.Fields(f => f.Fields(a => a.Title, a => a.Description))
										.Query(queryEnvelope.Query.QueryString)
										.Fuzziness(Fuzziness.Auto));

								var filterFunctions =
									userInvolvementIntoAuctionToFilterFunctionsMap[queryEnvelope.Query.UserInvolvementIntoAuction];

								return filterFunctions.Aggregate(currentFilter, (current, filterFunction) => current && filterFunction(qcd, queryEnvelope));
							},
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
										.Fuzziness(Fuzziness.Auto)) && q.Term(bqd => bqd.Field(a => a.WasFinished).Value(false)),
							sd => sd.Descending(a => a.EndDate));
		}

		private static Func<QueryContainerDescriptor<AuctionDetailsReadModel>,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>, QueryContainer> AsFilter(
			Func<QueryContainerDescriptor<AuctionDetailsReadModel>,
				IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>, QueryContainer> filterFunction)
		{
			return filterFunction;
		}

		private static QueryContainer FilterCreatedByCurrentUser(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return queryContainerDescriptor.Term(tqd => tqd.Field(a => a.CreatedByUserName).Value(queryEnvelope.SenderUserName));
		}

		private QueryContainer FilterNotFinished(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return
				queryContainerDescriptor.Term(bqd => bqd.Field(a => a.WasFinished).Value(false));
		}

		private QueryContainer FilterFinished(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return
				queryContainerDescriptor.Term(bqd => bqd.Field(a => a.WasFinished).Value(true));
		}

		private QueryContainer FilterCurrentUserAmongBidders(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return queryContainerDescriptor.Term(tqd => tqd.Field(a => a.BiddersUserNames).Value(queryEnvelope.SenderUserName));
		}

		private QueryContainer FilterCurrentUserIsHighestBidder(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return
				queryContainerDescriptor.Term(tqd => tqd.Field(a => a.HighestBidderUserName).Value(queryEnvelope.SenderUserName));
		}

		private QueryContainer FilterCurrentUserIsNotHighestBidder(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return queryContainerDescriptor.Bool(bqd => bqd.MustNot(qcd => FilterCurrentUserIsHighestBidder(qcd, queryEnvelope)));
		}

		private QueryContainer FilterWithoutBidders(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return queryContainerDescriptor.Bool(bqd => bqd.MustNot(qcd => FilterWithBidders(qcd, queryEnvelope)));
		}

		private QueryContainer FilterWithBidders(
			QueryContainerDescriptor<AuctionDetailsReadModel> queryContainerDescriptor,
			IQueryEnvelope<GetAuctionsInvolvingUserQuery, AuctionsListReadModel> queryEnvelope)
		{
			return queryContainerDescriptor.Exists(tqd => tqd.Field(a => a.HighestBidderUserName));
		}
	}
}
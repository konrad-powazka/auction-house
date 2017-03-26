using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Paging;
using AuctionHouse.Messages.Queries.Auctions;
using AuctionHouse.Messages.Queries.UserMessaging;
using AuctionHouse.ReadModel.Dtos.Auctions.Details;
using AuctionHouse.ReadModel.Dtos.Auctions.List;
using AuctionHouse.ReadModel.Dtos.UserMessaging;
using Nest;

namespace AuctionHouse.QueryHandling
{
	public class ElasticsearchUserMessagingQueriesHandler :
		IQueryHandler<GetUserInboxQuery, UserInboxReadModel>
	{
		private readonly IElasticClient _elasticClient;

		public ElasticsearchUserMessagingQueriesHandler(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}


		Task<UserInboxReadModel> IQueryHandler<GetUserInboxQuery, UserInboxReadModel>.Handle(
			IQueryEnvelope<GetUserInboxQuery, UserInboxReadModel> queryEnvelope)
		{
			throw new NotImplementedException();
		}	
	}
}
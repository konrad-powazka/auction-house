﻿using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Queries.UserMessaging;
using AuctionHouse.ReadModel.Dtos.UserMessaging;
using Nest;

namespace AuctionHouse.QueryHandling
{
	public class ElasticsearchUserMessagingQueriesHandler :
		IQueryHandler<GetUserInboxQuery, UserInboxReadModel>,
		IQueryHandler<GetSentUserMessagesQuery, UserSentMessagesReadModel>
	{
		private readonly IElasticClient _elasticClient;

		public ElasticsearchUserMessagingQueriesHandler(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		public async Task<UserInboxReadModel> Handle(IQueryEnvelope<GetUserInboxQuery, UserInboxReadModel> queryEnvelope)
		{
			return
				await
					_elasticClient
						.RunPagedQuery<GetUserInboxQuery, UserInboxReadModel, UserMessageReadModel, UserMessageReadModel>(
							queryEnvelope.Query,
							qc => qc.Term(mqd => mqd.Field(m => m.RecipientUserName).Value(queryEnvelope.SenderUserName)),
							sd => sd.Descending(m => m.SentDateTime));
		}

		public async Task<UserSentMessagesReadModel> Handle(IQueryEnvelope<GetSentUserMessagesQuery, UserSentMessagesReadModel> queryEnvelope)
		{
			return
				await
					_elasticClient
						.RunPagedQuery<GetSentUserMessagesQuery, UserSentMessagesReadModel, UserMessageReadModel, UserMessageReadModel>(
							queryEnvelope.Query,
							qc => qc.Term(mqd => mqd.Field(m => m.SenderUserName).Value(queryEnvelope.SenderUserName)),
							sd => sd.Descending(m => m.SentDateTime));
		}
	}
}
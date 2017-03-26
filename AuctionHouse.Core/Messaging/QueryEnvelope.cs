using System;

namespace AuctionHouse.Core.Messaging
{
	public class QueryEnvelope<TQuery, TQueryResult> : IQueryEnvelope<TQuery, TQueryResult> where TQuery : IQuery<TQueryResult>
	{
		public QueryEnvelope(TQuery query, string senderUserName)
		{
			if (string.IsNullOrWhiteSpace(senderUserName))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(senderUserName));
			}

			SenderUserName = senderUserName;
			Query = query;
		}

		public TQuery Query { get; }

		public string SenderUserName { get; }
	}
}
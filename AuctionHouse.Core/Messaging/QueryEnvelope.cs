namespace AuctionHouse.Core.Messaging
{
	public class QueryEnvelope<TQuery, TQueryResult> : IQueryEnvelope<TQuery, TQueryResult> where TQuery : IQuery<TQueryResult>
	{
		public QueryEnvelope(TQuery query, string senderUserName = null)
		{
			SenderUserName = senderUserName;
			Query = query;
		}

		public TQuery Query { get; }

		public string SenderUserName { get; }
	}
}
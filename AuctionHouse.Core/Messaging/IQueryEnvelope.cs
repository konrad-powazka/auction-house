namespace AuctionHouse.Core.Messaging
{
	public interface IQueryEnvelope<out TQuery, out TQueryResult> where TQuery : IQuery<TQueryResult>
	{
		TQuery Query { get; }
		string SenderUserName { get; }
	}
}
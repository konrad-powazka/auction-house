namespace AuctionHouse.Application
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }

    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public string Handle(TestQuery query)
        {
            return "Test XXX";
        }
    }
}
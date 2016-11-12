namespace AuctionHouse.Application
{
    public interface IQuery<TResult>
    {
    }

    public class TestQuery : IQuery<string>
    {
    }
}
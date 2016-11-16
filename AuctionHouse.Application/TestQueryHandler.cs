using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages;

namespace AuctionHouse.Application
{
    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public string Handle(TestQuery query)
        {
            return "Test XXX";
        }
    }
}
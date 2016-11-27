using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
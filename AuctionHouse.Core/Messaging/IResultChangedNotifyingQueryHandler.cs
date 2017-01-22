using System;

namespace AuctionHouse.Core.Messaging
{
    public interface IEventAppliedNotifyingQueryHandler<in TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        IDisposable AddQueryResultChangedHandler(TQuery query, Action<TResult> resultChangedHandler);
    }
}
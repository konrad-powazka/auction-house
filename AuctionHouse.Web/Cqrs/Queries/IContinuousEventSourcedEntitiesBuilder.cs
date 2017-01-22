using System;

namespace AuctionHouse.Web.Cqrs.Queries
{
    public interface IContinuousEventSourcedEntitiesBuilder : IDisposable
    {
        event EventHandler<EventAppliedEventArgs> EventApplied;

        void Start();
        void Stop();
    }
}
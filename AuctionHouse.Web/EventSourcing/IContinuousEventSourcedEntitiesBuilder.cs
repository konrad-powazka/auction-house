using System;

namespace AuctionHouse.Web.EventSourcing
{
    public interface IContinuousEventSourcedEntitiesBuilder : IDisposable
    {
        event EventHandler<EventAppliedEventArgs> EventApplied;

        void Start();
        void Stop();
        bool CheckIfEventWasApplied(Guid eventId);
    }
}
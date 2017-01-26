using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Web.EventSourcing
{
    public class EventAppliedEventArgs : EventArgs
    {
        public EventAppliedEventArgs(MessageEnvelope<IEvent> appliedEventEnvelope)
        {
            AppliedEventEnvelope = appliedEventEnvelope;
        }

        public MessageEnvelope<IEvent> AppliedEventEnvelope { get; }
    }
}
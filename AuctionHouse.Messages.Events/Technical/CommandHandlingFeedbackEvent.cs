using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.Technical
{
    public abstract class CommandHandlingFeedbackEvent : IEvent
    {
        public Guid CommandId { get; set; }
    }
}
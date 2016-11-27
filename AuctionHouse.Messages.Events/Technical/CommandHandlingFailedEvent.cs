using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.Technical
{
    public class CommandHandlingFailedEvent : IEvent
    {
        public Guid CommandId { get; set; }
    }
}
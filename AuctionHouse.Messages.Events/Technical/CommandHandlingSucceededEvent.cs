using System;
using System.Collections.Generic;

namespace AuctionHouse.Messages.Events.Technical
{
    public class CommandHandlingSucceededEvent : CommandHandlingFeedbackEvent
    {
        public IReadOnlyCollection<Guid> PublishedEventIds { get; set; }
    }
}
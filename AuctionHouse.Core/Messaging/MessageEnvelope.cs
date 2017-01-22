using System;

namespace AuctionHouse.Core.Messaging
{
    public class MessageEnvelope<TMessage> where TMessage : IMessage
    {
        public Guid MessageId { get; set; }
        public TMessage Message { get; set; }
    }
}
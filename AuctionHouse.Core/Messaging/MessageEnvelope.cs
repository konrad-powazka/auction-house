﻿using System;

namespace AuctionHouse.Core.Messaging
{
    public class MessageEnvelope<TMessage> where TMessage : IMessage
    {
        public Guid MessageId { get; }
        public TMessage Message { get; }

        public MessageEnvelope(Guid messageId, TMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            MessageId = messageId;
            Message = message;
        }
    }
}
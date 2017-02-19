using System;

namespace AuctionHouse.Core.Messaging
{
    public class CommandEnvelope<TCommand> : MessageEnvelope<TCommand> where TCommand : ICommand
    {
        public CommandEnvelope(TCommand command, Guid commandId, string senderUserName) : base(command, commandId)
        {
            if (string.IsNullOrWhiteSpace(senderUserName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(senderUserName));
            }

            SenderUserName = senderUserName;
        }

        public string SenderUserName { get; }
    }
}
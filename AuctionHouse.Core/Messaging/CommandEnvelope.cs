using System;

namespace AuctionHouse.Core.Messaging
{
	public class CommandEnvelope<TCommand> : ICommandEnvelope<TCommand> where TCommand : ICommand
	{
		public CommandEnvelope(TCommand command, Guid commandId, string senderUserName)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}

			if (string.IsNullOrWhiteSpace(senderUserName))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(senderUserName));
			}

			Command = command;
			CommandId = commandId;
			SenderUserName = senderUserName;
		}

		public TCommand Command { get; }
		public Guid CommandId { get; }
		public string SenderUserName { get; }
	}
}
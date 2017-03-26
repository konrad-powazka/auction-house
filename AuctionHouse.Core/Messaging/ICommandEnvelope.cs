using System;

namespace AuctionHouse.Core.Messaging
{
	public interface ICommandEnvelope<out TCommand> where TCommand : ICommand
	{
		TCommand Command { get; }
		Guid CommandId { get; }
		string SenderUserName { get; }
	}
}
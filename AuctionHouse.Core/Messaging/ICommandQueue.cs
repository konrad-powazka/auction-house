using System;
using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
	public interface ICommandQueue
	{
		Task QueueCommand<TCommand>(TCommand command, Guid commandId, string senderUserName, DateTime? delayedUntil = null)
			where TCommand : ICommand;
	}
}
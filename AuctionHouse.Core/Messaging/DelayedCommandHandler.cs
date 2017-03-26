using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
	public class DelayedCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
	{
		private readonly ICommandHandler<TCommand> _decoratedCommandHandler;

		public DelayedCommandHandler(ICommandHandler<TCommand> decoratedCommandHandler)
		{
			if (decoratedCommandHandler == null)
			{
				throw new ArgumentNullException(nameof(decoratedCommandHandler));
			}

			_decoratedCommandHandler = decoratedCommandHandler;
		}

		public TimeSpan Delay { get; set; }

		public Task Handle(ICommandEnvelope<TCommand> commandEnvelope)
		{
			Thread.Sleep(Delay);
			return _decoratedCommandHandler.Handle(commandEnvelope);
		}
	}
}
using System.Threading.Tasks;
using AuctionHouse.Messages.Events.Technical;
using AuctionHouse.Web.Hubs;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace AuctionHouse.Web.Cqrs
{
	public class CommandHandlingFeedbackEventHandler :
		IHandleMessages<CommandHandlingFailedEvent>,
		IHandleMessages<CommandHandlingSucceededEvent>
	{
		private readonly IHubContext<ICommandHandlingFeedbackHubClient> _commandHandlingFeedbackHubContext;

		public CommandHandlingFeedbackEventHandler(
			IHubContext<ICommandHandlingFeedbackHubClient> commandHandlingFeedbackHubContext)
		{
			_commandHandlingFeedbackHubContext = commandHandlingFeedbackHubContext;
		}

		public async Task Handle(CommandHandlingFailedEvent message, IMessageHandlerContext context)
		{
			await
				_commandHandlingFeedbackHubContext.Clients.User(message.CommandSenderUserName).HandleCommandFailure(message);
		}

		public async Task Handle(CommandHandlingSucceededEvent message, IMessageHandlerContext context)
		{
			await
				_commandHandlingFeedbackHubContext.Clients.User(message.CommandSenderUserName).HandleCommandSuccess(message);
		}
	}
}
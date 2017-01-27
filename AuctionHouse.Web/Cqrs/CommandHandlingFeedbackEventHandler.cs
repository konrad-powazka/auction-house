using System.Threading.Tasks;
using AuctionHouse.Messages.Events.Technical;
using AuctionHouse.Web.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using NServiceBus;

namespace AuctionHouse.Web.Cqrs
{
    public class CommandHandlingFeedbackEventHandler : 
        IHandleMessages<CommandHandlingFailedEvent>,
        IHandleMessages<CommandHandlingSucceededEvent>
    {
        private readonly IHubContext<ICommandHandlingFeedbackHubClient> _commandHandlingFeedbackHubContext;

        public CommandHandlingFeedbackEventHandler(IHubContext<ICommandHandlingFeedbackHubClient> commandHandlingFeedbackHubContext)
        {
            _commandHandlingFeedbackHubContext = commandHandlingFeedbackHubContext;
        }

        public async Task Handle(CommandHandlingFailedEvent message, IMessageHandlerContext context)
        {
            //TODO: Pick single user using message header _hubContext.Clients.User(...)
            await _commandHandlingFeedbackHubContext.Clients.All.HandleCommandFailure(message);
        }

        public async Task Handle(CommandHandlingSucceededEvent message, IMessageHandlerContext context)
        {
            await _commandHandlingFeedbackHubContext.Clients.All.HandleCommandSuccess(message);
        }
    }
}
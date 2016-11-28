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
        private static IHubContext _hubContext =
            GlobalHost.ConnectionManager.GetHubContext<CommandHandlingFeedbackHub>();

        public async Task Handle(CommandHandlingFailedEvent message, IMessageHandlerContext context)
        {
            //TODO: Pick single user using message header _hubContext.Clients.User(...)
            await _hubContext.Clients.All.handleCommandFailure(message);
        }

        public async Task Handle(CommandHandlingSucceededEvent message, IMessageHandlerContext context)
        {
            await _hubContext.Clients.All.handleCommandSuccess(message);
        }
    }
}
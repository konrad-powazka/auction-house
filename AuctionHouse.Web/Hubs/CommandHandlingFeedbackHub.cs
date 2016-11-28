using Microsoft.AspNet.SignalR;

namespace AuctionHouse.Web.Hubs
{
    [Authorize]
    public class CommandHandlingFeedbackHub : Hub
    {
    }
}
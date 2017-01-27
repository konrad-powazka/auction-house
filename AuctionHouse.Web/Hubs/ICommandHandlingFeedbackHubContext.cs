using System.Threading.Tasks;
using AuctionHouse.Messages.Events.Technical;

namespace AuctionHouse.Web.Hubs
{
    public interface ICommandHandlingFeedbackHubClient
    {
        Task HandleCommandSuccess(CommandHandlingSucceededEvent commandHandlingSucceededEvent);
        Task HandleCommandFailure(CommandHandlingFailedEvent commandHandlingFailedEvent);
    }
}
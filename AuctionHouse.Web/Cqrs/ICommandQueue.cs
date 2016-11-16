using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Web.Cqrs
{
    public interface ICommandQueue
    {
        Task QueueCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
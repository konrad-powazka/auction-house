using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}
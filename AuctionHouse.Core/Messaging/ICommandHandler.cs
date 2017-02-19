using System.Threading.Tasks;

namespace AuctionHouse.Core.Messaging
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Handle(CommandEnvelope<TCommand> commandEnvelope);
    }
}
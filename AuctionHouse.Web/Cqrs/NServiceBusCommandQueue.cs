using System;
using System.Threading.Tasks;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;

namespace AuctionHouse.Web.Cqrs
{
    public class NServiceBusCommandQueue : ICommandQueue
    {
        private readonly IEndpointInstance _endpoint;

        public NServiceBusCommandQueue(IEndpointInstance endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            _endpoint = endpoint;
        }

        public async Task QueueCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _endpoint.Send("AuctionHouse.ServiceBus", command);
        }
    }
}
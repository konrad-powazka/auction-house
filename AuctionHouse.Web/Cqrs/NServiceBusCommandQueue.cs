using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
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

        public async Task QueueCommand<TCommand>(TCommand command, Guid commandId, string senderUserName)
            where TCommand : ICommand
        {
            var sendOptions = new SendOptions();
            sendOptions.SetMessageId(commandId.ToString());
            sendOptions.SetDestination(Configuration.NServiceBusCommandHandlingDestination);
            sendOptions.SetHeader(MessageHeaderNames.SenderUserName, senderUserName);
            await _endpoint.Send(command, sendOptions);
        }
    }
}
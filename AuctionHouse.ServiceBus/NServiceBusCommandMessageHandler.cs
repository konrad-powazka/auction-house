using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using Autofac;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;

namespace AuctionHouse.ServiceBus
{
    public class NServiceBusCommandMessageHandler : IHandleMessages<ICommand>
    {
        private readonly IComponentContext _componentContext;

        public NServiceBusCommandMessageHandler(IComponentContext componentContext)
        {
            if (componentContext == null)
            {
                throw new ArgumentNullException(nameof(componentContext));
            }

            _componentContext = componentContext;
        }

        public async Task Handle(ICommand command, IMessageHandlerContext context)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var senderUserName = context.MessageHeaders[MessageHeaderNames.SenderUserName];
            var commandId = Guid.Parse(context.MessageId);
            await Handle((dynamic) command, commandId, senderUserName);
        }

        private async Task Handle<TCommand>(TCommand command, Guid commandId, string senderUserName)
            where TCommand : ICommand
        {
            var commandHandler = _componentContext.Resolve<ICommandHandler<TCommand>>();
            var commandEnvelope = new CommandEnvelope<TCommand>(command, commandId, senderUserName);
            await commandHandler.Handle(commandEnvelope);
        }
    }
}
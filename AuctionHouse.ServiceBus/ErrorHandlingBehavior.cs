using System;
using System.Threading.Tasks;
using AuctionHouse.Messages.Events.Technical;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

namespace AuctionHouse.ServiceBus
{
    public class ErrorHandlingBehavior :
        Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            try
            {
                await next();
            }
            catch
            {
                var isCommandMessage = context.Message.GetMesssageIntent() == MessageIntentEnum.Send;

                if (isCommandMessage)
                {
                    var endpointInstance = context.Builder.Build<IEndpointInstance>();

                    var commandHandlingFailedEvent = new CommandHandlingFailedEvent
                    {
                        CommandId = Guid.Parse(context.Message.MessageId)
                    };

                    await endpointInstance.Publish(commandHandlingFailedEvent);
                }

                throw;
            }
        }
    }
}
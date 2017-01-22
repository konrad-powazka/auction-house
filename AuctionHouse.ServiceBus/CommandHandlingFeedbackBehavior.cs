using System;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Messages.Events.Technical;
using NServiceBus;
using NServiceBus.Pipeline;
using NServiceBus.Transport;

namespace AuctionHouse.ServiceBus
{
    public class CommandHandlingFeedbackBehavior :
        Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            try
            {
                await next();
                await TryPublishCommandHandlingFeedbackEvent(context, true);
            }
            catch
            {
                await TryPublishCommandHandlingFeedbackEvent(context, false);
                throw;
            }
        }

        private static async Task TryPublishCommandHandlingFeedbackEvent(ITransportReceiveContext context,
            bool wasMessageHandledSuccessfully)
        {
            var isCommandMessage = context.Message.GetMesssageIntent() == MessageIntentEnum.Send;

            if (!isCommandMessage)
            {
                return;
            }

            var endpointInstance = context.Builder.Build<IEndpointInstance>();
            CommandHandlingFeedbackEvent commandHandlingFeedbackEvent;

            if (wasMessageHandledSuccessfully)
            {
                var trackingEventsDatabase = context.Builder.Build<ITrackingEventsDatabase>();
                var publishedEventIds = trackingEventsDatabase.WrittenEventEnvelopes.Select(e => e.MessageId).ToList();

                commandHandlingFeedbackEvent = new CommandHandlingSucceededEvent()
                {
                    PublishedEventIds = publishedEventIds
                };
            }
            else
            {
                commandHandlingFeedbackEvent = new CommandHandlingFailedEvent();
            }

            commandHandlingFeedbackEvent.CommandId = Guid.Parse(context.Message.MessageId);

            var publishOptions = new PublishOptions();
            publishOptions.RequireImmediateDispatch();
            await endpointInstance.Publish(commandHandlingFeedbackEvent, publishOptions);
        }
    }
}
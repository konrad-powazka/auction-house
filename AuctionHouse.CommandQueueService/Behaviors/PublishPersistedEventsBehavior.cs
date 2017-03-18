using System;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using NServiceBus.Pipeline;

namespace AuctionHouse.CommandQueueService.Behaviors
{
    public class PublishPersistedEventsBehavior :
        Behavior<ITransportReceiveContext>
    {
        public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
	        await next();
			var trackingEventsDatabase = context.Builder.Build<ITrackingEventsDatabase>();
			var eventPublisher = context.Builder.Build<IEventPublisher>();

	        foreach (var writtenEventEnvelope in trackingEventsDatabase.WrittenEventEnvelopes)
	        {
		        await eventPublisher.Publish(writtenEventEnvelope);
	        }
        }
    }
}
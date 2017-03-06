using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using NServiceBus;
using IEvent = AuctionHouse.Core.Messaging.IEvent;

namespace AuctionHouse.ServiceBus.Handlers
{
	public class NServiceBusEventMessageHandler<TEventHandler, TEvent> : IHandleMessages<TEvent>
		where TEventHandler : IEventHandler<TEvent> where TEvent : IEvent
	{
		private readonly IEventHandler<TEvent> _eventHandler;

		public NServiceBusEventMessageHandler(TEventHandler eventHandler)
		{
			_eventHandler = eventHandler;
		}

		public async Task Handle(TEvent @event, IMessageHandlerContext context)
		{
			var eventId = Guid.Parse(context.MessageId);
			var eventEnvelope = new MessageEnvelope<TEvent>(@event, eventId);
			await _eventHandler.Handle(eventEnvelope);
		}
	}
}
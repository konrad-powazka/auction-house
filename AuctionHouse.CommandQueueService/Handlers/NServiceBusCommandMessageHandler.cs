using System;
using System.Linq;
using System.Threading.Tasks;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Events.Technical;
using Autofac;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;

namespace AuctionHouse.CommandQueueService.Handlers
{
	public class NServiceBusCommandMessageHandler : IHandleMessages<ICommand>
	{
		private readonly IComponentContext _componentContext;
		private readonly ITrackingEventsDatabase _trackingEventsDatabase;

		public NServiceBusCommandMessageHandler(IComponentContext componentContext,
			ITrackingEventsDatabase trackingEventsDatabase)
		{
			if (componentContext == null)
			{
				throw new ArgumentNullException(nameof(componentContext));
			}

			if (trackingEventsDatabase == null)
			{
				throw new ArgumentNullException(nameof(trackingEventsDatabase));
			}

			_componentContext = componentContext;
			_trackingEventsDatabase = trackingEventsDatabase;
		}

		public async Task Handle(ICommand command, IMessageHandlerContext context)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}

			var senderUserName = context.MessageHeaders[MessageHeaderNames.SenderUserName];
			var commandId = Guid.Parse(context.MessageId);
			await Handle((dynamic) command, commandId, senderUserName, context);
		}

		private async Task Handle<TCommand>(TCommand command, Guid commandId, string senderUserName,
			IMessageHandlerContext context)
			where TCommand : ICommand
		{
			var commandHandler = _componentContext.Resolve<ICommandHandler<TCommand>>();
			var commandEnvelope = new CommandEnvelope<TCommand>(command, commandId, senderUserName);

			try
			{
				await commandHandler.Handle(commandEnvelope);
			}
			catch
			{
				await PublishCommandHandlingFeedbackEvent(false, commandId, senderUserName, context);
				throw;
			}

			await PublishCommandHandlingFeedbackEvent(true, commandId, senderUserName, context);
		}

		private async Task PublishCommandHandlingFeedbackEvent(bool wasCommandHandledSuccessfully, Guid commandId,
			string senderUserName, IMessageHandlerContext context)
		{
			CommandHandlingFeedbackEvent commandHandlingFeedbackEvent;

			if (wasCommandHandledSuccessfully)
			{
				var publishedEventIds = _trackingEventsDatabase.WrittenEventEnvelopes.Select(e => e.EventId).ToList();

				commandHandlingFeedbackEvent = new CommandHandlingSucceededEvent
				{
					PublishedEventIds = publishedEventIds
				};
			}
			else
			{
				commandHandlingFeedbackEvent = new CommandHandlingFailedEvent();
			}

			commandHandlingFeedbackEvent.CommandId = commandId;
			commandHandlingFeedbackEvent.CommandSenderUserName = senderUserName;

			var publishOptions = new PublishOptions();
			publishOptions.RequireImmediateDispatch();
			await context.Publish(commandHandlingFeedbackEvent, publishOptions);
		}
	}
}
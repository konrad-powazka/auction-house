using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using NServiceBus;
using ICommand = AuctionHouse.Core.Messaging.ICommand;

namespace AuctionHouse.Web.Cqrs
{
	public class NServiceBusCommandQueue : ICommandQueue
	{
		private readonly IEndpointInstance _endpoint;
		private readonly ITimeProvider _timeProvider;

		public NServiceBusCommandQueue(IEndpointInstance endpoint, ITimeProvider timeProvider)
		{
			if (endpoint == null)
			{
				throw new ArgumentNullException(nameof(endpoint));
			}

			if (timeProvider == null)
			{
				throw new ArgumentNullException(nameof(timeProvider));
			}

			_endpoint = endpoint;
			_timeProvider = timeProvider;
		}

		public async Task QueueCommand<TCommand>(TCommand command, Guid commandId, string senderUserName,
			DateTime? delayedUntil = null)
			where TCommand : ICommand
		{
			var sendOptions = new SendOptions();
			sendOptions.SetMessageId(commandId.ToString());
			sendOptions.SetDestination(Configuration.NServiceBusCommandHandlingDestination);
			sendOptions.SetHeader(MessageHeaderNames.SenderUserName, senderUserName);

			if (delayedUntil.HasValue)
			{
				var deliveryDelay = delayedUntil.Value - _timeProvider.Now;
				sendOptions.DelayDeliveryWith(deliveryDelay);
			}

			await _endpoint.Send(command, sendOptions);
		}
	}
}
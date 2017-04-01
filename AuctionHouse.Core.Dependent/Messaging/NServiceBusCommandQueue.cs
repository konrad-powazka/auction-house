using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Time;
using NServiceBus;

namespace AuctionHouse.Core.Messaging
{
	public class NServiceBusCommandQueue : ICommandQueue
	{
		private readonly INServiceBusCommandQueueConfiguration _configuration;
		private readonly IEndpointInstance _endpoint;
		private readonly ITimeProvider _timeProvider;

		public NServiceBusCommandQueue(IEndpointInstance endpoint, ITimeProvider timeProvider,
			INServiceBusCommandQueueConfiguration configuration)
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
			_configuration = configuration;
		}

		public async Task QueueCommand<TCommand>(TCommand command, Guid commandId, string senderUserName,
			DateTime? delayedUntil = null)
			where TCommand : ICommand
		{
			var sendOptions = new SendOptions();
			sendOptions.SetMessageId(commandId.ToString());
			sendOptions.SetDestination(_configuration.NServiceBusCommandHandlingDestination);
			sendOptions.SetHeader(MessageHeaderNames.SenderUserName, senderUserName);

			if (delayedUntil.HasValue)
			{
				var deliveryDelay = delayedUntil.Value - _timeProvider.UtcNow;

				if (deliveryDelay > TimeSpan.Zero)
				{
					sendOptions.DelayDeliveryWith(deliveryDelay);
				}
			}

			await _endpoint.Send(command, sendOptions);
		}
	}
}
using System.Threading.Tasks;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.UserMessaging;
using AuctionHouse.Messages.Commands.UserMessaging;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.UserMessaging
{
	public class SendUserMessageCommandHandler : ICommandHandler<SendUserMessageCommand>
	{
		private readonly IRepository<SentUserMessage> _sentUserMessageRepository;
		private readonly ITimeProvider _timeProvider;

		public SendUserMessageCommandHandler(IRepository<SentUserMessage> sentUserMessageRepository,
			ITimeProvider timeProvider)
		{
			_sentUserMessageRepository = sentUserMessageRepository;
			_timeProvider = timeProvider;
		}

		public async Task Handle(ICommandEnvelope<SendUserMessageCommand> commandEnvelope)
		{
			var command = commandEnvelope.Command;

			var createdUserMessage = SentUserMessage.Create(command.MessageSubject, command.MessageBody,
				commandEnvelope.SenderUserName, command.RecipientUserName, _timeProvider);

			await _sentUserMessageRepository.Create(createdUserMessage, commandEnvelope.CommandId.ToString());
		}
	}
}
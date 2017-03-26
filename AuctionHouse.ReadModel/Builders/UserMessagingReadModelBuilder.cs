using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.EventSourcing;
using AuctionHouse.Core.ReadModel;
using AuctionHouse.Messages.Events.UserMessaging;
using AuctionHouse.ReadModel.Dtos.UserMessaging;

namespace AuctionHouse.ReadModel.Builders
{
	public class UserMessagingReadModelBuilder : IReadModelBuilder
	{
		public async Task Apply(PersistedEventEnvelope eventEnvelope, IReadModelDbContext readModelDbContext)
		{
			// TODO: To async
			TypeSwitch.Do(eventEnvelope.Event,
				TypeSwitch.Case<UserMessageSentEvent>(userMessageSentEvent =>
				{
					var sentUserMessage = new UserMessageReadModel
					{
						Id = userMessageSentEvent.MessageId,
						RecipientUserName = userMessageSentEvent.RecipientUserName,
						SentDateTime = userMessageSentEvent.SentDateTime,
						SenderUserName = userMessageSentEvent.SenderUserName,
						Body = userMessageSentEvent.MessageBody,
						Subject = userMessageSentEvent.MessageSubject
					};

					readModelDbContext.CreateOrOverwrite(sentUserMessage, sentUserMessage.Id.ToString());
				}));
		}
	}
}
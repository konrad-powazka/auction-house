using System;
using AuctionHouse.Core.Domain;
using AuctionHouse.Core.Time;
using AuctionHouse.Messages.Events.UserMessaging;

namespace AuctionHouse.Domain.UserMessaging
{
	public class SentUserMessage : AggregateRoot
	{
		public static SentUserMessage Create(string subject, string body, string senderUserName, string recipientUserName,
			ITimeProvider timeProvider)
		{
			if (string.IsNullOrWhiteSpace(subject))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(subject));
			}

			if (string.IsNullOrWhiteSpace(body))
			{
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(body));
			}

			var sentUserMessage = new SentUserMessage();

			var userMessageSentEvent = new UserMessageSentEvent
			{
				MessageId = Guid.NewGuid(),
				MessageSubject = subject,
				MessageBody = body,
				SenderUserName = senderUserName,
				RecipientUserName = recipientUserName,
				SentDateTime = timeProvider.Now
			};

			sentUserMessage.ApplyChange(userMessageSentEvent);

			return sentUserMessage;
		}

		protected override void RegisterEventAppliers()
		{
			RegisterEventApplier<UserMessageSentEvent>(Apply);
		}

		private void Apply(UserMessageSentEvent messageSentEvent)
		{
			Id = messageSentEvent.MessageId;
		}
	}
}
using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.UserMessaging
{
	public class UserMessageSentEvent : IEvent
	{
		public Guid MessageId { get; set; }
		public string MessageSubject { get; set; }
		public string MessageBody { get; set; }
		public string RecipientUserName { get; set; }
		public string SenderUserName { get; set; }
		public DateTime SentDateTime { get; set; }
	}
}
using System;

namespace AuctionHouse.ReadModel.Dtos.UserMessaging
{
	public class UserMessageReadModel
	{
		public Guid Id { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string RecipientUserName { get; set; }
		public string SenderUserName { get; set; }
		public DateTime SentDateTime { get; set; }
	}
}
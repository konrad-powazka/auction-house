using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Commands.UserMessaging
{
	public class SendUserMessageCommand : ICommand
	{
		public string MessageSubject { get; set; }
		public string MessageBody { get; set; }
		public string RecipientUserName { get; set; }
	}
}
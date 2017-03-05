using System;

namespace AuctionHouse.Core.Messaging
{
	public interface IMessageEnvelope<out TMessage> where TMessage : IMessage
	{
		TMessage Message { get; }
		Guid MessageId { get; }
	}
}
using System;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Messages.Events.Auctions
{
	public class AuctionFinishedEvent : IEvent
	{
		public Guid AuctionId { get; set; }
	}
}
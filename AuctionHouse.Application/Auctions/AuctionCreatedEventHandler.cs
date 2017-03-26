using System;
using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Messages.Events.Auctions;

namespace AuctionHouse.Application.Auctions
{
	public class AuctionCreatedEventHandler : IEventHandler<AuctionCreatedEvent>
	{
		private static readonly Guid HandleAuctionCreatedEventNamespaceId = new Guid("a100fcb9-deed-4654-ba6d-344a9618c886");
		private readonly ICommandQueue _commandQueue;

		public AuctionCreatedEventHandler(ICommandQueue commandQueue)
		{
			_commandQueue = commandQueue;
		}

		public async Task Handle(IEventEnvelope<AuctionCreatedEvent> eventEnvelope)
		{
			var finishAuctionCommand = new FinishAuctionCommand
			{
				Id = eventEnvelope.Event.Id
			};

			var commandId = GuidGenerator.GenerateDeterministicGuid(HandleAuctionCreatedEventNamespaceId,
				eventEnvelope.EventId.ToString());

			await _commandQueue.QueueCommand(finishAuctionCommand, commandId, "system", eventEnvelope.Event.EndDateTime);
		}
	}
}
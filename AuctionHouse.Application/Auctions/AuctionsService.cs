using System;
using System.Threading.Tasks;
using AuctionHouse.Core;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Time;
using AuctionHouse.Domain.Auctions;
using AuctionHouse.Messages.Commands.Auctions;
using AuctionHouse.Messages.Events.Auctions;
using AuctionHouse.Persistence;

namespace AuctionHouse.Application.Auctions
{
	public class AuctionsService :
		ICommandHandler<CreateAuctionCommand>,
		ICommandHandler<MakeBidCommand>,
		ICommandHandler<FinishAuctionCommand>,
		IEventHandler<AuctionCreatedEvent>
	{
		private static readonly Guid HandleAuctionCreatedEventNamespaceId = new Guid("a100fcb9-deed-4654-ba6d-344a9618c886");
		private readonly IRepository<Auction> _auctionsRepository;
		private readonly ICommandQueue _commandQueue;
		private readonly ITimeProvider _timeProvider;

		public AuctionsService(IRepository<Auction> auctionsRepository, ITimeProvider timeProvider, ICommandQueue commandQueue)
		{
			_auctionsRepository = auctionsRepository;
			_timeProvider = timeProvider;
			_commandQueue = commandQueue;
		}

		public async Task Handle(CommandEnvelope<CreateAuctionCommand> commandEnvelope)
		{
			var command = commandEnvelope.Message;

			var createdAuction = Auction.Create(command.Id, command.Title, command.Description, command.EndDate,
				command.StartingPrice, command.BuyNowPrice, commandEnvelope.SenderUserName, _timeProvider);

			await _auctionsRepository.Create(createdAuction, commandEnvelope.MessageId.ToString());
		}

		public async Task Handle(CommandEnvelope<MakeBidCommand> commandEnvelope)
		{
			var makeBidCommand = commandEnvelope.Message;
			var auction = await _auctionsRepository.Get(makeBidCommand.AuctionId);
			auction.MakeBid(commandEnvelope.SenderUserName, makeBidCommand.Price);

			await
				_auctionsRepository.Save(auction, commandEnvelope.MessageId.ToString(), ExpectedAggregateRootVersion.Specific,
					makeBidCommand.ExpectedAuctionVersion);
		}

		public async Task Handle(CommandEnvelope<FinishAuctionCommand> commandEnvelope)
		{
			var auction = await _auctionsRepository.Get(commandEnvelope.Message.Id);
			auction.Finish();
			await _auctionsRepository.Save(auction, commandEnvelope.MessageId.ToString(), ExpectedAggregateRootVersion.Any);
		}

		public async Task Handle(IMessageEnvelope<AuctionCreatedEvent> eventEnvelope)
		{
			var finishAuctionCommand = new FinishAuctionCommand
			{
				Id = eventEnvelope.Message.Id
			};

			var commandId = GuidGenerator.GenerateDeterministicGuid(HandleAuctionCreatedEventNamespaceId,
				eventEnvelope.MessageId.ToString());

			await _commandQueue.QueueCommand(finishAuctionCommand, commandId, "system", eventEnvelope.Message.EndDateTime);
		}
	}
}
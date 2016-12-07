
namespace AuctionHouse.CommandHandling {
	export class CancelAuctionCommandHandler extends CommandHandler<AuctionHouse.Messages.Commands.Auctions.CancelAuctionCommand> {
		protected getCommandName(): string {
			return 'CancelAuctionCommand';
		}
	}
	export class CreateAuctionCommandHandler extends CommandHandler<AuctionHouse.Messages.Commands.Auctions.CreateAuctionCommand> {
		protected getCommandName(): string {
			return 'CreateAuctionCommand';
		}
	}
	export class MakeBidCommandHandler extends CommandHandler<AuctionHouse.Messages.Commands.Auctions.MakeBidCommand> {
		protected getCommandName(): string {
			return 'MakeBidCommand';
		}
	}

export class AngularCommandHandlersRegistry {
	static commandHandlers: {[name: string]: ng.Injectable<Function>} = {
							'CancelAuctionCommandHandler': CancelAuctionCommandHandler,
							'CreateAuctionCommandHandler': CreateAuctionCommandHandler,
							'MakeBidCommandHandler': MakeBidCommandHandler,
					};
	}
}
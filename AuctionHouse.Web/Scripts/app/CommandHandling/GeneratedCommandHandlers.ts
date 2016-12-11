
import { CommandHandler } from './CommandHandler';
import * as Commands from '../Messages/Commands';

	export class CancelAuctionCommandHandler extends CommandHandler<Commands.CancelAuctionCommand> {
		protected getCommandName(): string {
			return 'CancelAuctionCommand';
		}
	}
	export class CreateAuctionCommandHandler extends CommandHandler<Commands.CreateAuctionCommand> {
		protected getCommandName(): string {
			return 'CreateAuctionCommand';
		}
	}
	export class MakeBidCommandHandler extends CommandHandler<Commands.MakeBidCommand> {
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
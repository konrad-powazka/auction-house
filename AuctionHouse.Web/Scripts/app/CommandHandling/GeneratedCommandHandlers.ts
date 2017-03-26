
import { CommandHandler } from './CommandHandler';
import * as Commands from '../Messages/Commands';

	export class SendUserMessageCommandHandler extends CommandHandler<Commands.SendUserMessageCommand> {
		protected getCommandName(): string {
			return 'SendUserMessageCommand';
		}
	}
	export class PopulateDatabaseWithTestDataCommandHandler extends CommandHandler<Commands.PopulateDatabaseWithTestDataCommand> {
		protected getCommandName(): string {
			return 'PopulateDatabaseWithTestDataCommand';
		}
	}
	export class CreateAuctionCommandHandler extends CommandHandler<Commands.CreateAuctionCommand> {
		protected getCommandName(): string {
			return 'CreateAuctionCommand';
		}
	}
	export class FinishAuctionCommandHandler extends CommandHandler<Commands.FinishAuctionCommand> {
		protected getCommandName(): string {
			return 'FinishAuctionCommand';
		}
	}
	export class MakeBidCommandHandler extends CommandHandler<Commands.MakeBidCommand> {
		protected getCommandName(): string {
			return 'MakeBidCommand';
		}
	}

export class AngularCommandHandlersRegistry {
	static commandHandlers: {[name: string]: ng.Injectable<Function>} = {
							'sendUserMessageCommandHandler': SendUserMessageCommandHandler,
							'populateDatabaseWithTestDataCommandHandler': PopulateDatabaseWithTestDataCommandHandler,
							'createAuctionCommandHandler': CreateAuctionCommandHandler,
							'finishAuctionCommandHandler': FinishAuctionCommandHandler,
							'makeBidCommandHandler': MakeBidCommandHandler,
					};
	}
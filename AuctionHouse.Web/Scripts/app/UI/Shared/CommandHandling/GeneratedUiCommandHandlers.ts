
import { CommandUiHandler } from './CommandUiHandler';
import { ICommandHandler } from '../../../CommandHandling/ICommandHandler';
import * as Commands from '../../../Messages/Commands';
import {SecurityUiService } from '../SecurityUiService';
import BusyIndicator from '../BusyIndicator';

	export class PopulateDatabaseWithTestDataCommandUiHandler extends CommandUiHandler<Commands.PopulateDatabaseWithTestDataCommand> {
		static $inject: ['populateDatabaseWithTestDataCommandHandler', 'busyIndicator', 'securityUiService'];

		constructor(
			private populateDatabaseWithTestDataCommandHandler: ICommandHandler<Commands.PopulateDatabaseWithTestDataCommand>, busyIndicator: BusyIndicator, securityUiService: SecurityUiService) {
			super(busyIndicator, securityUiService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.PopulateDatabaseWithTestDataCommand> {
			return this.populateDatabaseWithTestDataCommandHandler;
		}
	}
	export class CreateAuctionCommandUiHandler extends CommandUiHandler<Commands.CreateAuctionCommand> {
		static $inject: ['createAuctionCommandHandler', 'busyIndicator', 'securityUiService'];

		constructor(
			private createAuctionCommandHandler: ICommandHandler<Commands.CreateAuctionCommand>, busyIndicator: BusyIndicator, securityUiService: SecurityUiService) {
			super(busyIndicator, securityUiService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.CreateAuctionCommand> {
			return this.createAuctionCommandHandler;
		}
	}
	export class FinishAuctionCommandUiHandler extends CommandUiHandler<Commands.FinishAuctionCommand> {
		static $inject: ['finishAuctionCommandHandler', 'busyIndicator', 'securityUiService'];

		constructor(
			private finishAuctionCommandHandler: ICommandHandler<Commands.FinishAuctionCommand>, busyIndicator: BusyIndicator, securityUiService: SecurityUiService) {
			super(busyIndicator, securityUiService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.FinishAuctionCommand> {
			return this.finishAuctionCommandHandler;
		}
	}
	export class MakeBidCommandUiHandler extends CommandUiHandler<Commands.MakeBidCommand> {
		static $inject: ['makeBidCommandHandler', 'busyIndicator', 'securityUiService'];

		constructor(
			private makeBidCommandHandler: ICommandHandler<Commands.MakeBidCommand>, busyIndicator: BusyIndicator, securityUiService: SecurityUiService) {
			super(busyIndicator, securityUiService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.MakeBidCommand> {
			return this.makeBidCommandHandler;
		}
	}

export class AngularCommandUiHandlersRegistry {
	static commandUiHandlers: {[name: string]: ng.Injectable<Function>} = {
							'populateDatabaseWithTestDataCommandUiHandler': PopulateDatabaseWithTestDataCommandUiHandler,
							'createAuctionCommandUiHandler': CreateAuctionCommandUiHandler,
							'finishAuctionCommandUiHandler': FinishAuctionCommandUiHandler,
							'makeBidCommandUiHandler': MakeBidCommandUiHandler,
					};
	}
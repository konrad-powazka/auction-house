
import { CommandUiHandler } from './CommandUiHandler';
import { ICommandHandler } from '../../../CommandHandling/ICommandHandler';
import * as Commands from '../../../Messages/Commands';
import {SecurityUiService } from '../SecurityUiService';
import BusyIndicator from '../BusyIndicator';

	export class CancelAuctionCommandUiHandler extends CommandUiHandler<Commands.CancelAuctionCommand> {
		static $inject: ['cancelAuctionCommandHandler', 'busyIndicator', 'securityUiService'];

		constructor(
			private cancelAuctionCommandHandler: ICommandHandler<Commands.CancelAuctionCommand>, busyIndicator: BusyIndicator, securityUiService: SecurityUiService) {
			super(busyIndicator, securityUiService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.CancelAuctionCommand> {
			return this.cancelAuctionCommandHandler;
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
							'cancelAuctionCommandUiHandler': CancelAuctionCommandUiHandler,
							'createAuctionCommandUiHandler': CreateAuctionCommandUiHandler,
							'makeBidCommandUiHandler': MakeBidCommandUiHandler,
					};
	}
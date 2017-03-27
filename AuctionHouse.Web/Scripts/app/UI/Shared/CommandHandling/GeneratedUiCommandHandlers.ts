
import { CommandUiHandler } from './CommandUiHandler';
import { ICommandHandler } from '../../../CommandHandling/ICommandHandler';
import * as Commands from '../../../Messages/Commands';
import {SecurityUiService } from '../SecurityUiService';
import BusyIndicator from '../BusyIndicator';
import GenericModalService from '../GenericModalService';

	export class SendUserMessageCommandUiHandler extends CommandUiHandler<Commands.SendUserMessageCommand> {
		static $inject = ['sendUserMessageCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];

		constructor(
			private sendUserMessageCommandHandler: ICommandHandler<Commands.SendUserMessageCommand>, 
				busyIndicator: BusyIndicator, 
				securityUiService: SecurityUiService,
				qService: ng.IQService,
				genericModalService: GenericModalService) {
			super(busyIndicator, securityUiService, qService, genericModalService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.SendUserMessageCommand> {
			return this.sendUserMessageCommandHandler;
		}
	}
	export class CreateAuctionCommandUiHandler extends CommandUiHandler<Commands.CreateAuctionCommand> {
		static $inject = ['createAuctionCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];

		constructor(
			private createAuctionCommandHandler: ICommandHandler<Commands.CreateAuctionCommand>, 
				busyIndicator: BusyIndicator, 
				securityUiService: SecurityUiService,
				qService: ng.IQService,
				genericModalService: GenericModalService) {
			super(busyIndicator, securityUiService, qService, genericModalService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.CreateAuctionCommand> {
			return this.createAuctionCommandHandler;
		}
	}
	export class FinishAuctionCommandUiHandler extends CommandUiHandler<Commands.FinishAuctionCommand> {
		static $inject = ['finishAuctionCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];

		constructor(
			private finishAuctionCommandHandler: ICommandHandler<Commands.FinishAuctionCommand>, 
				busyIndicator: BusyIndicator, 
				securityUiService: SecurityUiService,
				qService: ng.IQService,
				genericModalService: GenericModalService) {
			super(busyIndicator, securityUiService, qService, genericModalService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.FinishAuctionCommand> {
			return this.finishAuctionCommandHandler;
		}
	}
	export class MakeBidCommandUiHandler extends CommandUiHandler<Commands.MakeBidCommand> {
		static $inject = ['makeBidCommandHandler', 'busyIndicator', 'securityUiService', '$q', 'genericModalService'];

		constructor(
			private makeBidCommandHandler: ICommandHandler<Commands.MakeBidCommand>, 
				busyIndicator: BusyIndicator, 
				securityUiService: SecurityUiService,
				qService: ng.IQService,
				genericModalService: GenericModalService) {
			super(busyIndicator, securityUiService, qService, genericModalService);
		}

		protected getCommandHandler(): ICommandHandler<Commands.MakeBidCommand> {
			return this.makeBidCommandHandler;
		}
	}

export class AngularCommandUiHandlersRegistry {
	static commandUiHandlers: {[name: string]: ng.Injectable<Function>} = {
							'sendUserMessageCommandUiHandler': SendUserMessageCommandUiHandler,
							'createAuctionCommandUiHandler': CreateAuctionCommandUiHandler,
							'finishAuctionCommandUiHandler': FinishAuctionCommandUiHandler,
							'makeBidCommandUiHandler': MakeBidCommandUiHandler,
					};
	}
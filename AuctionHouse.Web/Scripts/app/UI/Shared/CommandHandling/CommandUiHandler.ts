import { ICommandUiHandler } from './ICommandUiHandler';
import BusyIndicator from '../BusyIndicator';
import {SecurityUiService } from '../SecurityUiService';
import {ICommandHandler } from '../../../CommandHandling/ICommandHandler';
import {CommandHandlingErrorType} from '../../../CommandHandling/CommandHandlingErrorType';
import GenericModalService from '../GenericModalService';
import {NotificationType} from '../NotificationType';

export abstract class CommandUiHandler<TCommand> implements ICommandUiHandler<TCommand> {
	$inject = ['busyIndicator', 'securityUiService', '$q', 'genericModalService'];

	constructor(private busyIndicator: BusyIndicator,
		private securityUiService: SecurityUiService,
		private qService: ng.IQService,
		private genericModalService: GenericModalService) {
	}

	protected abstract getCommandHandler(): ICommandHandler<TCommand>;

	handle(command: TCommand, commandId: string, shouldWaitForEventsApplicationToReadModel: boolean): angular.IPromise<void> {
		return this.securityUiService.ensureUserIsAuthenticated()
			.then(() => {
				const promise = this.getCommandHandler().handle(command, commandId, shouldWaitForEventsApplicationToReadModel);

					return this.busyIndicator.attachToPromise(promise)
						.catch((commandHandlingErrorType: CommandHandlingErrorType) => {
							var actionData = this.getActionDataForCommandHandlingErrorType(commandHandlingErrorType);

							this.genericModalService
								.showNotification(actionData.notificationMessage, actionData.notificationType);

							return this.qService.reject();
						});
				}
			);
	}

	private getActionDataForCommandHandlingErrorType(commandHandlingErrorType: CommandHandlingErrorType):
	{ notificationType: NotificationType, notificationMessage: string } {
		switch (commandHandlingErrorType) {
		case CommandHandlingErrorType.FailedToConnectToFeedbackHub:
		case CommandHandlingErrorType.FailedToQueue:
		case CommandHandlingErrorType.FailedToProcess:
			return {
				notificationType: NotificationType.Error,
				notificationMessage: 'Failed to process you request. Please try again.'
			};
		case CommandHandlingErrorType.ProcessingTimeout:
			return {
				notificationType: NotificationType.Error,
				notificationMessage:
					'Your request timed out. Please check whether its effects are visible or try again.'
			};
		case CommandHandlingErrorType.FailedToSubscribeToReadModelChangeNotification:
		case CommandHandlingErrorType.ReadModelChangeNotificationTimeout:
			return {
				notificationType: NotificationType.Information,
				notificationMessage: 'Your request was handled, but its effects may not be visible for a while.'
			};
		default:
			return {
				notificationType: NotificationType.Error,
				notificationMessage: 'An unknown error occurred.'
			};
		}
	}
}
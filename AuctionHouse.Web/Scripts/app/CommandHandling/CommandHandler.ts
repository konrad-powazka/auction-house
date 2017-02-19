import { ICommandHandler } from './ICommandHandler';
import { CommandHandlingErrorType } from './CommandHandlingErrorType'
import { IEventAppliedToReadModelNotificationHubServer } from
    '../Infrastructure/IEventAppliedToReadModelNotificationHubServer';
import { NotifyOnEventsAppliedToReadModelResponse } from '../Infrastructure/NotifyOnEventsAppliedToReadModelResponse';
import { CommandHandlingSucceededEvent, CommandHandlingFailedEvent } from '../Events';
import GuidGenerator from '../Infrastructure/GuidGenerator';

export abstract class CommandHandler<TCommand> implements ICommandHandler<TCommand> {
    private static wasSignalrRInitialized = false;
    private static commandHandlingSuccessCallbacks = $.Callbacks();
    private static commandHandlingFailureCallbacks = $.Callbacks();
    private static eventsAppliedToReadModelCallbacks = $.Callbacks();
    private static eventAppliedToReadModelNotificationHubServer: IEventAppliedToReadModelNotificationHubServer;

    static $inject = ['$http', '$q', '$timeout'];

    constructor(
        private httpService: ng.IHttpService,
        private qService: ng.IQService,
        private timeoutService: ng.ITimeoutService) {
        if (!CommandHandler.wasSignalrRInitialized) {
            const connection = ($.connection as any);
            const commandHandlingFeedbackHub = connection.commandHandlingFeedbackHub;

            commandHandlingFeedbackHub.client.handleCommandSuccess = (commandHandlingSucceededEvent: any): void => {
                CommandHandler.commandHandlingSuccessCallbacks.fire(commandHandlingSucceededEvent);
            };

            commandHandlingFeedbackHub.client.handleCommandFailure = (commandHandlingFailedEvent: any): void => {
                CommandHandler.commandHandlingFailureCallbacks.fire(commandHandlingFailedEvent);
            };

            const eventAppliedToReadModelNotificationHub = connection.eventAppliedToReadModelNotificationHub;

            eventAppliedToReadModelNotificationHub.client
                .handleEventsAppliedToReadModel = (subscriptionId: string): void => {
                    CommandHandler.eventsAppliedToReadModelCallbacks.fire(subscriptionId);
                };

            CommandHandler
                .eventAppliedToReadModelNotificationHubServer = eventAppliedToReadModelNotificationHub.server as any;

            CommandHandler.wasSignalrRInitialized = true;
        }
    }

    handle(command: TCommand, shouldWaitForEventsApplicationToReadModel: boolean): ng.IPromise<void> {
        const commandId = GuidGenerator.generateGuid();
        const deferred = this.qService.defer<void>();

        this.connectSignalR()
            .then(() => {
                this.sendCommandAndWaitForHandling(command, commandId, shouldWaitForEventsApplicationToReadModel, deferred);
            })
            .catch(() => deferred.reject(CommandHandlingErrorType.FailedToConnectToFeedbackHub));

        return deferred.promise;
    }

    private sendCommandAndWaitForHandling(command: TCommand,
        commandId: string,
        shouldWaitForEventsApplicationToReadModel: boolean,
        deferred: ng.IDeferred<void>): void {

        const commandHandlingSuccessCallback = (commandHandlingSucceededEvent: CommandHandlingSucceededEvent) => {
            if (commandHandlingSucceededEvent.commandId === commandId) {

                if (!shouldWaitForEventsApplicationToReadModel) {
                    deferred.resolve();
                } else {
                    this
                        .waitForEventsApplicationToReadModel(commandHandlingSucceededEvent
                            .publishedEventIds,
                            deferred);
                }
            }
        };

        const commandHandlingFailureCallback = (commandHandlingFailedEvent: CommandHandlingFailedEvent) => {
            if (commandHandlingFailedEvent.commandId === commandId) {
                deferred.reject(CommandHandlingErrorType.FailedToProcess);
            }
        };

        CommandHandler.commandHandlingSuccessCallbacks.add(commandHandlingSuccessCallback);
        CommandHandler.commandHandlingFailureCallbacks.add(commandHandlingFailureCallback);

        this.sendCommand(command, commandId)
            .then(() => {
                const commandHandlingTimeoutMilliseconds = 15 * 1000;

                this.timeoutService(commandHandlingTimeoutMilliseconds)
                    .then(() => {
                        deferred.reject(CommandHandlingErrorType.Timeout);
                    });
            })
            .catch(() => deferred.reject(CommandHandlingErrorType.FailedToQueue));

        const removeCallbacks = () => {
            CommandHandler.commandHandlingSuccessCallbacks.remove(commandHandlingSuccessCallback);
            CommandHandler.commandHandlingFailureCallbacks.remove(commandHandlingFailureCallback);
        };

        deferred.promise.finally(removeCallbacks);
    }

    private sendCommand(command: TCommand, commandId: string): ng.IHttpPromise<void> {
        const url = `api/${this.getCommandName()}/Handle?commandId=${commandId}`;

        return this.httpService.post<void>(url, command);
    }

    private waitForEventsApplicationToReadModel(publishedEventIds: string[], deferred: ng.IDeferred<void>): void {
        CommandHandler
            .eventAppliedToReadModelNotificationHubServer
            .notifyOnEventsApplied(publishedEventIds)
            .done((notifyOnEventsAppliedToReadModelResponse: NotifyOnEventsAppliedToReadModelResponse) => {
                if (notifyOnEventsAppliedToReadModelResponse.wereAllEventsAlreadyApplied) {
                    deferred.resolve();
                    return;
                }

                const eventsAppliedCallback = (currentSubscriptionId: string) => {
                    if (currentSubscriptionId === notifyOnEventsAppliedToReadModelResponse.subscriptionId) {
                        deferred.resolve();
                    }
                };

                CommandHandler.eventsAppliedToReadModelCallbacks.add(eventsAppliedCallback);

                deferred.promise.finally(() => CommandHandler.eventsAppliedToReadModelCallbacks
                    .remove(eventsAppliedCallback));

                deferred.promise.catch(() => {
                    // TODO: cancel subscription on timeout
                });
            })
            .fail(() => deferred.reject(CommandHandlingErrorType.FailedToSubscribeToReadModelChangeNotification));
    }

    private connectSignalR(): ng.IPromise<void> {
        const deferred = this.qService.defer<void>();

        if ($.connection.hub.state === SignalR.ConnectionState.Connected) {
            deferred.resolve();
        } else {
            $.connection.hub
                .start()
                .done(() => deferred.resolve())
                .fail(() => deferred.reject());
        }

        return deferred.promise;
    }

    protected abstract getCommandName(): string;
}
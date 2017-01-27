import { ICommandHandler } from './ICommandHandler';
import { CommandHandlingErrorType } from './CommandHandlingErrorType'
import {ICommand } from '../Messages/ICommand';
import { IEventAppliedToReadModelNotificationHubServer } from '../Infrastructure/IEventAppliedToReadModelNotificationHubServer';
import { NotifyOnEventsAppliedToReadModelResponse } from '../Infrastructure/NotifyOnEventsAppliedToReadModelResponse';

export abstract class CommandHandler<TCommand extends ICommand> implements ICommandHandler<TCommand> {
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

            eventAppliedToReadModelNotificationHub.client.handleEventsAppliedToReadModel = (subscriptionId: string): void => {
                CommandHandler.eventsAppliedToReadModelCallbacks.fire(subscriptionId);
            };

            CommandHandler
                .eventAppliedToReadModelNotificationHubServer = eventAppliedToReadModelNotificationHub.server as any;

            CommandHandler.wasSignalrRInitialized = true;
        }
    }

    handle(command: TCommand, shouldWaitForEventsApplicationToReadModel: boolean): ng.IPromise<void> {
        const url = `api/${this.getCommandName()}/Handle`;
        const deferred = this.qService.defer<void>();
        
        this.connectSignalR()
            .then(() => {
                this.httpService.post<void>(url, command)
                    .then(() => {
                        var wasPromiseResolvedOrRejected = false;

                        const commandHandlingSuccessCallback = (commandHandlingSucceededEvent: any) => {
                            if (commandHandlingSucceededEvent.commandId === command.id) {
                                wasPromiseResolvedOrRejected = true;

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

                        const commandHandlingFailureCallback = (commandHandlingFailedEvent: any) => {
                            if (commandHandlingFailedEvent.CommandId === command.id) {
                                wasPromiseResolvedOrRejected = true;
                                deferred.reject(CommandHandlingErrorType.FailedToProcess);
                            }
                        };
                        CommandHandler.commandHandlingSuccessCallbacks.add(commandHandlingSuccessCallback);
                        CommandHandler.commandHandlingFailureCallbacks.add(commandHandlingFailureCallback);

                        const removeCallbacks = () => {
                            CommandHandler.commandHandlingSuccessCallbacks.remove(commandHandlingSuccessCallback);
                            CommandHandler.commandHandlingFailureCallbacks.remove(commandHandlingFailureCallback);
                        };

                        deferred.promise.finally(removeCallbacks);
                        const commandHandlingTimeoutMilliseconds = 15 * 1000;

                        this.timeoutService(commandHandlingTimeoutMilliseconds)
                            .then(() => {
                                if (!wasPromiseResolvedOrRejected) {
                                    deferred.reject(CommandHandlingErrorType.Timeout);
                                }
                            });

                    })
                    .catch(() => deferred.reject(CommandHandlingErrorType.FailedToQueue));
            })
            .catch(() => deferred.reject(CommandHandlingErrorType.FailedToConnectToFeedbackHub));

        return deferred.promise;
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
                deferred.promise.finally(() => CommandHandler.eventsAppliedToReadModelCallbacks.remove(eventsAppliedCallback));
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
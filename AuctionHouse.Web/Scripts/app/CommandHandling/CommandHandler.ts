import { ICommandHandler, ICommandHandler as Handler } from './ICommandHandler';
import { CommandHandlingErrorType } from './CommandHandlingErrorType'
import {ICommand } from '../Messages/ICommand';

export abstract class CommandHandler<TCommand extends ICommand> implements Handler<TCommand> {
    private static wasSignalrRInitialized = false;
    private static commandHandlingSuccessCallbacks = $.Callbacks();
    private static commandHandlingFailureCallbacks = $.Callbacks();

    static $inject = ['$http', '$q', '$timeout'];

    constructor(
        private httpService: ng.IHttpService,
        private qService: ng.IQService,
        private timeoutService: ng.ITimeoutService) {
        if (!CommandHandler.wasSignalrRInitialized) {
            const commandHandlingFeedbackHub = ($.connection as any).commandHandlingFeedbackHub;

            commandHandlingFeedbackHub.client.handleCommandSuccess = (commandHandlingSucceededEvent: any): void => {
                CommandHandler.commandHandlingSuccessCallbacks.fire(commandHandlingSucceededEvent);
            };

            commandHandlingFeedbackHub.client.handleCommandFailure = (commandHandlingFailedEvent: any): void => {
                CommandHandler.commandHandlingFailureCallbacks.fire(commandHandlingFailedEvent);
            };

            CommandHandler.wasSignalrRInitialized = true;
        }
    }

    handle(command: TCommand): ng.IPromise<void> {
        const url = `api/${this.getCommandName()}/Handle`;
        const deferred = this.qService.defer<void>();
        
        this.connectSignalR()
            .then(() => {
                this.httpService.post<void>(url, command)
                    .then(() => {
                        var wasPromiseResolvedOrRejected = false;

                        const commandHandlingSuccessCallback = (commandHandlingSucceededEvent: any) => {
                            if (commandHandlingSucceededEvent.CommandId === command.id) {
                                wasPromiseResolvedOrRejected = true;
                                deferred.resolve();
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
                        const commandHandlingTimeoutMilliseconds = 4 * 1000;

                        this.timeoutService(commandHandlingTimeoutMilliseconds)
                            .then(() => {
                                if (!wasPromiseResolvedOrRejected) {
                                    deferred.reject(CommandHandlingErrorType.FeedbackTimeout);
                                }
                            });

                    })
                    .catch(() => deferred.reject(CommandHandlingErrorType.FailedToQueue));
            })
            .catch(() => deferred.reject(CommandHandlingErrorType.FailedToConnectToFeedbackHub));

        return deferred.promise;
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
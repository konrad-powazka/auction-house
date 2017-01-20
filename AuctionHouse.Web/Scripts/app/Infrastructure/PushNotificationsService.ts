import {PushNotificationsSubscription} from './PushNotificationsSubscription';
import {IPushNotificationsService } from './IPushNotificationsService';
import {IQueryResultChangeNotificationHubServer } from './IQueryResultChangeNotificationHubServer';
import {NotifyOnQueryResultChangedResponse} from './NotifyOnQueryResultChangedResponse';

export class PushNotificationsService implements IPushNotificationsService {
    private queryResultChangeNotificationHubServer: IQueryResultChangeNotificationHubServer;
    private wasSignalrRInitialized = false;
    private commandHandlingSuccessCallbacks = $.Callbacks();
    private commandHandlingFailureCallbacks = $.Callbacks();
    private handleQueryResultChangedCallbacks = $.Callbacks();

    static $inject = ['$http', '$q', '$timeout'];

    constructor(
        private httpService: ng.IHttpService,
        private qService: ng.IQService,
        private timeoutService: ng.ITimeoutService) {
            const connection = $.connection as any;
            const commandHandlingFeedbackHub = connection.commandHandlingFeedbackHub;

            commandHandlingFeedbackHub.client.handleCommandSuccess = (commandHandlingSucceededEvent: any): void => {
                this.commandHandlingSuccessCallbacks.fire(commandHandlingSucceededEvent);
            };

            commandHandlingFeedbackHub.client.handleCommandFailure = (commandHandlingFailedEvent: any): void => {
                this.commandHandlingFailureCallbacks.fire(commandHandlingFailedEvent);
            };

            const queryResultChangeNotificationHub = connection.queryResultChangeNotificationHub;
            this.queryResultChangeNotificationHubServer = queryResultChangeNotificationHub.server;

            queryResultChangeNotificationHub.client.handleQueryResultChanged = (subscriptionId: string, queryResult: any): void => {
                this.handleQueryResultChangedCallbacks.fire(subscriptionId, queryResult);
            };
    }

    notifyOnQueryResultChanged<TQuery, TQueryResult>(query: TQuery,
        queryName: string,
        queryResultChangedCallback: (result: TQueryResult) => void): PushNotificationsSubscription {
        const serializedQuery = angular.toJson(query);
        var currentSubscriptionId: string;
        var handleQueryResultChangedCallback: (subscriptionId: string, queryResult: TQueryResult) => void;

        const cancelSubscriptionCallback = () => {
            this.handleQueryResultChangedCallbacks.remove(handleQueryResultChangedCallback);

            // This call may fail, but notifications sent by server from now on will be ignored anyway
            this.queryResultChangeNotificationHubServer
                .cancelNotificationOnResultChanged(currentSubscriptionId);
        };

        const fullActivateSubscriptionPromise =
            this.connectSignalR()
                .then(() => {
                    const activateSubscriptionDeferred = this.qService.defer<void>();

                    this.queryResultChangeNotificationHubServer
                        .notifyOnResultChanged(queryName, serializedQuery)
                        .done((response: NotifyOnQueryResultChangedResponse) => {
                            currentSubscriptionId = response.subscriptionId;

                            handleQueryResultChangedCallback = (subscriptionId: string, queryResult: TQueryResult) => {
                                if (subscriptionId === currentSubscriptionId) {
                                    queryResultChangedCallback(queryResult);
                                }
                            };

                            this.handleQueryResultChangedCallbacks.add(handleQueryResultChangedCallback);
                            activateSubscriptionDeferred.resolve();
                        })
                        .fail(() => activateSubscriptionDeferred.reject());

                    return activateSubscriptionDeferred.promise;
                });

        return new PushNotificationsSubscription(fullActivateSubscriptionPromise, cancelSubscriptionCallback);
    }

    notifyOnCommandHandlingSucceeded(commandHandlingSucceededCallback: (commandHandlingSucceededEvent: any) => void):
    PushNotificationsSubscription {
        throw new Error("Not implemented");
    }

    notifyOnCommandHandlingFailed(commandHandlingFailedCallback: (commandHandlingFailedEvent: any) => void):
    PushNotificationsSubscription {
        throw new Error("Not implemented");
    }

    //private activateSubscription<THubClientCallback>(): PushNotificationsSubscription {
        
    //}
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
}
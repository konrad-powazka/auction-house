import { PushNotificationsSubscription } from './PushNotificationsSubscription';

export interface IPushNotificationsService {
    notifyOnQueryResultChanged<TQuery, TResult>(query: TQuery,
        queryName: string,
        queryResultChangedCallback: (result: TResult) => void): PushNotificationsSubscription;

    notifyOnCommandHandlingSucceeded(commandHandlingSucceededCallback: (commandHandlingSucceededEvent: any) => void):
        PushNotificationsSubscription;

    notifyOnCommandHandlingFailed(commandHandlingFailedCallback: (commandHandlingFailedEvent: any) => void):
        PushNotificationsSubscription;
}
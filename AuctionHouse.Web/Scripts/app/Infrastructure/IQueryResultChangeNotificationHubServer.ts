import {NotifyOnQueryResultChangedResponse} from './NotifyOnQueryResultChangedResponse';

export interface IQueryResultChangeNotificationHubServer {
    notifyOnResultChanged(queryName: string, serializedQuery: string): JQueryDeferred<NotifyOnQueryResultChangedResponse>;
    cancelNotificationOnResultChanged(subscriptionId: string): JQueryDeferred<void>;
}
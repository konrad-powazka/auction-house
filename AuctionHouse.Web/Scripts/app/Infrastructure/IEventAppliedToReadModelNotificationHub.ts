import { NotifyOnQueryResultChangedResponse as NotifyOnEventsAppliedToReadModelResponse } from './NotifyOnEventsAppliedToReadModelResponse';

export interface IEventAppliedToReadModelNotificationHub {
    notifyOnEventsApplied(queryName: string, serializedQuery: string): JQueryDeferred<NotifyOnEventsAppliedToReadModelResponse>;
    cancelNotificationOnEventsApplied(subscriptionId: string): JQueryDeferred<void>;
}
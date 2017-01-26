import { NotifyOnEventsAppliedToReadModelResponse } from './NotifyOnEventsAppliedToReadModelResponse';

export interface IEventAppliedToReadModelNotificationHubServer {
    notifyOnEventsApplied(eventIds: string[]): JQueryDeferred<NotifyOnEventsAppliedToReadModelResponse>;
    cancelNotificationOnEventsApplied(subscriptionId: string): JQueryDeferred<void>;
}
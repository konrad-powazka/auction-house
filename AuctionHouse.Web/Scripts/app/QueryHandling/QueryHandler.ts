﻿import { IQueryHandler } from './IQueryHandler';
import { QueryResultChangedSubscriptionErrorType } from './QueryResultChangedSubscriptionErrorType';
import { IPushNotificationsService } from '../Infrastructure/IPushNotificationsService';

export abstract class QueryHandler<TQuery, TResult> implements IQueryHandler<TQuery, TResult> {
    static $inject = ['$http', '$q'];

    constructor(private httpService: ng.IHttpService,
        private qService: ng.IQService,
        private pushNotificationsService: IPushNotificationsService) {
    }

    handle(query: TQuery): ng.IPromise<TResult> {
        const url = `api/${this.getQueryName()}/Handle`;
        return this.httpService.get<TResult>(url, { params: query });
    }

    // TODO: add timeout
    notifyOnceOnResultChanged(query: TQuery): ng.IPromise<TResult> {
        const deferred = this.qService.defer<TResult>();

        var subscription = this.pushNotificationsService.notifyOnQueryResultChanged<TQuery, TResult>(query,
            this.getQueryName(),
            result => {
                subscription.cancel();
                deferred.resolve(result);
            });

        subscription.activatedPromise.catch(() => deferred
            .reject(QueryResultChangedSubscriptionErrorType.FailedToConnectToServer));

        return deferred.promise;
    }

    protected abstract getQueryName(): string;
}
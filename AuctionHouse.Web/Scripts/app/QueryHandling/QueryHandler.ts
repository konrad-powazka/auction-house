﻿import { IQueryHandler } from './IQueryHandler';
import { QueryResultChangedSubscriptionErrorType } from './QueryResultChangedSubscriptionErrorType';

export abstract class QueryHandler<TQuery, TResult> implements IQueryHandler<TQuery, TResult> {
    static $inject = ['$http', '$q'];

    constructor(private httpService: ng.IHttpService,
        private qService: ng.IQService) {
    }

    handle(query: TQuery): ng.IPromise<TResult> {
        const url = `api/${this.getQueryName()}/Handle`;
        return this.httpService.get<TResult>(url, { params: query }).then(response => response.data);
    }

    protected abstract getQueryName(): string;
}
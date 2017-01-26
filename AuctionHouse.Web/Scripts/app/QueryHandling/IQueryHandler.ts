export interface IQueryHandler<TQuery, TResult> {
    handle(query: TQuery): angular.IPromise<TResult>;
}
namespace AuctionHouse.QueryHandling {
    export abstract class QueryHandler<TQuery, TResult> {
        static $inject = ['$http'];

        constructor(private httpService: ng.IHttpService) {}

        handle(query: TQuery): ng.IPromise<TResult> {
            const url = `api/${this.getQueryName()}/Handle`;
            return this.httpService.get<TResult>(url); //TODO: pass the actual query
        }

        protected abstract getQueryName(): string;
    }
}
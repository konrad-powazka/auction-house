namespace AuctionHouse.QueryHandling {
    export abstract class QueryHandler<TQuery, TResult> {
        static $inject = ['$http'];

        constructor(private httpService: ng.IHttpService) {}

        handle(guery: TQuery): ng.IHttpPromise<TResult> {
            const url = `api/${this.getQueryName()}/Handle`;
            return this.httpService.get<TResult>(url);
        }

        protected abstract getQueryName(): string;
    }
}
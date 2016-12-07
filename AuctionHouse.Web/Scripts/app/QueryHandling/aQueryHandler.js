var AuctionHouse;
(function (AuctionHouse) {
    var QueryHandling;
    (function (QueryHandling) {
        var QueryHandler = (function () {
            function QueryHandler(httpService) {
                this.httpService = httpService;
            }
            QueryHandler.prototype.handle = function (query) {
                var url = "api/" + this.getQueryName() + "/Handle";
                return this.httpService.get(url); //TODO: pass the actual query
            };
            QueryHandler.$inject = ['$http'];
            return QueryHandler;
        }());
        QueryHandling.QueryHandler = QueryHandler;
    })(QueryHandling = AuctionHouse.QueryHandling || (AuctionHouse.QueryHandling = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=aQueryHandler.js.map
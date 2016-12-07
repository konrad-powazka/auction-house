var AuctionHouse;
(function (AuctionHouse) {
    var QueryHandling;
    (function (QueryHandling) {
        var QueryHandler = (function () {
            function QueryHandler(httpService) {
                this.httpService = httpService;
            }
            QueryHandler.prototype.handle = function (guery) {
                var url = "api/" + this.getQueryName() + "/Handle";
                return this.httpService.get(url);
            };
            QueryHandler.$inject = ['$http'];
            return QueryHandler;
        }());
        QueryHandling.QueryHandler = QueryHandler;
    })(QueryHandling = AuctionHouse.QueryHandling || (AuctionHouse.QueryHandling = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=QueryHandler.js.map
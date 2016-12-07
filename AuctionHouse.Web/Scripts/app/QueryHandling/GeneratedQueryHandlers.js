var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var AuctionHouse;
(function (AuctionHouse) {
    var QueryHandling;
    (function (QueryHandling) {
        var GetAuctionDetailsQueryHandler = (function (_super) {
            __extends(GetAuctionDetailsQueryHandler, _super);
            function GetAuctionDetailsQueryHandler() {
                _super.apply(this, arguments);
            }
            GetAuctionDetailsQueryHandler.prototype.getQueryName = function () {
                return 'GetAuctionDetailsQuery';
            };
            return GetAuctionDetailsQueryHandler;
        }(QueryHandling.QueryHandler));
        QueryHandling.GetAuctionDetailsQueryHandler = GetAuctionDetailsQueryHandler;
        var GetAuctionListQueryHandler = (function (_super) {
            __extends(GetAuctionListQueryHandler, _super);
            function GetAuctionListQueryHandler() {
                _super.apply(this, arguments);
            }
            GetAuctionListQueryHandler.prototype.getQueryName = function () {
                return 'GetAuctionListQuery';
            };
            return GetAuctionListQueryHandler;
        }(QueryHandling.QueryHandler));
        QueryHandling.GetAuctionListQueryHandler = GetAuctionListQueryHandler;
    })(QueryHandling = AuctionHouse.QueryHandling || (AuctionHouse.QueryHandling = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=GeneratedQueryHandlers.js.map
var AuctionHouse;
(function (AuctionHouse) {
    var Auctions;
    (function (Auctions) {
        var CreateAuctionCtrl = (function () {
            function CreateAuctionCtrl($scope) {
                this.$scope = $scope;
                $scope.message = 'Test message';
            }
            return CreateAuctionCtrl;
        }());
        Auctions.CreateAuctionCtrl = CreateAuctionCtrl;
    })(Auctions = AuctionHouse.Auctions || (AuctionHouse.Auctions = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=CreateAuctionCtrl.js.map
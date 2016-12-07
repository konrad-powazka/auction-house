var AuctionHouse;
(function (AuctionHouse) {
    var Auctions;
    (function (Auctions) {
        var CreateAuctionComponent = (function () {
            function CreateAuctionComponent() {
                this.controller = Auctions.CreateAuctionCtrl;
                this.templateUrl = 'Template/Auctions/Create';
                this.registerAs = 'createAuction';
            }
            return CreateAuctionComponent;
        }());
        Auctions.CreateAuctionComponent = CreateAuctionComponent;
    })(Auctions = AuctionHouse.Auctions || (AuctionHouse.Auctions = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=CreateAuctionComponent.js.map
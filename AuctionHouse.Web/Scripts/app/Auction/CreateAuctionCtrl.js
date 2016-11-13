var AuctionHouse;
(function (AuctionHouse) {
    var Auctions;
    (function (Auctions) {
        var CreateAuctionCtrl = (function () {
            function CreateAuctionCtrl($scope) {
                this.$scope = $scope;
                this.model = {};
                this.fields = [
                    {
                        key: 'title',
                        type: 'input',
                        templateOptions: {
                            label: 'Title'
                        }
                    },
                    {
                        key: 'description',
                        type: 'textarea',
                        templateOptions: {
                            label: 'Description'
                        }
                    }];
            }
            return CreateAuctionCtrl;
        }());
        Auctions.CreateAuctionCtrl = CreateAuctionCtrl;
    })(Auctions = AuctionHouse.Auctions || (AuctionHouse.Auctions = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=CreateAuctionCtrl.js.map
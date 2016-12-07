var AuctionHouse;
(function (AuctionHouse) {
    var Auctions;
    (function (Auctions) {
        var CreateAuctionCommand = AuctionHouse.Messages.Commands.Auctions.CreateAuctionCommand;
        var CreateAuctionCtrl = (function () {
            //TODO: inject interface
            function CreateAuctionCtrl(createAuctionCommandHandler) {
                this.createAuctionCommandHandler = createAuctionCommandHandler;
                this.model = new CreateAuctionCommand();
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
            CreateAuctionCtrl.prototype.submit = function () {
                this.createAuctionCommandHandler.handle(this.model);
            };
            CreateAuctionCtrl.$inject = ['CreateAuctionCommandHandler'];
            return CreateAuctionCtrl;
        }());
        Auctions.CreateAuctionCtrl = CreateAuctionCtrl;
    })(Auctions = AuctionHouse.Auctions || (AuctionHouse.Auctions = {}));
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=CreateAuctionCtrl.js.map
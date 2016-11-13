var AuctionHouse;
(function (AuctionHouse) {
    var CreateAuctionCtrl = AuctionHouse.Auctions.CreateAuctionCtrl;
    var ViewAuctionCtrl = AuctionHouse.Auctions.ViewAuctionCtrl;
    var Application = (function () {
        function Application() {
        }
        Application.bootstrap = function () {
            var module = angular.module('auctionHouse', ['formly', 'formlyBootstrap']);
            for (var controllerName in Application.controllers) {
                if (Application.controllers.hasOwnProperty(controllerName)) {
                    var controllerCtor = Application.controllers[controllerName];
                    module.controller(controllerName, controllerCtor);
                }
            }
        };
        ;
        Application.controllers = {
            'CreateAuctionCtrl': CreateAuctionCtrl,
            'ViewAuctionCtrl': ViewAuctionCtrl
        };
        return Application;
    }());
    AuctionHouse.Application = Application;
    Application.bootstrap();
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=Application.js.map
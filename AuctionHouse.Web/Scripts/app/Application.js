var AuctionHouse;
(function (AuctionHouse) {
    var AngularCommandHandlersRegistry = AuctionHouse.CommandHandling.AngularCommandHandlersRegistry;
    var Application = (function () {
        function Application() {
        }
        Application.bootstrap = function () {
            var module = angular.module('auctionHouse', ['ui.router', 'formly', 'formlyBootstrap']);
            module.service(AngularCommandHandlersRegistry.commandHandlers);
            for (var _i = 0, _a = Application.components; _i < _a.length; _i++) {
                var component = _a[_i];
                module.component(component.registerAs, component);
            }
            module.config(Application.configureRouting);
        };
        ;
        Application.configureRouting = function ($stateProvider) {
            var states = [
                {
                    name: 'createAuction',
                    url: '/auction/create',
                    component: 'createAuction'
                }
            ];
            for (var _i = 0, states_1 = states; _i < states_1.length; _i++) {
                var state = states_1[_i];
                $stateProvider.state(state);
            }
        };
        ;
        Application.components = [
            new AuctionHouse.Auctions.CreateAuctionComponent()
        ];
        return Application;
    }());
    AuctionHouse.Application = Application;
    Application.bootstrap();
})(AuctionHouse || (AuctionHouse = {}));
//# sourceMappingURL=Application.js.map
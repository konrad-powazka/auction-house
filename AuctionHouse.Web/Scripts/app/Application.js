"use strict";
var CreateAuctionComponent_1 = require('./UI/Auctions/CreateAuctionComponent');
var GeneratedCommandHandlers_1 = require('./CommandHandling/GeneratedCommandHandlers');
var Application = (function () {
    function Application() {
    }
    Application.bootstrap = function () {
        var module = angular.module('auctionHouse', ['ui.router', 'formly', 'formlyBootstrap']);
        module.service(GeneratedCommandHandlers_1.AngularCommandHandlersRegistry.commandHandlers);
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
        new CreateAuctionComponent_1.CreateAuctionComponent()
    ];
    return Application;
}());
exports.Application = Application;
Application.bootstrap();
//# sourceMappingURL=Application.js.map
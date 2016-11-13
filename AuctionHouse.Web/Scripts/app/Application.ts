namespace AuctionHouse {

    export class Application {
        private static components: Infrastructure.INamedComponentOptions[] = [
            new Auctions.CreateAuctionComponent()
        ];

        static bootstrap(): void {
            const module = angular.module('auctionHouse', ['ui.router', 'formly', 'formlyBootstrap'] as string[]);

            for (let component of Application.components) {
                module.component(component.registerAs, component);
            }

            module.config(Application.configureRouting);
        };

        private static configureRouting($stateProvider: ng.ui.IStateProvider): void {
            const states: ng.ui.IState[] = [
                {
                    name: 'createAuction',
                    //templateUrl: 'Template/Auctions/Create'
                    url: '/auction/create',
                    component: 'createAuction'
                }
            ];

            for (let state of states) {
                $stateProvider.state(state);
            }
        };
    }

    Application.bootstrap();
}
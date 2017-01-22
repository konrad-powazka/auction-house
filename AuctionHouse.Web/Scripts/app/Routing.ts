export class Routing {
    static configure($stateProvider: ng.ui.IStateProvider): void {
        const states: ng.ui.IState[] = [
            {
                name: 'createAuction',
                url: '/auction/create',
                component: 'createAuction'
            },
            {
                name: 'displayAuction',
                url: '/auction/{auctionId}',
                component: 'displayAuction',
                resolve: {
                    auctionId($stateParams: angular.ui.IStateParamsService) {
                        return ($stateParams as any).auctionId;
                    }
                }
            }
        ];

        for (let state of states) {
            $stateProvider.state(state);
        }
    }
}
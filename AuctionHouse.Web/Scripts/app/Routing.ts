export class Routing {
	static configure($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider): void {
		const states: ng.ui.IState[] = [
		{
			name: 'home',
			url: '/',
			component: 'activeAuctionsList'
		},
		{
			name: 'auctionsSearch',
			url: '/auction/search/{queryString}',
			component: 'activeAuctionsList',
			resolve: {
				queryString: [
					'$stateParams', ($stateParams: angular.ui.IStateParamsService) => {
						return $stateParams['queryString'];
					}]
				}
			},
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
					auctionId: ['$stateParams', ($stateParams: angular.ui.IStateParamsService) => $stateParams['auctionId']]
				}
			},
			{
				name: 'userMessages',
				url: '/user-messages',
				component: 'userMessages'
			},
			{
				name: 'userAuctionsList',
				url: '/my-auctions',
				component: 'userAuctionsList'
			}
			
		];

		for (let state of states) {
			$stateProvider.state(state);
		}

		$urlRouterProvider.when('', '/');
	}
}
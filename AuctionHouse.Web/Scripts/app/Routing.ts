export class Routing {
	static configure($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider): void {
		const states: ng.ui.IState[] = [
		{
			name: 'home',
			url: '/',
			component: 'auctionsList'
		},
		{
			name: 'auctionsSearch',
			url: '/auction/search/{queryString}',
			component: 'auctionsList',
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
				url: '/userMessages',
				component: 'userMessages'
			}
		];

		for (let state of states) {
			$stateProvider.state(state);
		}

		$urlRouterProvider.when('', '/');
	}
}
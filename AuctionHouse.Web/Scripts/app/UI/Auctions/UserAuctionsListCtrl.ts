import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { GetAuctionsInvolvingUserQuery } from '../../Messages/Queries';

export class UserAuctionsListCtrl implements ng.IController {
	queryString: string;

	tastyInitCfg = {
		'count': 10,
		'page': 1
	};

	staticResource = {
		header: [
			{
				key: 'title',
				name: 'Title',
				style: { width: '20%' }
			},
			{
				key: 'currentPrice',
				name: 'Current price',
				style: { width: '120px', 'text-align': 'right'  }
			},
			{
				key: 'buyNowPrice',
				name: 'Buy now price',
				style: { width: '120px', 'text-align': 'right' }
			},
			{
				key: 'numberOfBids',
				name: 'Bids',
				style: { width: '50px', 'text-align': 'right' }
			},
			{
				key: 'description',
				name: 'Description',
				style: { width: '' }
			}
		]
	};

	static $inject = ['getAuctionsInvolvingUserQueryHandler'];

	constructor(private getAuctionsInvolvingUserQueryHandler: IQueryHandler<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>) {
	}

	getResource = (paramsString: string, paramsObject: any): ng.IPromise<any> => {
		const query: GetAuctionsInvolvingUserQuery = {
			queryString: '', //this.queryString,
			userInvolvementIntoAuction: null,
			pageSize: paramsObject.count,
			pageNumber: paramsObject.page
		};

		return this.getAuctionsInvolvingUserQueryHandler.handle(query)
			.then(auctionsList => {
				return {
					rows: auctionsList.pageItems,
					pagination: {
						count: auctionsList.pageSize,
						page: auctionsList.pageNumber,
						pages: auctionsList.totalPagesCount,
						size: auctionsList.totalItemsCount
					},
					header: this.staticResource.header
				};
			});
	};
}
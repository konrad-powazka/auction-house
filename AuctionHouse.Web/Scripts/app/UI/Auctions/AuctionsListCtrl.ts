import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { SearchAuctionsQuery } from '../../Messages/Queries';

export class AuctionsListCtrl implements ng.IController {
	queryString: string;
	getAuctions: (pageSize: number, pageNumber: number) => ng.IPromise<AuctionsListReadModel>;

	tastyInitCfg = {
		'count': 25,
		'page': 1
	};

	staticResource = {
		header: [
			{
				key: 'titleAndDescription',
				name: 'Auction',
				style: { width: '' }
			},
			{
				key: 'currentPrice',
				name: 'Current price',
				style: { width: '120px', 'text-align': 'right'  }
			},
			{
				key: 'soldFor',
				name: 'Sold for',
				style: { width: '120px', 'text-align': 'right' }
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
				key: 'seller',
				name: 'Seller',
				style: { width: '200px' }
			},
			{
				key: 'winner',
				name: 'Winner',
				style: { width: '200px' }
			}
		]
	};

	static $inject = [];

	getResource = (paramsString: string, paramsObject: any): ng.IPromise<any> => {
		var pageSize = paramsObject.count;
		var pageNumber = paramsObject.page;

		return this.getAuctions(pageSize, pageNumber)
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
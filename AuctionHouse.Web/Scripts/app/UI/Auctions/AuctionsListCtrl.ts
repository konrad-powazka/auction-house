import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { SearchAuctionsQuery } from '../../Messages/Queries';

export class AuctionsListCtrl implements ng.IController {
	queryString: string;

	tastyInitCfg = {
		'count': 10,
		'page': 1
	};

	staticResource = {
		header: [
			{
				key: 'Title',
				name: 'Title',
				style: { width: '30%' }
			},
			{
				key: 'Description',
				name: 'Description',
				style: { width: '70%' }
			}
		]
	};

	static $inject = ['searchAuctionsQueryHandler'];

	constructor(private searchAuctionsQueryHandler: IQueryHandler<SearchAuctionsQuery, AuctionsListReadModel>) {
	}

	getResource = (paramsString: string, paramsObject: any): ng.IPromise<any> => {
		const query = {
			queryString: this.queryString,
			pageSize: paramsObject.count,
			pageNumber: paramsObject.page
		};

		return this.searchAuctionsQueryHandler.handle(query)
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
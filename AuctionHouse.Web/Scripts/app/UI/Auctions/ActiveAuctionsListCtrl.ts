import { AuctionsListReadModel } from '../../ReadModel';
import { IQueryHandler } from '../../QueryHandling/IQueryHandler';
import { SearchAuctionsQuery } from '../../Messages/Queries';
import { AuctionsListColumn } from './AuctionsListColumn';

export class ActiveAuctionsListCtrl implements ng.IController {
	queryString: string;
	displayedColumns = [AuctionsListColumn.TitleAndDescription, AuctionsListColumn.CurrentPrice, AuctionsListColumn.BuyNowPrice, AuctionsListColumn.NumberOfBids, AuctionsListColumn.Seller];

	static $inject = ['searchAuctionsQueryHandler'];

	constructor(private searchAuctionsQueryHandler: IQueryHandler<SearchAuctionsQuery, AuctionsListReadModel>) {
	}

	getAuctions = (pageSize: number, pageNumber: number): ng.IPromise<AuctionsListReadModel> => {
		const query = {
			queryString: this.queryString,
			pageSize: pageSize,
			pageNumber: pageNumber
		};

		return this.searchAuctionsQueryHandler.handle(query);
	};
}
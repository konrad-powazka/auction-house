import { AuctionsListReadModel, AuctionListItemReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { SearchAuctionsQuery } from '../../Messages/Queries';
import {AuctionsListColumn} from './AuctionsListColumn';
import ListHeaderDefinition from '../Shared/Lists/ListHeaderDefinition';
import {ListCtrl} from '../Shared/Lists/ListCtrl';

export class AuctionsListCtrl extends ListCtrl<AuctionsListColumn, AuctionListItemReadModel> {
	getAuctions: (pageSize: number, pageNumber: number) => ng.IPromise<AuctionsListReadModel>;

	static $inject = ['$scope'];

	constructor(scope: ng.IScope) {
		super(scope);
	}

	protected getAllHeaderDefinitions() {
		return [
			new ListHeaderDefinition<AuctionsListColumn>('TitleAndDescription', 'Auction'),
			new ListHeaderDefinition<AuctionsListColumn>('CurrentPrice', 'Current price', { width: '120px', 'text-align': 'right' }),
			new ListHeaderDefinition<AuctionsListColumn>('SoldFor', 'Sold for', { width: '120px', 'text-align': 'right' }),
			new ListHeaderDefinition<AuctionsListColumn>('BuyNowPrice', 'Buy now price', { width: '120px', 'text-align': 'right' }),
			new ListHeaderDefinition<AuctionsListColumn>('NumberOfBids', 'Bids', { width: '50px', 'text-align': 'right' }),
			new ListHeaderDefinition<AuctionsListColumn>('Seller', 'Seller', { width: '150px' }),
			new ListHeaderDefinition<AuctionsListColumn>('Winner', 'Winner', { width: '150px' }),
			new ListHeaderDefinition<AuctionsListColumn>('StartedDateTime', 'Started', { width: '180px' }),
			new ListHeaderDefinition<AuctionsListColumn>('EndsDateTime', 'Ends in', { width: '130px' }),
			new ListHeaderDefinition<AuctionsListColumn>('EndedDateTime', 'Ended', { width: '180px' }),
		];
	}

	protected getResults(pageSize: number, pageNumber: number) {
		return this.getAuctions(pageSize, pageNumber);
	}
}
import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { SearchAuctionsQuery } from '../../Messages/Queries';
import {AuctionsListColumn} from './AuctionsListColumn';

class HeaderDefinition {
	constructor(public column: AuctionsListColumn, public displayName: string, public style?: any) {
	}

	toTastyTableHeader() {
		return {
			key: AuctionsListColumn[this.column],
			name: this.displayName,
			style: this.style
		}
	}
}

export class AuctionsListCtrl implements ng.IController {
	private static allHeaders = [
		new HeaderDefinition(AuctionsListColumn.TitleAndDescription, 'Auction'),
		new HeaderDefinition(AuctionsListColumn.CurrentPrice, 'Current price', { width: '120px', 'text-align': 'right' }),
		new HeaderDefinition(AuctionsListColumn.SoldFor, 'Sold for', { width: '120px', 'text-align': 'right' }),
		new HeaderDefinition(AuctionsListColumn.BuyNowPrice, 'Buy now price', { width: '120px', 'text-align': 'right' }),
		new HeaderDefinition(AuctionsListColumn.NumberOfBids, 'Bids', { width: '50px', 'text-align': 'right' }),
		new HeaderDefinition(AuctionsListColumn.Seller, 'Seller', { width: '200px' }),
		new HeaderDefinition(AuctionsListColumn.Winner, 'Winner', { width: '200px' })
	];

	queryString: string;
	getAuctions: (pageSize: number, pageNumber: number) => ng.IPromise<AuctionsListReadModel>;
	displayedColumns: AuctionsListColumn[];
	onReloadFunctionChanged: (args: { reloadFunction: () => void }) => void;
	reload: () => void = angular.noop;

	staticResource = {
		header: _(AuctionsListCtrl.allHeaders).map(header => header.toTastyTableHeader())
	};

	tastyInitCfg = {
		'count': 25,
		'page': 1
	};

	static $inject = ['$scope'];

	constructor(scope: ng.IScope) {
		scope.$watch(() => this.reload, () => {
			this.onReloadFunctionChanged({ reloadFunction: this.reload });
			this.reload();
		});

		scope.$watchCollection(() => this.displayedColumns, () => {
			const displayedHeaders = _(AuctionsListCtrl.allHeaders)
				.filter(header => _(this.displayedColumns).contains(header.column));

			this.staticResource.header = _(displayedHeaders).map(header => header.toTastyTableHeader());

		});
	}

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

	checkIfColumnIsDisplayed(columnName: string) {
		const column = (AuctionsListColumn as any)[columnName] as AuctionsListColumn;
		return _(this.displayedColumns).contains(column);
	}
}
import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { GetAuctionsInvolvingUserQuery } from '../../Messages/Queries';
import {AuctionsListColumn} from './AuctionsListColumn';

export class UserAuctionsListCtrl implements ng.IController {
	search: () => void;
	queryString: string;
	userInvolvementIntoAuction = 'Selling';
	displayedColumns = [AuctionsListColumn.TitleAndDescription, AuctionsListColumn.Winner];

	static $inject = ['getAuctionsInvolvingUserQueryHandler'];

	constructor(private getAuctionsInvolvingUserQueryHandler:
		IQueryHandler<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>) {
	}

	getAuctions = (pageSize: number, pageNumber: number): ng.IPromise<AuctionsListReadModel> => {
		this.refreshDisplayedColumns();

		const query: GetAuctionsInvolvingUserQuery = {
			queryString: this.queryString,
			userInvolvementIntoAuction: this.userInvolvementIntoAuction,
			pageSize: pageSize,
			pageNumber: pageNumber
		};

		return this.getAuctionsInvolvingUserQueryHandler.handle(query);
	};

	setReloadFunction(reloadFn: () => void) {
		this.search = reloadFn;
	}

	private refreshDisplayedColumns() {
		const commonColumns = [AuctionsListColumn.TitleAndDescription, AuctionsListColumn.BuyNowPrice];

		// TODO: add ended date and final price columns
		const userInvolvementIntoAuctionToAdditionalColumnsMap: { [userInvolvementIntoAuction: string]: AuctionsListColumn[] } = {
			'Selling': [AuctionsListColumn.CurrentPrice, AuctionsListColumn.BuyNowPrice, AuctionsListColumn.NumberOfBids],
			'Sold': [AuctionsListColumn.SoldFor, AuctionsListColumn.Winner, AuctionsListColumn.BuyNowPrice, AuctionsListColumn.NumberOfBids],
			'FailedToSell': [] as AuctionsListColumn[],
			'Bidding': [AuctionsListColumn.Seller, AuctionsListColumn.CurrentPrice, AuctionsListColumn.NumberOfBids],
			'Bought': [AuctionsListColumn.SoldFor, AuctionsListColumn.Seller, AuctionsListColumn.NumberOfBids],
			'FailedToBuy': [AuctionsListColumn.SoldFor, AuctionsListColumn.Seller, AuctionsListColumn.Winner, AuctionsListColumn.NumberOfBids]
		}

		this.displayedColumns = commonColumns
			.concat(userInvolvementIntoAuctionToAdditionalColumnsMap[this.userInvolvementIntoAuction]);
	}
}
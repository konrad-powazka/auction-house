﻿import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { GetAuctionsInvolvingUserQuery } from '../../Messages/Queries';
import {AuctionsListColumn} from './AuctionsListColumn';

export class UserAuctionsListCtrl implements ng.IController {
	search: () => void;
	queryString: string;
	userInvolvementIntoAuction = 'Selling';
	displayedColumns: AuctionsListColumn[];

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
		const commonColumns: AuctionsListColumn[] = ['TitleAndDescription', 'BuyNowPrice'];

		// TODO: add ended date and final price columns
		const userInvolvementIntoAuctionToAdditionalColumnsMap: { [userInvolvementIntoAuction: string]: AuctionsListColumn[] } = {
			'Selling': ['CurrentPrice', 'BuyNowPrice', 'NumberOfBids', 'EndsDateTime'],
			'Sold': ['SoldFor', 'Winner', 'BuyNowPrice', 'NumberOfBids', 'EndedDateTime'],
			'FailedToSell': ['EndedDateTime'],
			'Bidding': ['Seller', 'CurrentPrice', 'NumberOfBids', 'EndsDateTime'],
			'Bought': ['SoldFor', 'Seller', 'NumberOfBids', 'EndedDateTime'],
			'FailedToBuy': ['SoldFor', 'Seller', 'Winner', 'NumberOfBids', 'EndedDateTime']
		}

		this.displayedColumns = commonColumns
			.concat(userInvolvementIntoAuctionToAdditionalColumnsMap[this.userInvolvementIntoAuction]);
	}
}
import { AuctionsListReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { GetAuctionsInvolvingUserQuery } from '../../Messages/Queries';

export class UserAuctionsListCtrl implements ng.IController {
	queryString: string;
	userInvolvementIntoAuction = 'Selling';

	static $inject = ['getAuctionsInvolvingUserQueryHandler'];

	constructor(private getAuctionsInvolvingUserQueryHandler: IQueryHandler<GetAuctionsInvolvingUserQuery, AuctionsListReadModel>) {
	}

	getAuctions = (pageSize: number, pageNumber: number): ng.IPromise<AuctionsListReadModel> => {
		const query: GetAuctionsInvolvingUserQuery = {
			queryString: this.queryString,
			userInvolvementIntoAuction: this.userInvolvementIntoAuction,
			pageSize: pageSize,
			pageNumber: pageNumber
		};

		return this.getAuctionsInvolvingUserQueryHandler.handle(query);
	};
}
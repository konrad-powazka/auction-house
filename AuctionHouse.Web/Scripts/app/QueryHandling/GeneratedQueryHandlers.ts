
import { QueryHandler } from './QueryHandler';
import * as Queries from '../Messages/Queries';
import * as ReadModel from '../ReadModel';

export class GetUserInboxQueryHandler extends QueryHandler<Queries.GetUserInboxQuery, ReadModel.UserInboxReadModel> {
	protected getQueryName(): string {
		return 'GetUserInboxQuery';
	}
}
export class GetAuctionDetailsQueryHandler extends QueryHandler<Queries.GetAuctionDetailsQuery, ReadModel.AuctionDetailsReadModel> {
	protected getQueryName(): string {
		return 'GetAuctionDetailsQuery';
	}
}
export class GetAuctionsInvolvingUserQueryHandler extends QueryHandler<Queries.GetAuctionsInvolvingUserQuery, ReadModel.AuctionsListReadModel> {
	protected getQueryName(): string {
		return 'GetAuctionsInvolvingUserQuery';
	}
}
export class SearchAuctionsQueryHandler extends QueryHandler<Queries.SearchAuctionsQuery, ReadModel.AuctionsListReadModel> {
	protected getQueryName(): string {
		return 'SearchAuctionsQuery';
	}
}

export class AngularQueryHandlersRegistry {
	static queryHandlers: {[name: string]: ng.Injectable<Function>} = {
							'getUserInboxQueryHandler': GetUserInboxQueryHandler,
							'getAuctionDetailsQueryHandler': GetAuctionDetailsQueryHandler,
							'getAuctionsInvolvingUserQueryHandler': GetAuctionsInvolvingUserQueryHandler,
							'searchAuctionsQueryHandler': SearchAuctionsQueryHandler,
					};
	}
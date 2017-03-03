
import { QueryHandler } from './QueryHandler';
import * as Queries from '../Messages/Queries';
import * as ReadModel from '../ReadModel';

export class GetAuctionDetailsQueryHandler extends QueryHandler<Queries.GetAuctionDetailsQuery, ReadModel.AuctionDetailsReadModel> {
	protected getQueryName(): string {
		return 'GetAuctionDetailsQuery';
	}
}
export class SearchAuctionsQueryHandler extends QueryHandler<Queries.SearchAuctionsQuery, ReadModel.AuctionsListReadModel> {
	protected getQueryName(): string {
		return 'SearchAuctionsQuery';
	}
}

export class AngularQueryHandlersRegistry {
	static queryHandlers: {[name: string]: ng.Injectable<Function>} = {
							'getAuctionDetailsQueryHandler': GetAuctionDetailsQueryHandler,
							'searchAuctionsQueryHandler': SearchAuctionsQueryHandler,
					};
	}
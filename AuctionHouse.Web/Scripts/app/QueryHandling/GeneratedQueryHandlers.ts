
import { QueryHandler } from './QueryHandler';
import * as Queries from '../Messages/Queries';
import * as ReadModel from '../ReadModel';

export class GetAuctionDetailsQueryHandler extends QueryHandler<Queries.GetAuctionDetailsQuery, ReadModel.AuctionDetailsReadModel> {
	protected getQueryName(): string {
		return 'GetAuctionDetailsQuery';
	}
}
export class GetAuctionListQueryHandler extends QueryHandler<Queries.GetAuctionListQuery, ReadModel.AuctionListReadModel> {
	protected getQueryName(): string {
		return 'GetAuctionListQuery';
	}
}

export class AngularQueryHandlersRegistry {
	static queryHandlers: {[name: string]: ng.Injectable<Function>} = {
							'getAuctionDetailsQueryHandler': GetAuctionDetailsQueryHandler,
							'getAuctionListQueryHandler': GetAuctionListQueryHandler,
					};
	}
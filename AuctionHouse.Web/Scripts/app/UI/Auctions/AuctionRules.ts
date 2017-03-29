import { AuctionListItemReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { SearchAuctionsQuery } from '../../Messages/Queries';

export class AuctionRules {
	static checkIfAuctionShouldBeDisplayedAsFinished(auction: AuctionListItemReadModel): boolean {
		return false;
		//return auction.wasFinished || auction.
	}
}
import { ActiveAuctionsListCtrl } from './ActiveAuctionsListCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class ActiveAuctionsListComponent implements INamedComponentOptions {
	controller = ActiveAuctionsListCtrl;
    templateUrl = 'Template/Auctions/ActiveList';
	registerAs = 'activeAuctionsList';
	bindings = {
		queryString: '<'
    }
}
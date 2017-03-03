import { AuctionsListCtrl } from './AuctionsListCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class AuctionsListComponent implements INamedComponentOptions {
	controller = AuctionsListCtrl;
    templateUrl = 'Template/Auctions/List';
    registerAs = 'auctionsList';
    bindings = {
    }
}
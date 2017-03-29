import { UserAuctionsListCtrl } from './UserAuctionsListCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class UserAuctionsListComponent implements INamedComponentOptions {
	controller = UserAuctionsListCtrl;
    templateUrl = 'Template/Auctions/UserList';
    registerAs = 'userAuctionsList';
	//bindings = {
	//	queryString: '<'
 //   }
}
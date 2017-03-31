import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import {UserMessagesListCtrl} from './UserMessagesListCtrl';

export class UserMessagesListComponent implements INamedComponentOptions {
	controller = UserMessagesListCtrl;
	templateUrl = 'Template/UserMessaging/UserMessagesList';
	registerAs = 'userMessagesList';
	bindings = {
		getMessages: '<',
		displayedColumns: '<'
	}
}
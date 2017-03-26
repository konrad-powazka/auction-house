import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import {UserMessagesCtrl} from './UserMessagesCtrl';

export class UserMessagesComponent implements INamedComponentOptions {
	controller = UserMessagesCtrl;
	templateUrl = 'Template/UserMessaging/UserMessages';
	registerAs = 'userMessages';
}
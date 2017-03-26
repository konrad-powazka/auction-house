import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import { UserReferenceCtrl } from './UserReferenceCtrl';

export class UserReferenceComponent implements INamedComponentOptions {
	controller = UserReferenceCtrl;
	templateUrl = 'Template/Shared/UserReference';
	registerAs = 'userReference';
    bindings = {
		userName: '<'
    }
}
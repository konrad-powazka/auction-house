import { DisplayUserMessageDialogCtrl } from './DisplayUserMessageDialogCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class DisplayUserMessageDialogComponent implements INamedComponentOptions {
	controller = DisplayUserMessageDialogCtrl;
	templateUrl = 'Template/UserMessaging/DisplayMessageDialog';
	registerAs = 'displayUserMessageDialog';
	bindings = {
		modalInstance: '<',
		resolve: '<'
	}
}
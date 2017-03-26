import { ComposeUserMessageDialogCtrl } from './ComposeUserMessageDialogCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class ComposeUserMessageDialogComponent implements INamedComponentOptions {
	controller = ComposeUserMessageDialogCtrl;
	templateUrl = 'Template/UserMessaging/ComposeMessageDialog';
	registerAs = 'composeUserMessageDialog';
	bindings = {
		modalInstance: '<',
		resolve: '<'
	}
}
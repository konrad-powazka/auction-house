import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import {SimpleNotificationDialogCtrl} from './SimpleNotificationDialogCtrl';

export class SimpleNotificationDialogComponent implements INamedComponentOptions {
	controller = SimpleNotificationDialogCtrl;
	templateUrl = 'Template/Shared/SimpleNotificationDialog';
	registerAs = 'simpleNotificationDialog';
    bindings = {
		modalInstance: '<',
		resolve: '<'
    }
}
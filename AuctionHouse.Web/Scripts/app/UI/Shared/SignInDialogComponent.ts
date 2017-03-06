import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import { SignInDialogCtrl } from './SignInDialogCtrl';

export class SignInDialogComponent implements INamedComponentOptions {
	controller = SignInDialogCtrl;
	templateUrl = 'Template/Security/SignInDialog';
    registerAs = 'signInDialog';
    bindings = {
        modalInstance: '<'
    }
}
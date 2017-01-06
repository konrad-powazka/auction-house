import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';
import {LoginDialogCtrl} from './LoginDialogCtrl';

export class LoginDialogComponent implements INamedComponentOptions {
    controller = LoginDialogCtrl;
    templateUrl = 'Template/Security/LoginDialog';
    registerAs = 'loginDialog';
    bindings = {
        modalInstance: '<'
    }
}
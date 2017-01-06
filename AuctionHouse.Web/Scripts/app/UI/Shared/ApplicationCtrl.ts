import {SecurityUiService} from './SecurityUiService';

export class ApplicationCtrl implements ng.IController {
    static $inject = ['securityUiService'];

    constructor(private securityUiService: SecurityUiService) {
    }

    login(): void {
        this.securityUiService.showLoginDialog();
    }
}
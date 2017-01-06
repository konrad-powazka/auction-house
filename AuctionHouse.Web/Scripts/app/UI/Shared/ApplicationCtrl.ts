import {SecurityUiService} from './SecurityUiService';

export class ApplicationCtrl implements ng.IController {
    static $inject = ['securityUiService'];

    constructor(public securityUiService: SecurityUiService) {
    }
}
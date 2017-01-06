import {SecurityService} from '../../Security/SecurityService';
import ModalService = angular.ui.bootstrap.IModalService;

export class SecurityUiService {
    static $inject = ['securityService', '$uibModal', '$q'];

    constructor(private securityService: SecurityService,
        private modalService: ModalService,
        private qService: ng.IQService) {
    }

    get currentUserName(): string | null {
        return this.securityService.checkIfUserIsAuthenticated() ? this.securityService.getCurrentUserName() : null;
    }

    ensureUserIsAuthenticated(): ng.IPromise<void> {
        if (this.securityService.checkIfUserIsAuthenticated()) {
            return this.qService.resolve();
        }

        return this.showLoginDialog();
    }

    showLoginDialog(): ng.IPromise<void> {
        const modalInstance = this.modalService.open({
            component: 'loginDialog'
        });

        return modalInstance.result;
    }
}
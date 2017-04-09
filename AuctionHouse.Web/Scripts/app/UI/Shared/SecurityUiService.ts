import {SecurityService} from '../../Security/SecurityService';
import ModalService = angular.ui.bootstrap.IModalService;

export class SecurityUiService {
	static $inject = ['securityService', '$uibModal', '$q', '$state'];

    constructor(private securityService: SecurityService,
        private modalService: ModalService,
		private qService: ng.IQService,
		private stateService: ng.ui.IStateService
	) {
    }

    get currentUserName(): string | null {
        return this.securityService.checkIfUserIsAuthenticated() ? this.securityService.getCurrentUserName() : null;
    }

    get isUserAuthenticated(): boolean {
        return this.securityService.checkIfUserIsAuthenticated();
    }

    ensureUserIsAuthenticated(): ng.IPromise<void> {
        if (this.securityService.checkIfUserIsAuthenticated()) {
            return this.qService.resolve();
        }

        return this.showSignInDialog();
    }

    showSignInDialog(): ng.IPromise<void> {
        const modalInstance = this.modalService.open({
            component: 'signInDialog'
        });

        return modalInstance.result;
    }

    signOut(): ng.IPromise<void> {
	    return this.securityService.signOut()
		    .then(() => {
			    this.stateService.go('home');
		    });
    }
}
import ModalServiceInstance = angular.ui.bootstrap.IModalServiceInstance;
import ModalService = angular.ui.bootstrap.IModalService;
import { UserMessageReadModel } from '../../ReadModel';
import { SecurityUiService } from '../Shared/SecurityUiService';

export class DisplayUserMessageDialogCtrl implements ng.IController {
	static $inject = ['$uibModal', 'securityUiService'];

	constructor(private modalService: ModalService, public securityUiService: SecurityUiService) {
	}

	resolve: {
		userMessage: UserMessageReadModel;
	}

	modalInstance: ModalServiceInstance;

	get messageIsFromCurrentUser() {
		return this.securityUiService.currentUserName === this.resolve.userMessage.senderUserName;
	}

	get messageIsToCurrentUser() {
		return this.securityUiService.currentUserName === this.resolve.userMessage.recipientUserName;
	}

	reply() {
		this.modalService.open({
			component: 'composeUserMessageDialog',
			resolve: {
				recipientUserName: () => this.resolve.userMessage.senderUserName,
				messageSubject: () => `RE: ${this.resolve.userMessage.subject}`
			}
		});
	}

	close(): void {
		this.modalInstance.dismiss();
	}
}
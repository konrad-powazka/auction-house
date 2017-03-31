import ModalService = angular.ui.bootstrap.IModalService;
import {SecurityUiService} from './SecurityUiService';

export class UserReferenceCtrl implements ng.IController {
	userName: string;

	static $inject = ['$uibModal', 'securityUiService'];

	constructor(private modalService: ModalService, private securityUiService: SecurityUiService) {
	}

	opendMessageCompositionDialog() {
		this.modalService.open({
			component: 'composeUserMessageDialog',
			resolve: {
				recipientUserName: () => this.userName
			}
		});
	}

	checkIfUserIsCurrentUser() {
		return this.userName === this.securityUiService.currentUserName;
	}
}
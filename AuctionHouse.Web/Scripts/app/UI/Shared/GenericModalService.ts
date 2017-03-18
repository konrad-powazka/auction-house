import ModalService = angular.ui.bootstrap.IModalService;
import { NotificationType } from './NotificationType';

export default class GenericModalService {
	static $inject = ['$uibModal'];

	constructor(private modalService: ModalService) {
	}

	showInformationNotification(notificationMessage: string): ng.IPromise<void> {
		return this.showNotification(notificationMessage, NotificationType.Information);
	}

	showSuccessNotification(notificationMessage: string): ng.IPromise<void> {
		return this.showNotification(notificationMessage, NotificationType.Success);
	}

	showErrorNotification(notificationMessage: string): ng.IPromise<void> {
		return this.showNotification(notificationMessage, NotificationType.Error);
	}

	showNotification(notificationMessage: string, notificationType: NotificationType): ng.IPromise<void> {
		const modalInstance = this.modalService.open({
			component: 'simpleNotificationDialog',
			resolve: {
				notificationMessage: () => notificationMessage,
				notificationType: () => notificationType
			}
		});

		return modalInstance.result;
	}
}
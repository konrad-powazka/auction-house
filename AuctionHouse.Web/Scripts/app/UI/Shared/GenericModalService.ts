import ModalService = angular.ui.bootstrap.IModalService;
import { NotificationType } from './NotificationType';

export default class GenericModalService {
    static $inject = ['$uibModal'];

    constructor(private modalService: ModalService) {
    }

    showErrorNotification(notificationMessage: string): ng.IPromise<void> {
		const modalInstance = this.modalService.open({
			component: 'simpleNotificationDialog',
			resolve: {
				notificationMessage: () => notificationMessage,
				notificationType: () => NotificationType.Error
			}
		});

	    return modalInstance.result;
    }
}
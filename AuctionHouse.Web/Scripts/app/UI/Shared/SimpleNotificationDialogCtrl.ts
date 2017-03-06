import {SecurityService} from '../../Security/SecurityService';
import ModalServiceInstance = angular.ui.bootstrap.IModalServiceInstance;
import {NotificationType} from './NotificationType';

export class SimpleNotificationDialogCtrl implements ng.IController {
	modalInstance: ModalServiceInstance;

	resolve: {
		notificationMessage: string;
		notificationType: NotificationType;
	}

	get isErrorNotification(): boolean {
		return this.resolve.notificationType === NotificationType.Error;
	}
}
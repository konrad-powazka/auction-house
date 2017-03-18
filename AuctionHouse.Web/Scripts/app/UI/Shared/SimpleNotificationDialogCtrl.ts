import {SecurityService} from '../../Security/SecurityService';
import ModalServiceInstance = angular.ui.bootstrap.IModalServiceInstance;
import {NotificationType} from './NotificationType';

export class SimpleNotificationDialogCtrl implements ng.IController {
	modalInstance: ModalServiceInstance;

	resolve: {
		notificationMessage: string;
		notificationType: NotificationType;
	}

	get isInformationNotification(): boolean {
		return this.resolve.notificationType === NotificationType.Information;
	}

	get isSuccessNotification(): boolean {
		return this.resolve.notificationType === NotificationType.Success;
	}

	get isErrorNotification(): boolean {
		return this.resolve.notificationType === NotificationType.Error;
	}
}
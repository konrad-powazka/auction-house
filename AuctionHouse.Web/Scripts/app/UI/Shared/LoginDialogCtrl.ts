import {SecurityService} from '../../Security/SecurityService';
import ModalServiceInstance = angular.ui.bootstrap.IModalServiceInstance;

export class LoginDialogCtrl implements ng.IController {
    modalInstance: ModalServiceInstance;

    fields: AngularFormly.IFieldArray;

    model: {
        userName: string,
        password: string
    };

    form: ng.IFormController;

    static $inject = ['securityService'];

    constructor(private securityService: SecurityService) {
        this.fields = [
            {
                key: 'userName',
                type: 'input',
                templateOptions: {
                    label: 'User name',
                    required: true
                }
            },
            {
                key: 'password',
                type: 'input',
                templateOptions: {
                    type: 'password',
                    label: 'Password',
                    required: true
                }
            }
        ];
    }

    login(): void {
        if (!this.form.$valid) {
            return;
        }

        this.securityService
            .logIn(this.model.userName, this.model.password)
            .then(() => {
                this.modalInstance.close();
            },
            () => {
                // TODO: create generic notification dialogs
                alert('error');
            });
    }

    cancel(): void {
        this.modalInstance.dismiss();
    }
}
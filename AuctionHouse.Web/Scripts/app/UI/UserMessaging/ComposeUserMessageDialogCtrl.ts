import ModalServiceInstance = angular.ui.bootstrap.IModalServiceInstance;
import { SendUserMessageCommand } from '../../Messages/Commands';
import GuidGenerator from '../../Infrastructure/GuidGenerator';
import { ICommandUiHandler } from '../Shared/CommandHandling/ICommandUiHandler';
import GenericModalService from '../Shared/GenericModalService';

export class ComposeUserMessageDialogCtrl implements ng.IController {
	resolve: {
		recipientUserName: string;
		messageSubject: string;
	}

	modalInstance: ModalServiceInstance;
	fields: AngularFormly.IFieldArray;
	sendUserMessageCommandId = GuidGenerator.generateGuid();

	model: SendUserMessageCommand = {
		recipientUserName: '',
		messageSubject: '',
		messageBody: ''
	};

	form: ng.IFormController;

	static $inject = ['sendUserMessageCommandUiHandler', 'genericModalService'];

	constructor(
		private sendUserMessageCommandUiHandler: ICommandUiHandler<SendUserMessageCommand>,
		private genericModalService: GenericModalService) {
		this.model.recipientUserName = this.resolve.recipientUserName;
		this.model.messageSubject = this.resolve.messageSubject || '';

		this.fields = [
			{
				key: 'messageSubject',
				type: 'input',
				templateOptions: {
					label: 'Subject',
					required: true
				}
			},
			{
				key: 'messageBody',
				type: 'textarea',
				templateOptions: {
					label: 'Message',
					required: true,
					maxlength: 10000,
					rows: 8
				}
			},
		];
	}

	sendMessage() {
		if (!this.form.$valid) {
			return;
		}


		this.sendUserMessageCommandUiHandler.handle(this.model, this.sendUserMessageCommandId, false)
			.then(() => {
				this.modalInstance.close();
				this.genericModalService.showSuccessNotification('Your message was sent successfully.');
			});
	}

	cancel(): void {
		this.modalInstance.dismiss();
	}
}
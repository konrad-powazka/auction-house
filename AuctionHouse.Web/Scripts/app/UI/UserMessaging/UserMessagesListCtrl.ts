import { UserMessageReadModel } from '../../ReadModel';
import { IQueryHandler } from '../../QueryHandling/IQueryHandler';
import { ListCtrl } from '../Shared/Lists/ListCtrl';
import { UserMessagesListColumn } from './UserMessagesListColumn';
import ListHeaderDefinition from '../Shared/Lists/ListHeaderDefinition';
import { IPagedResult } from '../Shared/Lists/IPagedResult';
import ModalService = angular.ui.bootstrap.IModalService;

export class UserMessagesListCtrl extends ListCtrl<UserMessagesListColumn, UserMessageReadModel> {
	getMessages: (pageSize: number, pageNumber: number) => ng.IPromise<IPagedResult<UserMessageReadModel>>;

	static $inject = ['$scope', '$uibModal'];

	constructor(private scope: ng.IScope, private modalService: ModalService) {
		super(scope);
	}

	protected getAllHeaderDefinitions(): ListHeaderDefinition<UserMessagesListColumn>[] {
		return [
			new ListHeaderDefinition<UserMessagesListColumn>('SenderUserName', 'From', { width: '200px' }),
			new ListHeaderDefinition<UserMessagesListColumn>('RecipientUserName', 'To', { width: '200px' }),
			new ListHeaderDefinition<UserMessagesListColumn>('SubjectAndBody', 'Message'),
			new ListHeaderDefinition<UserMessagesListColumn>('SentDateTime', 'Sent', { width: '180px' })
		];
	}

	protected getResults(pageSize: number, pageNumber: number) {
		return this.getMessages(pageSize, pageNumber);
	}

	displayUserMessage(userMessage: UserMessageReadModel) {
		this.modalService.open({
			component: 'displayUserMessageDialog',
			resolve: {
				userMessage: () => userMessage
			}
		});
	}
}
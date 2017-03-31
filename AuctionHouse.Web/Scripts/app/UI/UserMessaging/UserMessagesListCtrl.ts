import { UserMessageReadModel } from '../../ReadModel';
import { IQueryHandler } from '../../QueryHandling/IQueryHandler';
import { ListCtrl } from '../Shared/Lists/ListCtrl';
import { UserMessagesListColumn } from './UserMessagesListColumn';
import ListHeaderDefinition from '../Shared/Lists/ListHeaderDefinition';
import { IPagedResult } from '../Shared/Lists/IPagedResult';

export class UserMessagesListCtrl extends ListCtrl<UserMessagesListColumn, UserMessageReadModel> {
	getMessages: (pageSize: number, pageNumber: number) => ng.IPromise<IPagedResult<UserMessageReadModel>>;

	static $inject = ['$scope'];

	constructor(scope: ng.IScope) {
		super(scope);
	}

	protected getAllHeaderDefinitions(): ListHeaderDefinition<UserMessagesListColumn>[] {
		return [
			new ListHeaderDefinition<UserMessagesListColumn>('SenderUserName', 'Sender', { width: '200px' }),
			new ListHeaderDefinition<UserMessagesListColumn>('RecipientUserName', 'Recipient', { width: '200px' }),
			new ListHeaderDefinition<UserMessagesListColumn>('SubjectAndBody', 'Message'),
			new ListHeaderDefinition<UserMessagesListColumn>('SentDateTime', 'Sent', { width: '250px' })
		];
	}

	protected getResults(pageSize: number, pageNumber: number) {
		return this.getMessages(pageSize, pageNumber);
	}
}
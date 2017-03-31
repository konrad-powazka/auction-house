import { UserInboxReadModel, UserSentMessagesReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { GetUserInboxQuery, GetSentUserMessagesQuery } from '../../Messages/Queries';
import { UserMessagesListColumn } from './UserMessagesListColumn';

export class UserMessagesCtrl implements ng.IController {
	inboxMessagesListDisplayedColumns: UserMessagesListColumn[] =
	['SenderUserName', 'SubjectAndBody', 'SentDateTime'];

	sentMessagesListDisplayedColumns: UserMessagesListColumn[] =
	['SubjectAndBody', 'SentDateTime', 'RecipientUserName'];

	static $inject = ['getUserInboxQueryHandler', 'getSentUserMessagesQueryHandler'];

	constructor(private getUserInboxQueryHandler: IQueryHandler<GetUserInboxQuery, UserInboxReadModel>,
		private getSentUserMessagesQueryHandler: IQueryHandler<GetSentUserMessagesQuery, UserSentMessagesReadModel>) {
	}

	getUserInbox = (pageSize: number, pageNumber: number): ng.IPromise<UserInboxReadModel> => {
		const query: GetUserInboxQuery = {
			pageSize: pageSize,
			pageNumber: pageNumber
		};

		return this.getUserInboxQueryHandler.handle(query);
	};

	getSentUserMessages = (pageSize: number, pageNumber: number): ng.IPromise<UserInboxReadModel> => {
		const query: GetSentUserMessagesQuery = {
			pageSize: pageSize,
			pageNumber: pageNumber
		};

		return this.getSentUserMessagesQueryHandler.handle(query);
	};
}
import { UserInboxReadModel } from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import { GetUserInboxQuery } from '../../Messages/Queries';
import {SecurityUiService} from '../Shared/SecurityUiService';

export class UserMessagesCtrl implements ng.IController {
	tastyInitCfg = {
		'count': 10,
		'page': 1
	};

	staticResource = {
		header: [
			{
				key: 'sender',
				name: 'Sender',
				style: { width: '15%' }
			},
			{
				key: 'message',
				name: 'Message',
				style: { width: '60%' }
			},
			{
				key: 'sentDateTime',
				name: 'Sent',
				style: { width: '25%' }
			}
		]
	};

	static $inject = ['getUserInboxQueryHandler'];

	constructor(private getUserInboxQueryHandler: IQueryHandler<GetUserInboxQuery, UserInboxReadModel>, private securityUiService: SecurityUiService) {
	}

	getResource = (paramsString: string, paramsObject: any): ng.IPromise<any> => {
		const query: GetUserInboxQuery = {
			pageSize: paramsObject.count,
			pageNumber: paramsObject.page
		};

		return this.getUserInboxQueryHandler.handle(query)
			.then(userInbox => {
				return {
					rows: userInbox.pageItems,
					pagination: {
						count: userInbox.pageSize,
						page: userInbox.pageNumber,
						pages: userInbox.totalPagesCount,
						size: userInbox.totalItemsCount
					},
					header: this.staticResource.header
				};
			});
	};
}
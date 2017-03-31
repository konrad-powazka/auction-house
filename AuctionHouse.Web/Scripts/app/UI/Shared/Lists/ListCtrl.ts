import {IQueryHandler} from '../../../QueryHandling/IQueryHandler';
import ListHeaderDefinition from './ListHeaderDefinition';
import {IPagedResult as PagedResult} from './IPagedResult';

export abstract class ListCtrl<TColumn extends string, TListItem> implements ng.IController {
	displayedColumns: TColumn[];
	onReloadFunctionChanged: (args: { reloadFunction: () => void }) => void;
	reload: () => void = angular.noop;

	tastyInitCfg = {
		'count': 25,
		'page': 1
	};

	static $inject = ['$scope'];

	constructor(scope: ng.IScope) {
		scope.$watch(
		() => this.reload,
		() => {
			if (this.onReloadFunctionChanged) {
				this.onReloadFunctionChanged({ reloadFunction: this.reload });
			}

			this.reload();
		});
	}

	getResource = (paramsString: string, paramsObject: any): ng.IPromise<any> => {
		var pageSize = paramsObject.count;
		var pageNumber = paramsObject.page;

		return this.getResults(pageSize, pageNumber)
			.then(pagedResults => {
				const displayedHeaders = _(this.getAllHeaderDefinitions())
					.filter(headerDefinition => _(this.displayedColumns).contains(headerDefinition.column));

				const tastyHeader = _(displayedHeaders).map(header => this.mapHeaderDefinitionToTastyTableHeader(header));

				return {
					rows: pagedResults.pageItems,
					pagination: {
						count: pagedResults.pageSize,
						page: pagedResults.pageNumber,
						pages: pagedResults.totalPagesCount,
						size: pagedResults.totalItemsCount
					},
					header: tastyHeader
				};
			});
	};

	checkIfColumnIsDisplayed(column: TColumn) {
		return _(this.displayedColumns).contains(column);
	}

	protected abstract getAllHeaderDefinitions(): ListHeaderDefinition<TColumn>[];

	protected abstract getResults(pageSize: number, pageNumber: number): ng.IPromise<PagedResult<TListItem>>;

	private mapHeaderDefinitionToTastyTableHeader(listHeaderDefinition: ListHeaderDefinition<TColumn>) {
		return {
			key: listHeaderDefinition.column,
			name: listHeaderDefinition.displayName,
			style: listHeaderDefinition.style
		};
	}
}
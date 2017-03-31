export interface IPagedResult<TItem> {
	pageNumber: number;
	totalPagesCount: number;
	totalItemsCount: number;
	pageItems: TItem[];
	pageSize: number;
}
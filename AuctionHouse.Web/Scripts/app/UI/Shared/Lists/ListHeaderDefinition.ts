export default class ListHeaderDefinition<TColumn> {
	constructor(public column: TColumn, public displayName: string, public style?: any) {
	}
}
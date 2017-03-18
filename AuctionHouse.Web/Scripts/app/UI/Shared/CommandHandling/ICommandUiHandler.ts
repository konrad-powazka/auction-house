export interface ICommandUiHandler<TCommand> {
	handle(command: TCommand, commandId: string, shouldWaitForEventsApplicationToReadModel: boolean): ng.IPromise<void>;
}
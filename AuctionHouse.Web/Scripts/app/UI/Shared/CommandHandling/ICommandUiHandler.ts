export interface ICommandUiHandler<TCommand> {
    handle(command: TCommand, shouldWaitForEventsApplicationToReadModel: boolean): ng.IPromise<void>;
}
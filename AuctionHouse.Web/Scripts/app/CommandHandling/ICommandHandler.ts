export interface ICommandHandler<TCommand> {
    // TODO: Add an enum to define asynchronity level (Full, WaitForHandling, WaitForHandlingAndReadModelChange
	handle(command: TCommand, commandId: string, shouldWaitForEventsApplicationToReadModel: boolean): ng.IPromise<void>;
}
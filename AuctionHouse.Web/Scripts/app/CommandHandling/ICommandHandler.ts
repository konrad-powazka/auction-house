export interface ICommandHandler<TCommand> {
    handle(command: TCommand): ng.IPromise<void>;
}
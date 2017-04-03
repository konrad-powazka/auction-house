import { CommandHandlingAsynchronityLevel } from '../../../CommandHandling/CommandHandlingAsynchronityLevel';

export interface ICommandUiHandler<TCommand> {
	handle(command: TCommand, commandId: string, asynchronityLevel: CommandHandlingAsynchronityLevel): ng.IPromise<void>;
}
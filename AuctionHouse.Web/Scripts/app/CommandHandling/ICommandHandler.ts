import {CommandHandlingAsynchronityLevel} from './CommandHandlingAsynchronityLevel';

export interface ICommandHandler<TCommand> {
	handle(command: TCommand, commandId: string, asynchronityLevel: CommandHandlingAsynchronityLevel): ng.IPromise<void>;
}
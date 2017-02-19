import { ICommandUiHandler } from './ICommandUiHandler';
import BusyIndicator from '../BusyIndicator';
import {SecurityUiService } from '../SecurityUiService';
import {ICommandHandler } from '../../../CommandHandling/ICommandHandler';

export abstract class CommandUiHandler<TCommand> implements ICommandUiHandler<TCommand> {
    constructor(private busyIndicator: BusyIndicator, private securityUiService: SecurityUiService) {
    }

    protected abstract getCommandHandler(): ICommandHandler<TCommand>;

    handle(command: TCommand, shouldWaitForEventsApplicationToReadModel: boolean): angular.IPromise<void> {
        // TODO: Authorization and authentication
        const promise = this.getCommandHandler().handle(command, shouldWaitForEventsApplicationToReadModel);
        return this.busyIndicator.attachToPromise(promise);
    }
}
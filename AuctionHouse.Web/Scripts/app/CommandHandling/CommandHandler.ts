namespace AuctionHouse.CommandHandling {
    export abstract class CommandHandler<TCommand> {
        static $inject = ['$http'];

        constructor(private httpService: ng.IHttpService) {}

        handle(command: TCommand): ng.IPromise<void> {
            const url = `api/${this.getCommandName()}/Handle`;
            return this.httpService.post<void>(url, command).then(() => {});
        }

        protected abstract getCommandName(): string;
    }
}
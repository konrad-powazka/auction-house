import { CreateAuctionCommand } from '../../Messages/Commands';
import { CreateAuctionCommandHandler } from '../../CommandHandling/GeneratedCommandHandlers';

import { ICommandHandler } from '../../CommandHandling/ICommandHandler';
import { CommandHandlingErrorType } from '../../CommandHandling/CommandHandlingErrorType';

export class CreateAuctionCtrl implements ng.IController {
    fields: AngularFormly.IFieldArray;
    model: CreateAuctionCommand;

    static $inject = ['CreateAuctionCommandHandler'];

    constructor(private createAuctionCommandHandler: ICommandHandler<CreateAuctionCommand>) {
        let endDate = new Date();
        endDate.setDate(endDate.getDate() + 5);
        this.model = {
            id: this.guid(),
            title: '',
            description: '',
            auctionId: this.guid(),
            startingPrice: 5,
            endDate: endDate.toISOString(),
            buyNowPrice: 3
        }

        this.model.id = this.guid();

        this.fields = [
            {
                key: 'title',
                type: 'input',
                templateOptions: {
                    label: 'Title'
                }
            },
            {
                key: 'description',
                type: 'textarea',
                templateOptions: {
                    label: 'Description'
                }
            }
        ];
    }

    submit(): void {
        this.createAuctionCommandHandler
            .handle(this.model)
            .then(() => alert('Success'))
            .catch((commandHandlingErrorType: CommandHandlingErrorType) =>
                alert(`Command processing error: ${CommandHandlingErrorType[commandHandlingErrorType]}`));
    }

    private guid(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() +
            s4() +
            '-' +
            s4() +
            '-' +
            s4() +
            '-' +
            s4() +
            '-' +
            s4() +
            s4() +
            s4();
    }
}//
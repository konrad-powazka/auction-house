import { CreateAuctionCommand } from '../../Messages/Commands';
import { CreateAuctionCommandHandler } from '../../CommandHandling/GeneratedCommandHandlers';
import { ICommandHandler } from '../../CommandHandling/ICommandHandler';
import { CommandHandlingErrorType } from '../../CommandHandling/CommandHandlingErrorType';

export class CreateAuctionCtrl implements ng.IController {
    fields: AngularFormly.IFieldArray;
    model: CreateAuctionCommand;
    form: ng.IFormController

    static $inject = ['CreateAuctionCommandHandler'];

    constructor(private createAuctionCommandHandler: ICommandHandler<CreateAuctionCommand>) {
        this.model = {
            id: this.guid(),
            title: '',
            description: '',
            auctionId: this.guid(),
            startingPrice: 5,
            buyNowPrice: 10,
            endDate: ''
        }

        this.model.id = this.guid();

        this.fields = [
            {
                key: 'title',
                type: 'input',
                templateOptions: {
                    label: 'Title',
                    required: true,
                    minlength: 5,
                    maxlength: 200
                }
            },
            {
                key: 'description',
                type: 'textarea',
                templateOptions: {
                    label: 'Description',
                    required: true,
                    minlength: 10,
                    maxlength: 10000
                }
            },
            {
                key: 'endDate',
                type: 'dateTimePicker',
                templateOptions: {
                    label: 'End date and time',
                    required: true
                }
            }
        ];
    }

    submit(): void {
        if (!this.form.$valid) {
            return;
        }

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
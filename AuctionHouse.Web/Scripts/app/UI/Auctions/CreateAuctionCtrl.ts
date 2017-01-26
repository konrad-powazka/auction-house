import { CreateAuctionCommand } from '../../Messages/Commands';
import { CreateAuctionCommandHandler } from '../../CommandHandling/GeneratedCommandHandlers';
import { ICommandHandler } from '../../CommandHandling/ICommandHandler';
import { CommandHandlingErrorType } from '../../CommandHandling/CommandHandlingErrorType';
import {IQueryHandler as QueryHandler} from '../../QueryHandling/IQueryHandler';
import {GetAuctionDetailsQuery as AuctionDetailsQuery} from '../../Messages/Queries';
import {AuctionDetailsReadModel} from '../../ReadModel';

export class CreateAuctionCtrl implements ng.IController {
    fields: AngularFormly.IFieldArray;
    model: CreateAuctionCommand;
    form: ng.IFormController;

    static $inject = ['createAuctionCommandHandler', 'getAuctionDetailsQueryHandler', '$state'];

    constructor(private createAuctionCommandHandler: ICommandHandler<CreateAuctionCommand>,
        private getAuctionDetailsQueryHandler: QueryHandler<AuctionDetailsQuery, AuctionDetailsReadModel>,
        private stateService: ng.ui.IStateService) {
        this.model = {
            id: this.guid(),
            title: '',
            description: '',
            auctionId: this.guid(),
            startingPrice: 5,
            buyNowPrice: 10,
            endDate: undefined as any
        };
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
            .handle(this.model, true)
            .then(() => {
                alert('Success');
                this.stateService.go('displayAuction', { auctionId: this.model.auctionId });
            })
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
} //
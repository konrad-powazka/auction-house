import { CreateAuctionCommand } from '../../Messages/Commands';
import { CreateAuctionCommandHandler } from '../../CommandHandling/GeneratedCommandHandlers';
import { ICommandUiHandler } from '../Shared/CommandHandling/ICommandUiHandler';
import {IQueryHandler as QueryHandler} from '../../QueryHandling/IQueryHandler';
import {GetAuctionDetailsQuery as AuctionDetailsQuery} from '../../Messages/Queries';
import {AuctionDetailsReadModel} from '../../ReadModel';
import GuidGenerator from '../../Infrastructure/GuidGenerator';

export class CreateAuctionCtrl implements ng.IController {
    fields: AngularFormly.IFieldArray;
    model: CreateAuctionCommand;
	form: ng.IFormController;
	createAuctionCommandId = GuidGenerator.generateGuid();

    static $inject = ['createAuctionCommandUiHandler', 'getAuctionDetailsQueryHandler', '$state'];

    constructor(private createAuctionCommandUiHandler: ICommandUiHandler<CreateAuctionCommand>,
        private getAuctionDetailsQueryHandler: QueryHandler<AuctionDetailsQuery, AuctionDetailsReadModel>,
        private stateService: ng.ui.IStateService) {
        this.model = {
            id: GuidGenerator.generateGuid(),
            title: '',
            description: '',
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

	    this.createAuctionCommandUiHandler
		    .handle(this.model, this.createAuctionCommandId, true)
		    .then(() => {
			    this.stateService.go('displayAuction', { auctionId: this.model.id });
		    })
		    .catch(() => {
			    // TODO: To modal with proper error handling
		    });
    } 
}
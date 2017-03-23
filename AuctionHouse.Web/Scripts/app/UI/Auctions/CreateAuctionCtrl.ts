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
            startingPrice: 0,
            buyNowPrice: null,
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
				key: 'startingPrice',
				type: 'input',
				templateOptions: {
					label: 'Starting price',
					required: true,
					type: 'number',
					min: 0
				}
			},
			{
				key: 'buyNowPrice',
				type: 'input',
				templateOptions: {
					label: 'Buy now price',
					required: false,
					type: 'number',
					min: 0
				}
			},
            {
                key: 'endDate',
                type: 'dateTimePicker',
                templateOptions: {
                    label: 'End date and time',
                    required: true
				},
				validators: {
		            futureDate: {
			            expression($viewValue: any, $modelValue: any) {
				            const rawValue = $modelValue || $viewValue;
				            const momentValue = moment(rawValue);
				            return momentValue.isSameOrAfter(moment().add(5, 'minutes'));
			            },
						message($viewValue: any, $modelValue: any) {
							const rawValue = $modelValue || $viewValue;
							return `${moment(rawValue).format('Do MMMM YYYY, h:mm:ss A')} is not in the future`;
						}
		            }
	            }
            }
        ];
    }

    submit() {
        if (!this.form.$valid) {
            return;
        }

	    this.createAuctionCommandUiHandler
		    .handle(this.model, this.createAuctionCommandId, true)
		    .then(() => {
			    this.stateService.go('displayAuction', { auctionId: this.model.id });
		    });
    } 
}
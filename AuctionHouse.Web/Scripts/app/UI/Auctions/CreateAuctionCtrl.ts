import { CreateAuctionCommand } from '../../Messages/Commands';
import { CreateAuctionCommandHandler } from '../../CommandHandling/GeneratedCommandHandlers';

export class CreateAuctionCtrl implements ng.IController {
    fields: AngularFormly.IFieldArray;
    model = new CreateAuctionCommand();

    static $inject = ['CreateAuctionCommandHandler'];

    //TODO: inject interface
    constructor(private createAuctionCommandHandler: CreateAuctionCommandHandler) {
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
        this.createAuctionCommandHandler.handle(this.model);
    }
}
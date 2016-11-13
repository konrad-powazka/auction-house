namespace AuctionHouse.Auctions {
    export class CreateAuctionCtrl implements ng.IController {
        fields: AngularFormly.IFieldArray;
        model: any = {};
        message: string = 'asdsad';

        constructor() {
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
            }];
        }
    }
}
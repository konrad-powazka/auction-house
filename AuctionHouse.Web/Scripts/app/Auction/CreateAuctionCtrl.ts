namespace AuctionHouse.Auctions {
    import IController = angular.IController;
    import IScope = angular.IScope;
    import IFieldArray = AngularFormly.IFieldArray;

    export class CreateAuctionCtrl implements IController {
        fields: IFieldArray;
        model: any = {};

        constructor(private $scope: IScope) {
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
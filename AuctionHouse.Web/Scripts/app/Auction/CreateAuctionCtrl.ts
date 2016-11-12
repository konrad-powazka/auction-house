namespace AuctionHouse.Auctions {
    import IController = angular.IController;
    import IScope = angular.IScope;

    export class CreateAuctionCtrl implements IController {
        constructor(private $scope: IScope) {
            ($scope as any).message = 'Test message';
        }
    }
}
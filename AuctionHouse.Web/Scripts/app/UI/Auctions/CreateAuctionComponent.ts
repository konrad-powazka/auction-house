namespace AuctionHouse.Auctions {
    export class CreateAuctionComponent implements Infrastructure.INamedComponentOptions {
        controller: ng.Injectable<ng.IControllerConstructor> = CreateAuctionCtrl;
        templateUrl = 'Template/Auctions/Create';
        registerAs = 'createAuction';
    }
}
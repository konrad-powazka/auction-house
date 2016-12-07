namespace AuctionHouse.Auctions {
    export class CreateAuctionComponent implements Infrastructure.INamedComponentOptions {
        controller = CreateAuctionCtrl;
        templateUrl = 'Template/Auctions/Create';
        registerAs = 'createAuction';
    }
}
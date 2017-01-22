import {AuctionDetailsReadModel} from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import {GetAuctionDetailsQuery} from '../../Messages/Queries';

export class DisplayAuctionCtrl implements ng.IController {
    auctionId: string;
    auction: AuctionDetailsReadModel;

    static $inject = ['getAuctionDetailsQueryHandler'];

    constructor(getAuctionDetailsQueryHandler: IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>) {
        getAuctionDetailsQueryHandler.handle({
                id: this.auctionId
            })
            .then(auction => this.auction = auction);
    }

}
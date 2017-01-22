import { DisplayAuctionCtrl } from './DisplayAuctionCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class DisplayAuctionComponent implements INamedComponentOptions {
    controller = DisplayAuctionCtrl;
    templateUrl = 'Template/Auctions/Display';
    registerAs = 'displayAuction';
    bindings = {
        auctionId: '<'
    }
}
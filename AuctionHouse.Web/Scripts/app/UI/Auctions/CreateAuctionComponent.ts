import { CreateAuctionCtrl } from './CreateAuctionCtrl';
import { INamedComponentOptions } from '../../Infrastructure/INamedComponentOptions';

export class CreateAuctionComponent implements INamedComponentOptions {
    controller = CreateAuctionCtrl;
    templateUrl = 'Template/Auctions/Create';
    registerAs = 'createAuction';
}
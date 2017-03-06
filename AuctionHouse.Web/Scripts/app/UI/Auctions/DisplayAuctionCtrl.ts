import {AuctionDetailsReadModel} from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import {GetAuctionDetailsQuery} from '../../Messages/Queries';
import {SecurityUiService} from '../Shared/SecurityUiService';
import {MakeBidCommand} from '../../Messages/Commands';
import {ICommandHandler } from '../../CommandHandling/ICommandHandler';
import GenericModalService from '../Shared/GenericModalService';

export class DisplayAuctionCtrl implements ng.IController {
	auctionId: string;
	auction: AuctionDetailsReadModel;
	makeBidFields: AngularFormly.IFieldArray;

	makeBidModel: {
		price: number;
	};

	static $inject = ['getAuctionDetailsQueryHandler', 'securityUiService', 'createAuctionCommandUiHandler', 'genericModalService'];

	constructor(getAuctionDetailsQueryHandler: IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>,
		public securityUiService: SecurityUiService,
		private createAuctionCommandUiHandler: ICommandHandler<MakeBidCommand>,
		private genericModalService: GenericModalService) {
		getAuctionDetailsQueryHandler.handle({
				id: this.auctionId
			})
			.then(auction => {
				this.auction = auction;

				this.makeBidFields = [
					{
						key: 'price',
						type: 'input',
						defaultValue: this.auction.minimalPriceForNextBidder,
						templateOptions: {
							label: '',
							required: true
						}
					}
				];
			});
	}

	makeBid(): void {
		this.securityUiService.ensureUserIsAuthenticated()
			.then(() => {
				if (this.securityUiService.currentUserName === this.auction.createdByUserName) {
					this.genericModalService.showErrorNotification('You cannot bid at your own auction.');
					return;
				}
			});
	}
}
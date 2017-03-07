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

	static $inject = ['getAuctionDetailsQueryHandler', 'securityUiService', 'makeBidCommandUiHandler', 'genericModalService'];

	constructor(
		private getAuctionDetailsQueryHandler: IQueryHandler<GetAuctionDetailsQuery, AuctionDetailsReadModel>,
		public securityUiService: SecurityUiService,
		private makeBidCommandUiHandler: ICommandHandler<MakeBidCommand>,
		private genericModalService: GenericModalService) {
		getAuctionDetailsQueryHandler.handle({
				id: this.auctionId
			})
			.then(auction => {
				this.auctionLoadedCallback(auction);
			});
	}

	private auctionLoadedCallback(auction: AuctionDetailsReadModel) {
		this.auction = auction;
		this.initMakeBidFields(auction);
		this.makeBidModel.price = auction.minimalPriceForNextBidder;
	}

	private initMakeBidFields(auction: AuctionDetailsReadModel) {
		this.makeBidFields = [
			{
				key: 'price',
				type: 'input',
				templateOptions: {
					label: '',
					required: true,
					type: 'number',
					min: this.auction.minimalPriceForNextBidder
				}
			}
		];
	}

	makeBid(): void {
		this.securityUiService.ensureUserIsAuthenticated()
			.then(() => {
				if (this.securityUiService.currentUserName === this.auction.createdByUserName) {
					this.genericModalService.showErrorNotification('You cannot bid at your own auction.');
					return;
				}

				const makeBidCommand: MakeBidCommand = {
					auctionId: this.auctionId,
					expectedAuctionVersion: this.auction.version,
					price: this.makeBidModel.price
				};

				this.makeBidCommandUiHandler.handle(makeBidCommand, true)
					.then(() => {
						return this.getAuctionDetailsQueryHandler.handle({
							id: this.auctionId
						});
					})
					.then(auction => {
						this.auctionLoadedCallback(auction);

						// TODO: check if is highest bidder
					});
			});
	}
}
import {AuctionDetailsReadModel} from '../../ReadModel';
import {IQueryHandler} from '../../QueryHandling/IQueryHandler';
import {GetAuctionDetailsQuery} from '../../Messages/Queries';
import {SecurityUiService} from '../Shared/SecurityUiService';
import {MakeBidCommand} from '../../Messages/Commands';
import {ICommandHandler } from '../../CommandHandling/ICommandHandler';
import GenericModalService from '../Shared/GenericModalService';
import GuidGenerator from '../../Infrastructure/GuidGenerator';

export class DisplayAuctionCtrl implements ng.IController {
	auctionId: string;
	auction: AuctionDetailsReadModel;
	bidPrice: number;

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
		this.bidPrice = auction.minimalPriceForNextBidder;
	}

	makeUserEnteredBid() {
		if (!_(this.bidPrice).isNumber()) {
			this.genericModalService.showErrorNotification('Please enter a valid bid price.');
			return;
		} else if (this.bidPrice < this.auction.minimalPriceForNextBidder) {
			this.genericModalService
				.showErrorNotification(`Minimal bid price is ${this.auction.minimalPriceForNextBidder}.`);
			return;
		}

		this.makeBid(this.bidPrice);
	}

	makeBuyNowBid() {
		if (!this.auction.buyNowPrice) {
			throw new Error();
		}

		this.makeBid(this.auction.buyNowPrice as number);
	}

	private makeBid(bidPrice: number): void {
		this.securityUiService.ensureUserIsAuthenticated()
			.then(() => {
				if (this.securityUiService.currentUserName === this.auction.createdByUserName) {
					this.genericModalService.showErrorNotification('You cannot bid at your own auction.');
					return;
				}

				const makeBidCommand: MakeBidCommand = {
					auctionId: this.auctionId,
					expectedAuctionVersion: this.auction.version,
					price: bidPrice
				};

				this.makeBidCommandUiHandler.handle(makeBidCommand, GuidGenerator.generateGuid(), true)
					.then(() => {
						return this.getAuctionDetailsQueryHandler.handle({
							id: this.auctionId
						});
					})
					.then(auction => {
						var previousAuction = this.auction;
						this.auctionLoadedCallback(auction);

						if (auction.highestBidderUserName === this.securityUiService.currentUserName) {
							if (auction.wasFinished) {
								this.genericModalService
									.showSuccessNotification('Congratulations, you won the auction! Contact the seller in order to establish payment and delivery details.');
							} else if (auction.highestBidderUserName === previousAuction.highestBidderUserName) {
								this.genericModalService.showSuccessNotification('You are still the highest bidder.');
							} else {
								this.genericModalService.showSuccessNotification('Congratulations, you are now the highest bidder!');
							}
						} else {
							this.genericModalService.showInformationNotification('Unfortunately your offer was not the highest.');
						}
					});
			});
	}
}
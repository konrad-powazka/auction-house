//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.


export
class UserInboxReadModel
{
	public pageNumber: number;
	public totalPagesCount: number;
	public totalItemsCount: number;
	public pageItems: UserMessageReadModel[];
	public pageSize: number;
}
export
class UserMessageReadModel
{
	public id: string;
	public subject: string;
	public body: string;
	public recipientUserName: string;
	public senderUserName: string;
	public sentDateTime: string;
}
export
class AuctionListItemReadModel
{
	public id: string;
	public createdByUserName: string;
	public title: string;
	public description: string;
	public endDate: string;
	public buyNowPrice: number | null;
	public minimalPriceForNextBidder: number;
	public wasFinished: boolean;
	public numberOfBids: number;
	public finishedDateTime: any | null;
	public highestBidderUserName: string;
	public currentPrice: number;
}
export
class AuctionsListReadModel
{
	public pageNumber: number;
	public totalPagesCount: number;
	public totalItemsCount: number;
	public pageItems: AuctionListItemReadModel[];
	public pageSize: number;
}
export
class AuctionDetailsReadModel extends AuctionListItemReadModel
{
	public startingPrice: number;
	public version: number;
	public biddersUserNames: string[];
}

//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.


export
class CancelAuctionCommand
{
	public id: string;
}
export
class CreateAuctionCommand
{
	public id: string;
	public auctionId: string;
	public title: string;
	public description: string;
	public startingPrice: number;
	public buyNowPrice: number;
	public endDate: string;
}
export
class MakeBidCommand
{
	public id: string;
	public auctionId: string;
	public price: number;
}

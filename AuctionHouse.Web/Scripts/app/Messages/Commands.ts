//     This code was generated by a Reinforced.Typings tool. 
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.


export
class SendUserMessageCommand
{
	public messageSubject: string;
	public messageBody: string;
	public recipientUserName: string;
}
export
class PopulateDatabaseWithTestDataCommand
{
}
export
class CreateAuctionCommand
{
	public id: string;
	public title: string;
	public description: string;
	public startingPrice: number;
	public buyNowPrice: number | null;
	public endDate: string;
}
export
class FinishAuctionCommand
{
	public id: string;
}
export
class MakeBidCommand
{
	public auctionId: string;
	public price: number;
	public expectedAuctionVersion: number;
}

using System.Collections.Generic;

namespace AuctionHouse.ReadModel.Dtos.Auctions
{
	public class AuctionDetailsReadModel : AuctionListItemReadModel
	{
		private List<string> _biddersUserNames = new List<string>();

		public decimal StartingPrice { get; set; }
		public int Version { get; set; }

		public List<string> BiddersUserNames
		{
			get { return _biddersUserNames; }
			set { _biddersUserNames = value ?? new List<string>(); }
		}
	}
}
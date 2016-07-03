using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouse.Domain.Auctions
{
    public class Auction
    {

        public Auction(Guid id, string title, DateTime endDate, decimal startingPrice, decimal? buyNowPrice)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException(null, nameof(title));
            }


            //TODO: Mockable date
            if (endDate <= DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate));
            }


        }

        public AuctionState State { get; private set; }
    }
}

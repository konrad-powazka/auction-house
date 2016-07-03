using AuctionHouse.Application;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers
{
    public class QueryController<TQuery, TResult> : Controller where TQuery : IQuery<TResult>
    {
        [HttpPost]
        public TResult Handle([FromBody] TQuery command)
        {
            return default(TResult);
        }
    }
}
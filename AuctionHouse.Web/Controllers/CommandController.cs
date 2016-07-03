using AuctionHouse.Application;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers
{
    public class CommandController<TCommand> : Controller where TCommand : ICommand
    {
        [HttpPost]
        public void Handle([FromBody] TCommand command)
        {
        }
    }
}
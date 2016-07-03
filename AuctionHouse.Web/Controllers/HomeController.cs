using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public ViewResult Index()
        {
            return View();
        }
    }
}
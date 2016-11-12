using System.Web.Mvc;

namespace AuctionHouse.Web.Controllers.View
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
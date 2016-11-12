using System.Web.Mvc;

namespace AuctionHouse.Web.Controllers.View
{
    public class TemplateController : Controller
    {
        [Route("Template/{*path}")]
        [HttpGet]
        public ActionResult Get(string path)
        {
            return PartialView($"~/Views/{path}.cshtml");
        }
    }
}
using System.Web.Mvc;
using System.Web.Routing;

namespace AuctionHouse.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "OtherTemplates",
                "Template/{*path}",
                new {controller = "Template", action = "Get"}
                );

            routes.MapRoute(
                "Default",
                "",
                new {controller = "Home", action = "Index"}
                );
        }
    }
}
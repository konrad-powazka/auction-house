using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace AuctionHouse.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.signalR-2.2.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-messages.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/api-check.js",
                "~/Scripts/formly.js",
                "~/Scripts/angular-formly-templates-bootstrap.js",
                "~/node_modules/moment/moment.js",
                "~/node_modules/angular-ui-bootstrap-datetimepicker/datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/node_modules/angular-ui-bootstrap-datetimepicker/datetimepicker.css",
                "~/Content/Site.css"));
        }
    }
}
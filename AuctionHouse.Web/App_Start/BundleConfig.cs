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
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-ui.js",
                "~/Scripts/api-check.js",
                "~/Scripts/formly.js",
                "~/Scripts/angular-formly-templates-bootstrap.js"));

            var appBundle = new ScriptBundle("~/bundles/app").IncludeDirectory("~/Scripts/app", "*.js", true);
            appBundle.Orderer = new AppBundleOrderer();
            bundles.Add(appBundle);

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/Site.css"));
        }

        private class AppBundleOrderer : IBundleOrderer
        {
            public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
            {
                return files.OrderBy(f => f.IncludedVirtualPath.EndsWith("Application.js") ? 1 : 0);
            }
        }
    }
}
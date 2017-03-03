﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace AuctionHouse.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.signalR-2.2.1.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-messages.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/api-check.js",
                "~/Scripts/formly.js",
                "~/Scripts/angular-formly-templates-bootstrap.js",
                "~/node_modules/moment/moment.js",
                "~/Scripts/datetimepicker-fixed.js",
                "~/Scripts/spin.js",
                "~/Scripts/angular-spinner.js",
				"~/node_modules/ng-tasty/ng-tasty-tpls.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/node_modules/angular-ui-bootstrap-datetimepicker/datetimepicker.css",
                "~/Content/Site.css"));
        }
    }
}
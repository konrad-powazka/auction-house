using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(AuctionHouse.Web.Startup))]

namespace AuctionHouse.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
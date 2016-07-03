using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AuctionHouse.Application;
using AuctionHouse.Core.Reflection;
using AuctionHouse.Web.Infrastructure;
using AuctionHouse.Web.ModelBinding;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AuctionHouse.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var commandTypes = LoadAllAssignableTypes<ICommand>().Where(t => t.CanBeInstantiated()).ToList();
            var builder = services.AddMvc();
            builder.PartManager.ApplicationParts.Add(new CqrsControllersApplicationPart(commandTypes));

            builder.Services.Replace(new ServiceDescriptor(typeof(IApplicationModelProvider),
                typeof(CqrsControllersApplicationModelProvider), ServiceLifetime.Transient));

            return ConfigureDependencyInjection(services);
        }

        private static IServiceProvider ConfigureDependencyInjection(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            var auctionHouseAssemblies = LoadAuctionHouseAssemblies().ToArray();
            //builder.RegisterAssemblyTypes(auctionHouseAssemblies).InstancePerLifetimeScope().AsImplementedInterfaces();
            builder.Populate(services);
            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes
                    .MapRoute(
                        name: "api",
                        template: "api/{controller}/{action=Handle}");
            }).UseStaticFiles();
        }

        private static IEnumerable<Type> LoadAllAssignableTypes<TAssignableToType>()
        {
            return LoadAuctionHouseAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsAssignableTo<TAssignableToType>());
        }

        private static IEnumerable<Assembly> LoadAuctionHouseAssemblies()
        {
            var rootAssemblyName = Assembly.GetExecutingAssembly().GetName();

            return LoadAuctionHouseAssembliesRecursive(rootAssemblyName).Distinct().ToList();
        }

        private static IEnumerable<Assembly> LoadAuctionHouseAssembliesRecursive(AssemblyName rootAssemblyName)
        {
            if (!IsAuctionHouseAssembly(rootAssemblyName))
            {
                yield break;
            }

            var rootAssembly =
                AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.FullName == rootAssemblyName.FullName) ??
                AppDomain.CurrentDomain.Load(rootAssemblyName);

            yield return rootAssembly;

            foreach (
                var auctionHouseAssembly in
                    rootAssembly.GetReferencedAssemblies()
                        .SelectMany(LoadAuctionHouseAssembliesRecursive))
            {
                yield return auctionHouseAssembly;
            }
        }

        private static bool IsAuctionHouseAssembly(AssemblyName assemblyName)
        {
            return assemblyName.FullName.Contains("AuctionHouse");
        }
    }
}
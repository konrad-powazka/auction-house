using System;
using System.Linq;
using AuctionHouse.DynamicTypeScanning;
using Reinforced.Typings.Fluent;

namespace AuctionHouse.Web.CodeGen
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            var typesToExport =
                DynamicTypeScanner.GetCommandTypes()
                    .Concat(DynamicTypeScanner.GetQueryTypes())
                    .Concat(DynamicTypeScanner.GetReadModelTypes());

            builder.ExportAsClasses(typesToExport,
                c =>
                    c.WithProperties(
                        p =>
                            p.PropertyType != typeof(Guid) && p.PropertyType != typeof(DateTime) &&
                            p.GetGetMethod() != null,
                        pc => pc.CamelCase())
                        .WithProperties(
                            p =>
                                (p.PropertyType == typeof(Guid) || p.PropertyType == typeof(DateTime)) &&
                                p.GetGetMethod() != null,
                            pc => pc.CamelCase().Type<string>()));
        }
    }
}
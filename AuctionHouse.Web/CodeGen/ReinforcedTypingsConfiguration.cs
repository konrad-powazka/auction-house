using System;
using System.Linq;
using Reinforced.Typings.Fluent;

namespace AuctionHouse.Web.CodeGen
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            var typesToExport =
                CodeGenTypes.GetComandTypes()
                    .Concat(CodeGenTypes.GetQueryTypes())
                    .Concat(CodeGenTypes.GetReadModelTypes());

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
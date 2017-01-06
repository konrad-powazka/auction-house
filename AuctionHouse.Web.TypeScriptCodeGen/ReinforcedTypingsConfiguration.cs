using System;
using System.Collections.Generic;
using System.Reflection;
using AuctionHouse.DynamicTypeScanning;
using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Fluent;
using Reinforced.Typings.Generators;

namespace AuctionHouse.Web.TypeScriptCodeGen
{
    public static class ReinforcedTypingsConfiguration
    {
        public static void Configure(ConfigurationBuilder builder)
        {
            ExportTypes(builder, DynamicTypeScanner.GetCommandTypes(), "Messages/Commands.ts");
            ExportTypes(builder, DynamicTypeScanner.GetQueryTypes(), "Messages/Queries.ts");
            ExportTypes(builder, DynamicTypeScanner.GetReadModelTypes(), "ReadModel.ts");
        }

        private static void ExportTypes(ConfigurationBuilder builder, IEnumerable<Type> typesToExport, string fileName)
        {
            //TODO: nullable
            builder.ExportAsClasses(typesToExport,
                c =>
                {
                    c.PathToFile = fileName;
                    c.WithCodeGenerator<ExportKeywordPrependingClassCodeGenerator>();

                    c.DontIncludeToNamespace().WithProperties(
                        p =>
                            p.PropertyType != typeof(Guid) && p.PropertyType != typeof(DateTime) &&
                            p.GetGetMethod() != null,
                        pc => pc.CamelCase())
                        .WithProperties(
                            p =>
                                (p.PropertyType == typeof(Guid) || p.PropertyType == typeof(DateTime)) &&
                                p.GetGetMethod() != null,
                            pc => pc.CamelCase().Type<string>());
                });
        }

        private class ExportKeywordPrependingClassCodeGenerator : ClassCodeGenerator
        {
            public override RtClass GenerateNode(Type element, RtClass result, TypeResolver resolver)
            {
                var rtClass = base.GenerateNode(element, result, resolver);
                Context.Location.CurrentModule.CompilationUnits.Add(new RtRaw("export"));
                return rtClass;
            }
        }

        private class OptionalPropertyCodeGenerator : PropertyCodeGenerator
        {
            public override RtField GenerateNode(MemberInfo element, RtField result, TypeResolver resolver)
            {
                var baseResult = base.GenerateNode(element, result, resolver);
                baseResult.Type = new RtSimpleTypeName($"{baseResult.Type} | null");
                return baseResult;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AuctionHouse.Messages.Commands;
using AuctionHouse.Messages.Events;
using AuctionHouse.Messages.Queries;
using AuctionHouse.ReadModel.Dtos;
using Reinforced.Typings;
using Reinforced.Typings.Ast;
using Reinforced.Typings.Fluent;
using Reinforced.Typings.Generators;

namespace AuctionHouse.Web.TypeScriptCodeGen
{
    public static class ReinforcedTypingsConfiguration
    {
        private static readonly Dictionary<Type, Type> TypeOverrides = new Dictionary<Type, Type>
        {
            [typeof(Guid)] = typeof(string),
            [typeof(DateTime)] = typeof(string),
            [typeof(IReadOnlyCollection<Guid>)] = typeof(string[])
        };

        public static void Configure(ConfigurationBuilder builder)
        {
            ExportTypes(builder, CommandsAssemblyMarker.GetCommandTypes(), "Messages/Commands.ts");
            ExportTypes(builder, QueriesAssemblyMarker.GetQueryTypes(), "Messages/Queries.ts");
            ExportTypes(builder, ReadModelDtosAssemblyMarker.GetReadModelDtoTypes(), "ReadModel.ts");
            ExportTypes(builder, EventsAssemblyMarker.GetEventTypes(), "Events.ts");
        }

        private static void ExportTypes(ConfigurationBuilder builder, IReadOnlyCollection<Type> typesToExport,
            string fileName)
        {
            // We need to do this becase of a bug in ReinforcedTypings which causes a
            // "A class must be declared after its base class." TS compiler error
            typesToExport = GetOrderedInheritanceHierarchy(typesToExport).ToList();

            //TODO: nullable
            builder.ExportAsClasses(typesToExport,
                c =>
                {
                    c.PathToFile = fileName;
	                c.WithCodeGenerator<ExportKeywordPrependingClassCodeGenerator>();

                    c.DontIncludeToNamespace().WithProperties(
                        p =>
                            !TypeOverrides.ContainsKey(p.PropertyType) && p.GetGetMethod() != null,
                        pc => pc.CamelCase().WithCodeGenerator<OptionalPropertyCodeGenerator>());

                    foreach (var typeOverride in TypeOverrides)
                    {
	                    c.WithProperties(
		                    p => p.PropertyType == typeOverride.Key && p.GetGetMethod() != null,
		                    pc => pc.CamelCase().Type(typeOverride.Value));
                    }
                });
        }

        private static IEnumerable<Type> GetOrderedInheritanceHierarchy(IEnumerable<Type> types)
        {
            var unorderedTypesHierarchy =
                types.SelectMany(GetTypeInheritanceChain).Where(t => t != typeof(object)).Distinct().ToList();

            var typesOrderedInLastIteration = unorderedTypesHierarchy.Where(t => t.BaseType == typeof(object)).ToList();
            var orderedTypesHierarchy = new List<Type>();
            var typesLeftToOrder = new HashSet<Type>(unorderedTypesHierarchy);

            while (typesLeftToOrder.Any())
            {
                foreach (var typeOrderedInLastIteration in typesOrderedInLastIteration)
                {
                    typesLeftToOrder.Remove(typeOrderedInLastIteration);
                    orderedTypesHierarchy.Add(typeOrderedInLastIteration);
                }

                typesOrderedInLastIteration =
                    typesLeftToOrder.Where(
                        derivedType => typesOrderedInLastIteration.Any(baseType => derivedType.BaseType == baseType))
                        .ToList();
            }

            Debug.Assert(!typesLeftToOrder.Any());

            return orderedTypesHierarchy;
        }

        private static IEnumerable<Type> GetTypeInheritanceChain(Type type)
        {
            if (type == typeof(object))
            {
                return new[] {type};
            }

            return GetTypeInheritanceChain(type.BaseType).Concat(new[] {type});
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

				var propertyElement = element as PropertyInfo;
				if (propertyElement != null && Nullable.GetUnderlyingType(propertyElement.PropertyType) != null)
				{
					baseResult.Type = new RtSimpleTypeName($"{baseResult.Type} | null");
				}

				return baseResult;
			}
		}
    }
}
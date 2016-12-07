using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using AuctionHouse.Core.Emit;
using AuctionHouse.Web.Controllers.Api;

namespace AuctionHouse.Web.CodeGen
{
    public static class CqrsApiControllerTypesEmitter
    {
        private const string AssemblyNameText = "AuctionHouse.Web.Controllers.Api.Dynamic";

        private static readonly Lazy<Assembly> CqrsApiControllersAssembly =
            new Lazy<Assembly>(EmitCqrsApiControllersAssemblyInternal, LazyThreadSafetyMode.ExecutionAndPublication);

        public static Assembly EmitCqrsApiControllersAssembly()
        {
            return CqrsApiControllersAssembly.Value;
        }

        public static Assembly EmitCqrsApiControllersAssemblyInternal()
        {
            var appDomain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName(AssemblyNameText);

            var assemblyBuilder =
                appDomain.DefineDynamicAssembly(assemblyName,
                    AssemblyBuilderAccess.RunAndSave);

            var moduleBuilder =
                assemblyBuilder.DefineDynamicModule(assemblyName.Name,
                    assemblyName.Name + ".dll");

            EmitCommandControllerTypes(moduleBuilder);
            EmitQueryControllerTypes(moduleBuilder);

            assemblyBuilder.Save(assemblyName.Name + ".dll");

            return assemblyBuilder;
        }

        private static void EmitCommandControllerTypes(ModuleBuilder moduleBuilder)
        {
            var commandTypes = CodeGenTypes.GetComandTypes();
            EmitControllerTypes(moduleBuilder, typeof(CommandController<>), commandTypes, t => new[] {t});
        }

        private static void EmitQueryControllerTypes(ModuleBuilder moduleBuilder)
        {
            var queryTypeToQueryResultTypeMap = CodeGenTypes.GetQueryTypeInfos()
                .ToDictionary(i => i.QueryType, i => i.QueryResultType);


            Func<Type, IEnumerable<Type>> getControllerBaseTypeGenericArgsForMessageType = queryType =>
            {
                var queryResultType = queryTypeToQueryResultTypeMap[queryType];

                return new[] {queryType, queryResultType};
            };

            EmitControllerTypes(moduleBuilder, typeof(QueryController<,>), queryTypeToQueryResultTypeMap.Keys,
                getControllerBaseTypeGenericArgsForMessageType);
        }

        private static void EmitControllerTypes(ModuleBuilder moduleBuilder, Type controllersBaseType,
            IEnumerable<Type> messageTypes,
            Func<Type, IEnumerable<Type>> getControllerBaseTypeGenericArgsForMessageType)
        {
            foreach (var messageType in messageTypes)
            {
                var controllerTypeName = $"{AssemblyNameText}.{messageType.Name}Controller";

                var controllerBaseTypeGenericArgs =
                    getControllerBaseTypeGenericArgsForMessageType(messageType).ToArray();

                var controllerBaseType = controllersBaseType.MakeGenericType(controllerBaseTypeGenericArgs);

                var controllerTypeBuilder = moduleBuilder.DefineType(controllerTypeName, TypeAttributes.Public,
                    controllerBaseType);

                controllerTypeBuilder.CreatePassThroughConstructors(controllerBaseType);
                controllerTypeBuilder.CreateType();
            }
        }
    }
}
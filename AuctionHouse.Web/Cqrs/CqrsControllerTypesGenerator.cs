using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using AuctionHouse.Core.Emit;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Reflection;
using AuctionHouse.Messages.Commands;
using AuctionHouse.Messages.Queries;
using AuctionHouse.Web.Controllers.Api;

namespace AuctionHouse.Web.Cqrs
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
            var commandsCommonType = typeof(ICommand);
            EmitControllerTypes(moduleBuilder, typeof(CommandController<>), commandsCommonType,
                typeof(CommandsAssemblyMarker), t => new[] {t});
        }

        private static void EmitQueryControllerTypes(ModuleBuilder moduleBuilder)
        {
            var queriesCommonType = typeof(IQuery<>);

            Func<Type, IEnumerable<Type>> getControllerBaseTypeGenericArgsForMessageType = queryType =>
            {
                var queryResultType =
                    queryType.GetInterfaces()
                        .Single(t => t.IsGenericType && t.GetGenericTypeDefinition() == queriesCommonType)
                        .GetGenericArguments()
                        .Single();

                return new[] {queryType, queryResultType};
            };

            EmitControllerTypes(moduleBuilder, typeof(QueryController<,>), queriesCommonType,
                typeof(QueriesAssemblyMarker), getControllerBaseTypeGenericArgsForMessageType);
        }

        private static void EmitControllerTypes(ModuleBuilder moduleBuilder, Type controllersBaseType,
            Type messagesCommonType, Type messagesAssemblyMarkerType,
            Func<Type, IEnumerable<Type>> getControllerBaseTypeGenericArgsForMessageType)
        {
            var messageTypes = messagesAssemblyMarkerType.Assembly.GetTypes().Where(
                t =>
                    (messagesCommonType.IsAssignableFrom(t) ||
                     t.GetInterfaces()
                         .Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == messagesCommonType)) &&
                    t.CanBeInstantiated());

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
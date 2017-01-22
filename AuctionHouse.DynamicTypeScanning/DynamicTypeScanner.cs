using System;
using System.Collections.Generic;
using System.Linq;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Reflection;
using AuctionHouse.Messages.Commands;
using AuctionHouse.Messages.Events;
using AuctionHouse.Messages.Queries;
using AuctionHouse.ReadModel;

namespace AuctionHouse.DynamicTypeScanning
{
    public static class DynamicTypeScanner
    {
        private static readonly Lazy<IReadOnlyCollection<Type>> GetCommandTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(CommandsAssemblyMarker).Assembly.GetTypes()
                        .Where(t => t.GetInterfaces().Any(i => i == typeof(ICommand)) && t.CanBeInstantiated())
                        .ToList());

        private static readonly Lazy<IReadOnlyCollection<QueryTypeInfo>> QueryTypeInfosLazy =
            new Lazy<IReadOnlyCollection<QueryTypeInfo>>(
                () =>
                    typeof(QueriesAssemblyMarker).Assembly.GetTypes()
                        .Where(
                            t =>
                                t.GetInterfaces()
                                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>)) &&
                                t.CanBeInstantiated())
                        .Select(t => new QueryTypeInfo(t))
                        .ToList());

        private static readonly Lazy<IReadOnlyCollection<Type>> GetReadModelTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(ReadModelAssemblyMarker).Assembly.GetTypes()
                        .Where(t => t.Name.EndsWith("ReadModel") && t.CanBeInstantiated())
                        .ToList());

        private static readonly Lazy<IReadOnlyCollection<Type>> GetEventTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(EventsAssemblyMarker).Assembly.GetTypes()
                        .Where(t => typeof(IEvent).IsAssignableFrom(t) && t.CanBeInstantiated())
                        .ToList());

        public static IReadOnlyCollection<Type> GetCommandTypes() => GetCommandTypesLazy.Value;
        public static IReadOnlyCollection<QueryTypeInfo> GetQueryTypeInfos() => QueryTypeInfosLazy.Value;

        public static IReadOnlyCollection<Type> GetQueryTypes()
            => QueryTypeInfosLazy.Value.Select(t => t.QueryType).ToList();

        public static IReadOnlyCollection<Type> GetReadModelTypes() => GetReadModelTypesLazy.Value;

        public static IReadOnlyCollection<Type> GetEventTypes() => GetEventTypesLazy.Value;
    }
}
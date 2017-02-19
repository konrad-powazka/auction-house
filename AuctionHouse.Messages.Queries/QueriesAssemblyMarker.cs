using System;
using System.Collections.Generic;
using System.Linq;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Reflection;

namespace AuctionHouse.Messages.Queries
{
    public static class QueriesAssemblyMarker
    {
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

        public static IReadOnlyCollection<QueryTypeInfo> GetQueryTypeInfos() => QueryTypeInfosLazy.Value;

        public static IReadOnlyCollection<Type> GetQueryTypes()
            => QueryTypeInfosLazy.Value.Select(t => t.QueryType).ToList();
    }
}
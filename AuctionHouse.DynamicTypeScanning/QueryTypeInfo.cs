using System;
using System.Linq;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.DynamicTypeScanning
{
    public class QueryTypeInfo
    {
        public QueryTypeInfo(Type queryType)
        {
            if (queryType == null)
            {
                throw new ArgumentNullException(nameof(queryType));
            }

            QueryType = queryType;
            var queryInterfaceType =
                queryType.GetInterfaces()
                    .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
            QueryResultType = queryInterfaceType.GenericTypeArguments.Single();
        }

        public Type QueryType { get; }
        public Type QueryResultType { get; }
    }
}
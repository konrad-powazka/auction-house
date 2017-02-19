using System;
using System.Collections.Generic;
using System.Linq;
using AuctionHouse.Core.Reflection;

namespace AuctionHouse.ReadModel
{
    public static class ReadModelAssemblyMarker
    {
        private static readonly Lazy<IReadOnlyCollection<Type>> GetReadModelTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(ReadModelAssemblyMarker).Assembly.GetTypes()
                        .Where(t => t.Name.EndsWith("ReadModel") && t.CanBeInstantiated())
                        .ToList());

        public static IReadOnlyCollection<Type> GetReadModelTypes() => GetReadModelTypesLazy.Value;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AuctionHouse.Core.Reflection;

namespace AuctionHouse.ReadModel.Dtos
{
    public static class ReadModelDtosAssemblyMarker
	{
        private static readonly Lazy<IReadOnlyCollection<Type>> GetReadModelTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(ReadModelDtosAssemblyMarker).Assembly.GetTypes()
                        .Where(t => t.Name.EndsWith("ReadModel") && t.CanBeInstantiated())
                        .ToList());

        public static IReadOnlyCollection<Type> GetReadModelDtoTypes() => GetReadModelTypesLazy.Value;
    }
}
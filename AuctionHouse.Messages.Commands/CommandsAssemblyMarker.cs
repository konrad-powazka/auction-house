using System;
using System.Collections.Generic;
using System.Linq;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Reflection;

namespace AuctionHouse.Messages.Commands
{
    public static class CommandsAssemblyMarker
    {
        private static readonly Lazy<IReadOnlyCollection<Type>> GetCommandTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(CommandsAssemblyMarker).Assembly.GetTypes()
                        .Where(t => t.GetInterfaces().Any(i => i == typeof(ICommand)) && t.CanBeInstantiated())
                        .ToList());

        public static IReadOnlyCollection<Type> GetCommandTypes() => GetCommandTypesLazy.Value;
    }
}
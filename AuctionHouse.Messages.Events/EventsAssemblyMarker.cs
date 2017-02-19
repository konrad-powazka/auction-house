using System;
using System.Collections.Generic;
using System.Linq;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Core.Reflection;

namespace AuctionHouse.Messages.Events
{
    public static class EventsAssemblyMarker
    {
        private static readonly Lazy<IReadOnlyCollection<Type>> GetEventTypesLazy =
            new Lazy<IReadOnlyCollection<Type>>(
                () =>
                    typeof(EventsAssemblyMarker).Assembly.GetTypes()
                        .Where(t => typeof(IEvent).IsAssignableFrom(t) && t.CanBeInstantiated())
                        .ToList());


        public static IReadOnlyCollection<Type> GetEventTypes() => GetEventTypesLazy.Value;
    }
}
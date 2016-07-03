using System;

namespace AuctionHouse.Core.Reflection
{
    public static class TypeExtensions
    {
        public static bool CanBeInstantiated(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return !type.IsAbstract && !type.IsInterface;
        }
    }
}
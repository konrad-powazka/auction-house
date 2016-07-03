using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AuctionHouse.Web.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace AuctionHouse.Web.Infrastructure
{
    public class CqrsControllersApplicationPart : ApplicationPart, IApplicationPartTypeProvider
    {
        // TODO: optimize
        private readonly IEnumerable<Type> _commandTypes;
        public override string Name { get; } = "CqrsControllersApplicationPart";

        public IEnumerable<TypeInfo> Types
            => _commandTypes.Select(t => typeof(CommandController<>).MakeGenericType(t).GetTypeInfo()).ToList();

        public CqrsControllersApplicationPart(IEnumerable<Type> commandTypes)
        {
            _commandTypes = commandTypes;
        }
    }
}
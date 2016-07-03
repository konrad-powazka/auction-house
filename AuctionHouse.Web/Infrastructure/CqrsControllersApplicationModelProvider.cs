using System.Linq;
using System.Reflection;
using AuctionHouse.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;

namespace AuctionHouse.Web.Infrastructure
{
    public class CqrsControllersApplicationModelProvider : DefaultApplicationModelProvider
    {
        public CqrsControllersApplicationModelProvider(IOptions<MvcOptions> mvcOptionsAccessor) : base(mvcOptionsAccessor)
        {
        }

        protected override ControllerModel CreateControllerModel(TypeInfo typeInfo)
        {
            var controllerModel = base.CreateControllerModel(typeInfo);

            if (controllerModel == null)
            {
                return null;
            }

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(CommandController<>) && typeInfo.IsConstructedGenericType)
            {
                controllerModel.ControllerName = typeInfo.GetGenericArguments().First().Name;
            }

            return controllerModel;
        }
    }
}
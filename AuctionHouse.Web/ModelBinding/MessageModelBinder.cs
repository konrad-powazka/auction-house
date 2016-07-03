using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuctionHouse.Web.ModelBinding
{
    public class MessageModelBinder : IModelBinder
    {
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly Dictionary<Type, IModelBinder> _commandTypeToBinderMapings;

        public MessageModelBinder(
            IModelMetadataProvider metadataProvider,
            Dictionary<Type, IModelBinder> commandTypeToBinderMapings)
        {
            _metadataProvider = metadataProvider;
            _commandTypeToBinderMapings = commandTypeToBinderMapings;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var messageTypeResult = bindingContext.ValueProvider.GetValue("commandName");
            if (messageTypeResult == ValueProviderResult.None)
            {
                bindingContext.Result = ModelBindingResult.Failed(bindingContext.ModelName);
                return;
            }

            // TODO: cleanup!
            var commandTypeKvp =
                _commandTypeToBinderMapings.SingleOrDefault(c => c.Key.Name.Contains(messageTypeResult.FirstValue));
            if (commandTypeKvp.Key == null)
            {
                bindingContext.Result = ModelBindingResult.Failed(bindingContext.ModelName);
                return;
            }

            // Now know the type exists in the assembly.
            var type = commandTypeKvp.Key;
            var metadata = _metadataProvider.GetMetadataForType(type);

            bindingContext.ModelMetadata = metadata;
            await commandTypeKvp.Value.BindModelAsync(bindingContext);
        }
    }
}
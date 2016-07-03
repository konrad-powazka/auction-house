using System;
using System.Collections.Generic;
using AuctionHouse.Application;
using AuctionHouse.Core.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AuctionHouse.Web.ModelBinding
{
    public class MessageModelBinderProvider : IModelBinderProvider
    {
        private readonly IEnumerable<Type> _commandTypes;

        public MessageModelBinderProvider(IEnumerable<Type> commandTypes)
        {
            if (commandTypes == null)
            {
                throw new ArgumentNullException(nameof(commandTypes));
            }

            _commandTypes = commandTypes;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType != typeof (ICommand))
            {
                return null;
            }

            var commandTypeToBinderMapings = new Dictionary<Type, IModelBinder>();

            foreach (var type in _commandTypes)
            {
                if (!type.CanBeInstantiated())
                {
                    continue;
                }

                var metadata = context.MetadataProvider.GetMetadataForType(type);

                if (context.BindingInfo != null)
                {
                    var mutableMetadata = (DefaultModelMetadata) metadata;
                    mutableMetadata.BindingMetadata.BindingSource = context.BindingInfo.BindingSource;
                }

                var binder = context.CreateBinder(metadata);
                commandTypeToBinderMapings.Add(type, binder);
            }

            return new MessageModelBinder(context.MetadataProvider, commandTypeToBinderMapings);
        }
    }
}
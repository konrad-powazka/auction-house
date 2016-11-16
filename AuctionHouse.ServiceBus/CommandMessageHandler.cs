using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouse.Messages.Auctions;
using NServiceBus;

namespace AuctionHouse.ServiceBus
{
    class CommandMessageHandler : IHandleMessages<object>
    {
        public async Task Handle(object message, IMessageHandlerContext context)
        {
            if (message is CreateAuctionCommand)
            {
                
            }
        }
    }
}
using System;

namespace AuctionHouse.Core.Messaging
{
    public interface ICommand : IMessage
    {
        Guid Id { get; }
    }
}
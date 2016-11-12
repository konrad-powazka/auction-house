using System;
using System.Web.Http;
using AuctionHouse.Application;

namespace AuctionHouse.Web.Controllers.Api
{
    public abstract class CommandController<TCommand> : ApiController where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        protected CommandController(ICommandHandler<TCommand> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _handler = handler;
        }

        [HttpPost]
        public void Handle(TCommand command)
        {
            _handler.Handle(command);
        }
    }
}
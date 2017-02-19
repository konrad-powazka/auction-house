using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AuctionHouse.Core.Messaging;
using AuctionHouse.Web.Cqrs;

namespace AuctionHouse.Web.Controllers.Api
{
    public abstract class CommandController<TCommand> : ApiController where TCommand : ICommand
    {
        private readonly ICommandQueue _commandQueue;

        protected CommandController(ICommandQueue commandQueue)
        {
            if (commandQueue == null)
            {
                throw new ArgumentNullException(nameof(commandQueue));
            }

            _commandQueue = commandQueue;
        }

        [HttpPost]
        [Authorize]
        public async Task<HttpResponseMessage> Handle([FromBody] TCommand command, [FromUri] Guid commandId)
        {
            await _commandQueue.QueueCommand(command, commandId, User.Identity.Name);

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
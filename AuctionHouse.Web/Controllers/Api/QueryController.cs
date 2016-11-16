using System;
using System.Web.Http;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Web.Controllers.Api
{
    public abstract class QueryController<TQuery, TQueryResult> : ApiController where TQuery : IQuery<TQueryResult>
    {
        private readonly IQueryHandler<TQuery, TQueryResult> _handler;

        protected QueryController(IQueryHandler<TQuery, TQueryResult> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _handler = handler;
        }

        [HttpGet]
        public TQueryResult Handle(TQuery query)
        {
            return _handler.Handle(query);
        }
    }
}
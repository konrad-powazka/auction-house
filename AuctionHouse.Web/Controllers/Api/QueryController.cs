using System;
using System.Threading.Tasks;
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
		public async Task<TQueryResult> Handle([FromUri] TQuery query)
		{
			var queryEnvelope = new QueryEnvelope<TQuery, TQueryResult>(query, User.Identity.Name);
			return await _handler.Handle(queryEnvelope);
		}
	}
}
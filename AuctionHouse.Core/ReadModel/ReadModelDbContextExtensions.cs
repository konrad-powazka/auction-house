using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionHouse.Core.ReadModel
{
	public static class ReadModelDbContextExtensions
	{
		public static async Task<TReadModel> Get<TReadModel>(this IReadModelDbContext readModelDbContext, Guid id)
			where TReadModel : class
		{
			var result = await readModelDbContext.TryGet<TReadModel>(id);

			if (result == null)
			{
				throw new KeyNotFoundException();
			}

			return result;
		}
	}
}
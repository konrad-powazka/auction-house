using System;
using System.Threading.Tasks;
using AuctionHouse.Core.Paging;
using Nest;

namespace AuctionHouse.Core
{
	public static class ElasticsearchExtensions
	{
		public static async Task<TPagedResult> RunPagedQuery
			<TPagedQuery, TPagedResult, TPagedResultItem, TElasticsearchDocument>(
			this IElasticClient elasticClient, TPagedQuery query,
			Func<QueryContainerDescriptor<TElasticsearchDocument>, QueryContainer> applyQueryFiltersFunc)
			where TPagedResult : IPagedResult<TPagedResultItem>, new()
			where TPagedQuery : IPagedQuery<TPagedResult, TPagedResultItem>
			where TElasticsearchDocument : class, TPagedResultItem
		{
			var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
			var firstItemIndex = (pageNumber - 1)*query.PageSize;

			var elasticResult =
				await
					elasticClient.SearchAsync<TElasticsearchDocument>(
						s =>
							s.Query(applyQueryFiltersFunc)
								.From(firstItemIndex)
								.Take(query.PageSize));

			return new TPagedResult
			{
				PageNumber = pageNumber,
				PageItems = elasticResult.Documents,
				TotalItemsCount = elasticResult.Total,
				TotalPagesCount = (elasticResult.Total + query.PageSize - 1)/query.PageSize,
				PageSize = query.PageSize
			};
		}
	}
}
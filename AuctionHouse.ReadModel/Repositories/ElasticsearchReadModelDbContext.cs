using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionHouse.Core.Collections;
using AuctionHouse.Core.ReadModel;
using Elasticsearch.Net;
using Nest;

namespace AuctionHouse.ReadModel.Repositories
{
	public class ElasticsearchReadModelDbContext : IReadModelDbContext
	{
		private readonly Dictionary<Type, Dictionary<Guid, ReadModelChange>> _changes =
			new Dictionary<Type, Dictionary<Guid, ReadModelChange>>();

		private readonly IElasticClient _elasticClient;

		public ElasticsearchReadModelDbContext(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient;
		}

		public void CreateOrOverwrite<TReadModel>(TReadModel readModel, Guid id) where TReadModel : class
		{
			var changesForReadModelType = _changes.GetOrAdd(typeof(TReadModel), () => new Dictionary<Guid, ReadModelChange>());
			ReadModelChange readModelChange;

			if (changesForReadModelType.TryGetValue(id, out readModelChange))
			{
				readModelChange.ReadModel = readModel;
			}
			else
			{
				readModelChange = new ReadModelChange<TReadModel>(ChangeType.CreateOrOverwrite, id, readModel);
				changesForReadModelType[id] = readModelChange;
			}
		}

		public async Task SaveChanges()
		{
			var bulkDescriptor = new BulkDescriptor();
			var changesDetected = false;

			foreach (var readModelTypeToChangesMapping in _changes)
			{
				foreach (var readModelIdToChangeMapping in readModelTypeToChangesMapping.Value)
				{
					var readModelChange = readModelIdToChangeMapping.Value;
					readModelChange.IncludeInBulkDescriptor(bulkDescriptor);
					changesDetected = true;
				}
			}

			if (changesDetected)
			{
				await _elasticClient.BulkAsync(bulkDescriptor.Refresh(Refresh.WaitFor));
				_changes.Clear();
			}
		}

		public async Task<TReadModel> TryGet<TReadModel>(Guid id) where TReadModel : class
		{
			var changesForReadModelType = _changes.GetOrAdd(typeof(TReadModel), () => new Dictionary<Guid, ReadModelChange>());
			ReadModelChange readModelChange;

			if (changesForReadModelType.TryGetValue(id, out readModelChange))
			{
				return (TReadModel) readModelChange.ReadModel;
			}

			var readModel = (await _elasticClient.GetAsync<TReadModel>(id)).Source;

			if (readModel == null)
			{
				return null;
			}

			readModelChange = new ReadModelChange<TReadModel>(ChangeType.CreateOrOverwrite, id, readModel);
			changesForReadModelType[id] = readModelChange;

			return readModel;
		}

		private abstract class ReadModelChange
		{
			protected ReadModelChange(ChangeType type, Guid readModelId, object readModel)
			{
				Type = type;
				ReadModelId = readModelId;
				ReadModel = readModel;
			}

			public ChangeType Type { get; }
			public Guid ReadModelId { get; }
			public object ReadModel { get; set; }
			public abstract void IncludeInBulkDescriptor(BulkDescriptor bulkDescriptor);
		}

		private class ReadModelChange<TReadModel> : ReadModelChange where TReadModel : class
		{
			public ReadModelChange(ChangeType type, Guid readModelId, TReadModel readModel) : base(type, readModelId, readModel)
			{
			}

			public override void IncludeInBulkDescriptor(BulkDescriptor bulkDescriptor)
			{
				if (Type == ChangeType.CreateOrOverwrite)
				{
					bulkDescriptor.Index<TReadModel>(d => d.Document((TReadModel) ReadModel).Id(ReadModelId));
				}
				else
				{
					throw new NotImplementedException();
				}
			}
		}

		// TODO: Delete will be needed too
		// TODO: Add unchanged
		private enum ChangeType
		{
			CreateOrOverwrite
		}
	}
}
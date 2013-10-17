using System.Collections.Generic;
using AutoPoco.Engine;
namespace WhippedCream.DataSources
{
	public class CollectionDataSource<TModel> : DatasourceBase<ICollection<TModel>>
	{
		public System.Random Random { get; private set; }
		public int Min { get; private set; }
		public int Max { get; private set; }

		public CollectionDataSource(int min, int max)
		{
			Random = new System.Random(0);
			Min = min;
			Max = max;
		}

		public override ICollection<TModel> Next(IGenerationContext context)
		{
			ICollectionContext<TModel, IList<TModel>> collection = context.List<TModel>(Random.Next(Min, Max));
			return collection.Get();
		}
	}
}
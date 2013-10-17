using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace WhippedCream.InMemoryDataLayer
{
	/// <summary>
	/// The Default Implementation of <see cref="IInMemoryPersistentMedium"/>.
	/// </summary>
	internal class InMemoryPersistentMedium : IInMemoryPersistentMedium
	{
		private class RepositoryCollection<TRepo, TType> : List<TType> { }

		/// <summary>
		/// Gets a list of all instances of in-memory collections that have been
		/// created so far.
		/// </summary>
		public IList<IList> Storage { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public InMemoryPersistentMedium() { Storage = new List<IList>(); }

		#region IInMemoryPersistentMedium Members

		IList<TType> IInMemoryPersistentMedium.GetStorage<TRepo, TType>()
		{
			//If the collection hasn't been created yet, create it.
			if (!Storage.Any(a => a is RepositoryCollection<TRepo, TType>))
				Storage.Add(new RepositoryCollection<TRepo, TType>());

			return Storage.First(a => a is RepositoryCollection<TRepo, TType>) as IList<TType>;
		}

		void IInMemoryPersistentMedium.ClearAll()
		{
			foreach (IList list in Storage)
			{
				list.Clear();
			}
		}

		#endregion
	}
}
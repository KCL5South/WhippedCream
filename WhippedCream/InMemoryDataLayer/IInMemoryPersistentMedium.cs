using System.Collections.Generic;
namespace WhippedCream.InMemoryDataLayer
{
	/// <summary>
	/// This object can be used to interact with entities stored in memory.
	/// </summary>
	public interface IInMemoryPersistentMedium
	{
		/// <summary>
		/// Call this method to retrieve an in memory collection of entities.
		/// </summary>
		/// <typeparam name="TRepo">The Repository the collection belongs to.  (The Database).</typeparam>
		/// <typeparam name="TType">The type of entity the collection contains.  (The Table).</typeparam>
		/// <returns>An <see cref="IList{T}"/> object that can be used to manipulate the in memory collection.</returns>
		IList<TType> GetStorage<TRepo, TType>();
		/// <summary>
		/// Clears all entities from all in memory collections.
		/// </summary>
		void ClearAll();
	}
}
namespace WhippedCream
{
	/// <summary>
	/// Context Repositories are capable of interacting with the data source 
	/// in specials ways.
	/// 
	/// Such as Adding objects to an entity set, deleting objects from an entity set, 
	/// and Loading Entity Properties to persist entity relationships.
	/// </summary>
	public interface IRepositoryContext
	{
		/// <summary>
		/// Adds an entity to the entity set designated by <paramref name="entitySetName"/>.
		/// </summary>
		/// <param name="entitySetName">The name of the Entity Set to append the object to.</param>
		/// <param name="entity">The entity to add.</param>
		void AddObject(string entitySetName, object entity);
		/// <summary>
		/// Deletes an object from the Entity Container.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		void DeleteObject(object entity);
		/// <summary>
		/// Persists changes to the underlying data store.
		/// </summary>
		void SaveChanges();
		/// <summary>
		/// Loads up entity relationships on <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity who's property represents a relationship to other entities.</param>
		/// <param name="propertyName">The property on <paramref name="entity"/> to load.</param>
		void LoadProperty(object entity, string propertyName);
	}
}
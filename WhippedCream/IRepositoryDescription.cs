using System.Collections.Generic;
namespace WhippedCream
{
	/// <summary>
	/// A repository description is used by Whipped Cream to cache up the
	/// information about the repository so that it can more easily act
	/// on those repositories.
	/// </summary>
	public interface IRepositoryDescription
	{
		/// <summary>
		/// Gets the <see cref="System.Type"/> of the Repository.
		/// </summary>
		System.Type RepositoryType { get; }
		/// <summary>
		/// Gets a Read Only map of <see cref="DataLayerState"/> and a <see cref="System.Type"/>.
		/// These maps represent the repository implmenetations that will be generated for the 
		/// given state of Whipped Cream.
		/// </summary>
		IDictionary<DataLayerState, System.Type> Repositories { get; }
		/// <summary>
		/// Gets a Read Only map of <see cref="DataLayerState"/> and a <see cref="System.Type"/>.
		/// These maps represent the repository context implmenetations that will be generated for the 
		/// given state of Whipped Cream.
		/// </summary>
		IDictionary<DataLayerState, System.Type> RepositoryContexts { get; }
		/// <summary>
		/// Gets the path that will be used for the WCF Data Service used to service this repository.
		/// </summary>
		string ServicePath { get; }
		/// <summary>
		/// Gets the <see cref="System.Type"/> used for the WCF Data Service that will service this repository.
		/// </summary>
		System.Type ServiceType { get; }
	}
}

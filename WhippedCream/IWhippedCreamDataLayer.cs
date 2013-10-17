using System.ServiceModel;
namespace WhippedCream
{
	/// <summary>
	/// The Whipped Cream Data Layer service is the root object used
	/// to interact with Whipped Cream.
	/// </summary>
	public interface IWhippedCreamDataLayer
	{
		/// <summary>
		/// Gets or sets the 'State' of Whipped Cream.
		/// </summary>
		DataLayerState State { get; set; }
		/// <summary>
		/// Gets a repository specific Url.
		/// </summary>
		/// <typeparam name="TRepo">The repository to resolve for a service url.</typeparam>
		/// <returns>A repository specific url.</returns>
		string GetServiceUrl<TRepo>();
		/// <summary>
		/// Gets a repository specific Uri.
		/// </summary>
		/// <typeparam name="TRepo">The repository to resolve for a service uri.</typeparam>
		/// <returns>A repository specific uri.</returns>
		System.Uri GetServiceUri<TRepo>();
		/// <summary>
		/// Gets a repository specific <see cref="ServiceHost"/> object that can be used to setup a service 
		/// for a repository.
		/// </summary>
		/// <typeparam name="TRepo">The repository to generate the <see cref="ServiceHost"/> for.</typeparam>
		/// <returns>A repository specific <see cref="ServiceHost"/>.</returns>
		ServiceHost GetServiceHost<TRepo>();
		/// <summary>
		/// Gets an instance of <see cref="IRepositoryDescription"/> that describes the repository passed as 
		/// 'TRepo'.
		/// </summary>
		/// <returns> An instance of <see cref="IRepositoryDescription" />.</returns>
		IRepositoryDescription GetRepositoryDescription<TRepo>();
	}
}
namespace WhippedCream
{
	/// <summary>
	/// Inheritors of this interface should be able to create
	/// specific implementations of repositories for a given state
	/// of the data layer.
	/// </summary>
	public interface IRepositoryContextFactory
	{
		/// <summary>
		/// Generates a new Repository Context.
		/// </summary>
		/// <typeparam name="TRepo">The type of the repository context to generate.</typeparam>
		/// <returns>An instance of <typeparamref name="TRepo"/> or null if a <see cref="RepositoryContextAttribute"/>
		/// isn't found that matches the current state of Whipped Cream.</returns>
		TRepo CreateRepository<TRepo>() where TRepo : class, IRepository;
	}
}
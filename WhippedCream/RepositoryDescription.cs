namespace WhippedCream
{
	/// <summary>
	/// This is the default implementation of <see cref="IRepositoryDescription"/> used by WhippedCream.
	/// </summary>
	/// <typeparam name="TRepo">The type of the repository this describes.</typeparam>
	internal class RepositoryDescription<TRepo> : IRepositoryDescription
	{
		private System.Type _repositoryType;
		private System.Collections.Generic.IDictionary<DataLayerState, System.Type> _repositories;
		private System.Collections.Generic.IDictionary<DataLayerState, System.Type> _repositoryContexts;
		private System.Type _serviceType;
		private string _servicePath;

		/// <summary>
		/// Constructor.
		/// </summary>
		public RepositoryDescription()
		{
			_repositoryType = typeof(TRepo);
			_repositories = RepositoryAttribute.GetRepositories<TRepo>();
			_repositoryContexts = RepositoryContextAttribute.GetRepositoryContexts<TRepo>();
			_serviceType = RepositoryServiceTypeAttribute.GetType<TRepo>();
			_servicePath = RepositoryServicePathAttribute.GetPath<TRepo>();
		}

		#region IRepositoryDescription Members

		System.Type IRepositoryDescription.RepositoryType { get { return _repositoryType; } }

		System.Collections.Generic.IDictionary<DataLayerState, System.Type> IRepositoryDescription.Repositories { get { return _repositories; } }

		System.Collections.Generic.IDictionary<DataLayerState, System.Type> IRepositoryDescription.RepositoryContexts { get { return _repositoryContexts; } }

		System.Type IRepositoryDescription.ServiceType { get { return _serviceType; } }

		string IRepositoryDescription.ServicePath { get { return _servicePath; } }

		#endregion
	}
}

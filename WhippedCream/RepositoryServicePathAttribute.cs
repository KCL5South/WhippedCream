using System;
using System.Linq;

namespace WhippedCream
{
	/// <summary>
	/// Decorate your repository with this attribute to specify the path to use when Whipped Cream generates a 
	/// service url/uri for the repository.
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public class RepositoryServicePathAttribute : Attribute
	{
		/// <summary>
		/// This method queries a repository for an instances of <see cref="RepositoryServicePathAttribute"/>
		/// and returns the value of <see cref="P:ServicePath"/> if one is found.  Otherwise null is returned.
		/// </summary>
		/// <typeparam name="TRepo">The type of repository to query.</typeparam>
		/// <returns>The <see cref="P:ServicePath"/> of the attribute if it is found, otherwise null.</returns>
		public static string GetPath<TRepo>()
		{
			Type repoType = typeof(TRepo);
			if(repoType.IsDefined(typeof(RepositoryServicePathAttribute), false))
			{
				var attribute = repoType.GetCustomAttributes(typeof(RepositoryServicePathAttribute), false).First()
					as RepositoryServicePathAttribute;

				return attribute.ServicePath;
			}

			return null;
		}

		/// <summary>
		/// Gets the path to use when Whipped Cream creates a url/uri for the repository.
		/// </summary>
		public string ServicePath { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="servicePath">The path that whipped cream will use when generator a url/uri for the repository.</param>
		public RepositoryServicePathAttribute(string servicePath)
		{
			ServicePath = servicePath;
		}
	}
}

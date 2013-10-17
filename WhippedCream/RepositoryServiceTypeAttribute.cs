using System;
using System.Linq;

namespace WhippedCream
{
	/// <summary>
	/// Decorate your repository with this attribute to designate a type that will be used when
	/// Whipped Cream generates a service for your repository.
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public class RepositoryServiceTypeAttribute : Attribute
	{
		/// <summary>
		/// This method queries a repository for an instances of <see cref="RepositoryServiceTypeAttribute"/>
		/// and returns the value of <see cref="P:ServiceType"/> if one is found.  Otherwise null is returned.
		/// </summary>
		/// <typeparam name="TRepo">The type of repository to query.</typeparam>
		/// <returns>The <see cref="P:ServiceType"/> of the attribute if it is found, otherwise null.</returns>
		public static System.Type GetType<TRepo>()
		{
			Type repoType = typeof(TRepo);
			if (repoType.IsDefined(typeof(RepositoryServiceTypeAttribute), false))
			{
				var attribute = repoType.GetCustomAttributes(typeof(RepositoryServiceTypeAttribute), false).First()
					as RepositoryServiceTypeAttribute;

				return attribute.ServiceType;
			}

			return null;
		}

		/// <summary>
		/// Gets the service type used when Whipped Cream generates a service for your repository.
		/// </summary>
		public System.Type ServiceType { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="serviceType">The service type used when Whipped Cream generates a service for your repository.</param>
		public RepositoryServiceTypeAttribute(System.Type serviceType)
		{
			ServiceType = serviceType;
		}
	}
}

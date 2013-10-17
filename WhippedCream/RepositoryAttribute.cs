﻿using System.Collections.Generic;
using System.Linq;
namespace WhippedCream
{
	/// <summary>
	/// Decorate your interface with this attribute to designate implementations of your repository
	/// that will be generated by Whipped Cream.
	/// </summary>
    [System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class RepositoryAttribute : System.Attribute
    {
		/// <summary>
		///		<para>
		///			This method queries the type specified by <typeparamref name="TInterface"/> for instances
		///			of <see cref="RepositoryAttribute"/> and returns a mapping based on the results.
		///		</para>
		/// 
		///		<para>
		///			There can be multiple instances of <see cref="RepositoryAttribute"/> on your repository.
		///			However, if there are multiple <see cref="RepositoryAttribute"/> attributes with the same
		///			value for <see cref="P:State"/>, only the first one found is returned in the mapping.
		///		</para>
		/// </summary>
		/// <typeparam name="TInterface">The Repository to query.</typeparam>
		/// <returns>A mapping of states and their repository implementation types.</returns>
        public static IDictionary<DataLayerState, System.Type> GetRepositories<TInterface>()
        {
            Dictionary<DataLayerState, System.Type> result = new Dictionary<DataLayerState, System.Type>();
            System.Type targetType = typeof(TInterface);
            if (targetType.IsDefined(typeof(RepositoryAttribute), false))
            {
                IEnumerable<RepositoryAttribute> attributes = 
                    targetType.GetCustomAttributes(typeof(RepositoryAttribute), false).Cast<RepositoryAttribute>();
                var desiredAttributes = 
                    from att in 
                        (from a in attributes
                        group a by a.State into attGroup
                        select attGroup)
                    select att.First();
                foreach(var att in desiredAttributes)
                {
                    result.Add(att.State, att.RepositoryType);
                }
            }

            return result;
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="state">A <see cref="DataLayerState"/>.</param>
		/// <param name="repositoryType">A <see cref="System.Type"/> representing the 
		/// repository implementation that will be generated for the given <paramref name="state"/>.</param>
        public RepositoryAttribute(DataLayerState state, System.Type repositoryType)
        {
            State = state;
            RepositoryType = repositoryType;
        }

		/// <summary>
		/// Gets the <see cref="DataLayerState"/> value.
		/// </summary>
        public DataLayerState State { get; private set; }
		/// <summary>
		/// Gets the <see cref="System.Type"/> value.
		/// </summary>
        public System.Type RepositoryType { get; private set; }
    }
}
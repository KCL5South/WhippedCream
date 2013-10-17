using System.Data.Services;
using System.Reflection;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// WCF Data Services has a provider that uses reflection in order to find the Entity Sets and Operations
	/// your repository exposes.  In order for that provider to manage entity relationships, the data service 
	/// the provider is reflecting over needs to inherit from <see cref="IUpdatable"/>.  If it does not, then 
	/// entity relationships are not managed.
	/// 
	///		<remarks>
	///			I actually don't know exactly what happens if the service dosen't inherit from <see cref="IUpdatable"/>
	///			but everything works when I do.
	///		</remarks>
	/// </summary>
	public abstract class WhippedCreamUpdatableContext : IUpdatable
	{
		/// <summary>
		/// An entity can either have been Inserted or Deleted when tracking changes.
		/// These values are used to help keep track of those changes.
		/// </summary>
		public enum ODataOperation
		{
			/// <summary>
			/// Represents an Insert or Add operation.
			/// </summary>
			Insert,
			/// <summary>
			/// Represents a Delete operation.
			/// </summary>
			Delete
		}

		/// <summary>
		/// Gets the IOC Container used for dependency injection.
		/// </summary>
		public IUnityContainer Container { get; private set; }
		/// <summary>
		/// Gets a collection of operation, entity pairs that represent the pending changes on 
		/// entity sets.
		/// </summary>
		public ICollection<System.Tuple<ODataOperation, object>> PendingChanges { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="container">The IOC Container used for dependency injection.</param>
		public WhippedCreamUpdatableContext(IUnityContainer container)
		{
			if (container == null)
				throw new System.ArgumentNullException("container");

			Container = container;
			PendingChanges = new List<System.Tuple<ODataOperation, object>>();
		}

		/// <summary>
		/// A Utility method used to retrieve a <see cref="System.Type"/> from a type name.
		/// </summary>
		/// <param name="fullTypeName">The full type name.</param>
		/// <returns>A <see cref="System.Type"/> generated from the provider type name.</returns>
		public System.Type GetType(string fullTypeName) { return GetType(null, fullTypeName); }
		/// <summary>
		/// A Utility method used to retrieve a <see cref="System.Type"/> from a type name.
		/// </summary>
		/// <param name="containerName">The name of the entity set that is serving the type.</param>
		/// <param name="fullTypeName">The full type name.</param>
		/// <returns>A <see cref="System.Type"/> generated from the provider type name.</returns>
		public abstract System.Type GetType(string containerName, string fullTypeName);
		/// <summary>
		/// This method is called when pending changes on an entity need to be persisted to the backing store.
		/// </summary>
		/// <param name="operation">The type of operation the save is envoking.</param>
		/// <param name="entity">The entity to perform the save on.</param>
		public abstract void SaveEntity(ODataOperation operation, object entity);

		#region IUpdatable Members

		void IUpdatable.AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
		{
			if (targetResource == null)
				throw new System.ArgumentNullException("targetResource");
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new System.ArgumentException("Must not be null, empty, or only whitespace.", "propertyName");
			if (resourceToBeAdded == null)
				throw new System.ArgumentNullException("resourceToBeAdded");

			PropertyInfo pi = null;
			if ((pi = targetResource.GetType().GetProperty(propertyName)) == null)
				throw new UnknownPropertyException(propertyName, targetResource.GetType());
			if (!pi.PropertyType.GetInterfaces().Any(a => a == typeof(IList)))
				throw new PropertyTypeMustBeACollectionException(pi.PropertyType);
			
			IList collection = (IList)pi.GetValue(targetResource, null);
			collection.Add(resourceToBeAdded);
		}

		void IUpdatable.ClearChanges()
		{
			PendingChanges.Clear();	
		}

		object IUpdatable.CreateResource(string containerName, string fullTypeName)
		{
			if(string.IsNullOrWhiteSpace(fullTypeName))
				throw new System.ArgumentException("Must not be null, empty, or only whitespace.", "fullTypeName");

			System.Type targetType = GetType(containerName, fullTypeName);
			var target = Container.Resolve(targetType);
			PendingChanges.Add(new System.Tuple<ODataOperation, object>(ODataOperation.Insert, target));
			return target;
		}

		void IUpdatable.DeleteResource(object targetResource)
		{
			if(targetResource == null)
				throw new System.ArgumentNullException("targetResource");

			PendingChanges.Add(new System.Tuple<ODataOperation, object>(ODataOperation.Delete, targetResource));
		}

		object IUpdatable.GetResource(System.Linq.IQueryable query, string fullTypeName)
		{
			if(query == null)
				throw new System.ArgumentNullException("query");

			return query.Cast<object>().FirstOrDefault();
		}

		object IUpdatable.GetValue(object targetResource, string propertyName)
		{
			if(targetResource == null)
				throw new System.ArgumentNullException("targetResource");
			if(string.IsNullOrWhiteSpace(propertyName))
				throw new System.ArgumentException("Must not be null, empty, or only whitespace.", "propertyName");

			PropertyInfo info = targetResource.GetType().GetProperty(propertyName);
			if(info == null)
				throw new UnknownPropertyException(propertyName, targetResource.GetType());

			return info.GetValue(targetResource, null);
		}

		void IUpdatable.RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
		{
			if (targetResource == null)
				throw new System.ArgumentNullException("targetResource");
			if (string.IsNullOrWhiteSpace(propertyName))
				throw new System.ArgumentException("Must not be null, empty, or only whitespace.", "propertyName");
			if (resourceToBeRemoved == null)
				throw new System.ArgumentNullException("resourceToBeAdded");

			PropertyInfo pi = null;
			if ((pi = targetResource.GetType().GetProperty(propertyName)) == null)
				throw new UnknownPropertyException(propertyName, targetResource.GetType());
			if (!pi.PropertyType.GetInterfaces().Any(a => a == typeof(IList)))
				throw new PropertyTypeMustBeACollectionException(pi.PropertyType);
			
			IList collection = (IList)pi.GetValue(targetResource, null);
			collection.Remove(resourceToBeRemoved);
		}

		object IUpdatable.ResetResource(object resource)
		{
			return resource;
		}

		object IUpdatable.ResolveResource(object resource)
		{
			return resource;
		}

		void IUpdatable.SaveChanges()
		{
			foreach(var tuple in PendingChanges)
				SaveEntity(tuple.Item1, tuple.Item2);

			PendingChanges.Clear();
		}

		void IUpdatable.SetReference(object targetResource, string propertyName, object propertyValue)
		{
			(this as IUpdatable).SetValue(targetResource, propertyName, propertyValue);
		}

		void IUpdatable.SetValue(object targetResource, string propertyName, object propertyValue)
		{
			if(targetResource == null)
				throw new System.ArgumentNullException("targetResource");
			if(string.IsNullOrWhiteSpace(propertyName))
				throw new System.ArgumentException("Must not be null, empty, or only whitespace.", "propertyName");

			PropertyInfo info = targetResource.GetType().GetProperty(propertyName);
			if(info == null)
				throw new UnknownPropertyException(propertyName, targetResource.GetType());

			info.SetValue(targetResource, propertyValue, null);
		}

		#endregion
	}

}

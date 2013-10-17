using System;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// Whipped Cream uses this object so it can use Dependency Injection when creating instances of services hosts.
	/// </summary>
	internal class DataServiceDependencyInstanceProvider : IInstanceProvider
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="container">The IOC container to use for dependency injection.</param>
		/// <param name="serviceType">The type of host to create.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="container"/> or <paramref name="serviceType"/> are null.</exception>
		public DataServiceDependencyInstanceProvider(IUnityContainer container, Type serviceType)
		{
			if(container == null)
				throw new System.ArgumentNullException("container");
			if(serviceType == null)
				throw new System.ArgumentNullException("serviceType");
			Container = container;
			ServiceType = serviceType;
		}

		/// <summary>
		/// Gets the IOC container used for dependency injection.
		/// </summary>
		public IUnityContainer Container { get; private set; }
		/// <summary>
		/// Gets the type of instance to provide.
		/// </summary>
		public Type ServiceType { get; private set; }

		#region IInstanceProvider Members

		object IInstanceProvider.GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
		{
			return Container.Resolve(ServiceType);
		}

		object IInstanceProvider.GetInstance(System.ServiceModel.InstanceContext instanceContext)
		{
			return (this as IInstanceProvider).GetInstance(instanceContext, null);
		}

		void IInstanceProvider.ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance) { }				

		#endregion
	}
}

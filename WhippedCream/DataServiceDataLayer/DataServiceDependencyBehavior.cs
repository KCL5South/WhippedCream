using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// This implementation of <see cref="IServiceBehavior"/> is used by Whipped Cream to allow for
	/// dependency injection when creating service hosts.
	/// </summary>
	internal class DataServiceDependencyBehavior : IServiceBehavior
	{
		/// <summary>
		/// Gets the IOC Container used for dependency injection.
		/// </summary>
		public IUnityContainer Container { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="container">The IOC Container used for dependency injection.</param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="container"/> is null.</exception>
		public DataServiceDependencyBehavior(IUnityContainer container)
		{
			if (container == null)
				throw new System.ArgumentNullException("container");

			Container = container;
		}

		#region IServiceBehavior Members

		void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }

		void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
		{
			foreach (var cdb in serviceHostBase.ChannelDispatchers)
			{
				var cd = cdb as System.ServiceModel.Dispatcher.ChannelDispatcher;

				if (cd != null)
				{
					foreach (var ed in cd.Endpoints)
					{
						ed.DispatchRuntime.InstanceProvider = Container.Resolve<DataServiceDependencyInstanceProvider>(new DependencyOverride(typeof(System.Type), new InjectionParameter(serviceDescription.ServiceType)));
					}
				}
			}
		}

		void IServiceBehavior.Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase) { }

		#endregion
	}
}
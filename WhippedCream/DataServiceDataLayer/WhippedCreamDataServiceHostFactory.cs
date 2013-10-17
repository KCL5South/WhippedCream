using Microsoft.Practices.Unity;
namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// Whipped Cream uses this Host factory to generate service hosts with dependency injection.
	/// </summary>
	public class WhippedCreamDataServiceHostFactory : System.Data.Services.DataServiceHostFactory
	{
		/// <summary>
		/// Gets the IOC Container used for dependency injection.
		/// </summary>
		public IUnityContainer Container { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="container">The IOC container used for dependency injection.</param>
		public WhippedCreamDataServiceHostFactory(IUnityContainer container)
		{
			if (container == null)
				throw new System.ArgumentNullException("container");

			Container = container;
		}

		/// <summary>
		/// Overriden from <see cref="System.Data.Services.DataServiceHostFactory"/>.
		/// </summary>
		/// <param name="serviceType">The type of host to create.</param>
		/// <param name="baseAddresses">The base addresses to supply to the host.</param>
		/// <returns>An instance of <see cref="System.ServiceModel.ServiceHost"/>.</returns>
		protected override System.ServiceModel.ServiceHost CreateServiceHost(System.Type serviceType, System.Uri[] baseAddresses)
		{
			return Container.Resolve<WhippedCreamDataServiceHost>(new DependencyOverride(typeof(System.Type), new InjectionParameter(serviceType)),
																  new DependencyOverride<System.Uri[]>(baseAddresses));
		}
	}
}
using System;
using Microsoft.Practices.Unity;
using System.Data.Services;

namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// This is the service host that Whipped Cream uses in order to allow dependency injection within
	/// WCF Data Services.
	/// </summary>
	public class WhippedCreamDataServiceHost : DataServiceHost
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="container">The IOC container used for dependency injection.</param>
		/// <param name="serviceType">The type of service to host.</param>
		/// <param name="baseAddresses">The addresses to host the service on.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="container"/> is null.</exception>
		public WhippedCreamDataServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			if(container == null)
				throw new ArgumentNullException("container");

			Container = container;
		}

		/// <summary>
		/// Gets the IOC Container used for dependency injection.
		/// </summary>
		public IUnityContainer Container { get; private set; }

		/// <summary>
		/// Overriden from <see cref="DataServiceHost"/>.
		/// </summary>
		/// <param name="timeout">The amount of time to wait for the open to complete.</param>
		protected override void OnOpen(TimeSpan timeout)
		{
			//This test is untestable because Microsoft has encapulated everything out of site
			//of us.
			this.Description.Behaviors.Add(Container.Resolve<DataServiceDependencyBehavior>());
			base.OnOpen(timeout);
		}
	}
}

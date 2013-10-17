using NUnit.Framework;
using Moq;
using Microsoft.Practices.Unity;

namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class DataServiceDependencyBehaviorTests
	{
		#region TestingDependencies

		private class DummyChannelDispatcher : System.ServiceModel.Dispatcher.ChannelDispatcher
		{
			public DummyChannelDispatcher() : base(Mock.Of<System.ServiceModel.Channels.IChannelListener>())
			{
				Endpoints.Add(
					new System.ServiceModel.Dispatcher.EndpointDispatcher(
						new System.ServiceModel.EndpointAddress(new System.Uri("http://www.google.com")),
						"TestContract",
						"TestNamespace"));
			}
		}

		private class DummyServiceHost : System.ServiceModel.ServiceHostBase
		{
			public DummyServiceHost()
			{
				ChannelDispatchers.Add(new DummyChannelDispatcher());
			}

			protected override System.ServiceModel.Description.ServiceDescription CreateDescription(out System.Collections.Generic.IDictionary<string, System.ServiceModel.Description.ContractDescription> implementedContracts)
			{
				throw new System.NotImplementedException();
			}
		}

		#endregion

		[Test]
		public void Constructor_ContainerNull()
		{
			try
			{
				new DataServiceDependencyBehavior(null);
				Assert.Fail(@"
We expect a System.ArguementException if the container is null.
");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void Constructor_ContainerSet()
		{
			IUnityContainer container = Mock.Of<IUnityContainer>();
			var target = new DataServiceDependencyBehavior(container);

			Assert.AreEqual(container, target.Container, @"
The Container property was not set during construction.
");
		}

		[Test]
		public void ApplyDispatchBehavior_AppliesCorrectInstanceProvider()
		{
			Mock<IUnityContainer> containerMock = new Mock<IUnityContainer>();
			DataServiceDependencyInstanceProvider expectedResult = new DataServiceDependencyInstanceProvider(containerMock.Object, typeof(int));
			containerMock.Setup(a => a.Resolve(typeof(DataServiceDependencyInstanceProvider), null, It.IsAny<ResolverOverride[]>())).Returns(expectedResult);
			System.ServiceModel.Description.IServiceBehavior target = new DataServiceDependencyBehavior(containerMock.Object);
			var host = new DummyServiceHost();
			var serviceDescription = new System.ServiceModel.Description.ServiceDescription();
			serviceDescription.ServiceType = typeof(int);

			target.ApplyDispatchBehavior(serviceDescription, host);

			foreach (var cd in host.ChannelDispatchers)
			{
				foreach (var ep in (cd as DummyChannelDispatcher).Endpoints)
				{
					var instanceProvider = (host.ChannelDispatchers[0] as DummyChannelDispatcher).Endpoints[0].DispatchRuntime.InstanceProvider;

					Assert.IsNotNull(instanceProvider, @"
The instanceProvider should not be null because the behavior should have set it up.
");
					Assert.AreEqual(expectedResult, instanceProvider, @"
The instance provider is an instance of an object that wasn't expected.
");
				}
			}
		}
	}
}
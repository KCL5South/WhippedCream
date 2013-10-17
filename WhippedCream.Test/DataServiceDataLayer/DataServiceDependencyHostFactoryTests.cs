using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class DataServiceDependencyHostFactoryTests
	{
		#region Testing Dependencies

		private class DummyFactory : WhippedCreamDataServiceHostFactory
		{
			public DummyFactory(IUnityContainer container) : base(container) { }

			public System.ServiceModel.ServiceHost CallProtectedCreateServiceHost(System.Type type, params System.Uri[] baseAddresses)
			{
				return CreateServiceHost(type, baseAddresses);
			}
		}

		#endregion

		[Test]
		public void Constructor_NullContainer()
		{
			try
			{
				new WhippedCreamDataServiceHostFactory(null);
				Assert.Fail("An ArgumentNullException was expected when passing null in for the container.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void Constructor_ContainerSet()
		{
			IUnityContainer container = Mock.Of<IUnityContainer>();
			var target = new WhippedCreamDataServiceHostFactory(container);

			Assert.AreEqual(container, target.Container, @"
The Container property was not set correctly.
");
		}

		[Test]
		public void CreateServiceHost_WhippedCreamServiceHostReturned()
		{
			IUnityContainer container = new UnityContainer();
			DummyFactory factory = new DummyFactory(container);

			var result = factory.CallProtectedCreateServiceHost(typeof(string), new System.Uri("http://localhost0978"));

			Assert.IsNotNull(result, "The result should not be null.");
			Assert.IsInstanceOf<WhippedCreamDataServiceHost>(result);

			Assert.AreEqual(typeof(string), result.Description.ServiceType,
							"The type of the service is not what was passed in.");
			Assert.AreEqual(1, result.BaseAddresses.Count);
			Assert.AreEqual(new System.Uri("http://localhost0978"), result.BaseAddresses[0],
							"The base address is not what was passed.");
		}
	}
}
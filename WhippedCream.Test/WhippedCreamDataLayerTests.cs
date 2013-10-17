using Microsoft.Practices.Unity;
using NUnit.Framework;
using System.Linq;
using WhippedCream.EntityFrameworkDataLayer;
using Moq;
using System.Collections.Generic;
using WhippedCream.InMemoryDataLayer;
using WhippedCream.DataServiceDataLayer;
using System.Web.Routing;
using System.ServiceModel.Activation;
namespace WhippedCream
{
	[TestFixture]
	public class WhippedCreamDataLayerTests
	{
		#region TestingDependencies

		public class DisposableObject { public void Dispose() { } }

		[Repository(DataLayerState.Live, typeof(LiveITestRepositoryOne))]
		[Repository(DataLayerState.Testing, typeof(TestingITestRepositoryOne))]
		[RepositoryContext(DataLayerState.Live, typeof(LiveITestRepositoryOne))]
		[RepositoryContext(DataLayerState.Testing, typeof(TestingITestRepositoryOne))]
		[RepositoryServicePath("TestRepositoryPath")]
		[RepositoryServiceType(typeof(string))]
		public interface ITestRepositoryOne : IRepository
		{ }

		public interface ITestRepositoryTwo : IRepository { }

		public class LiveITestRepositoryOne : DisposableObject, ITestRepositoryOne { }
		public class TestingITestRepositoryOne : DisposableObject, ITestRepositoryOne { }

		[Repository(DataLayerState.Testing, typeof(TestingITestRepositoryNoLiveReg))]
		public interface ITestRepositoryNoLiveReg : IRepository { }
		public class TestingITestRepositoryNoLiveReg : DisposableObject, ITestRepositoryNoLiveReg { }

		[Repository(DataLayerState.Live, typeof(TestingITestRepositoryNoTestingReg))]
		public interface ITestRepositoryNoTestingReg : IRepository { }
		public class TestingITestRepositoryNoTestingReg : DisposableObject, ITestRepositoryNoTestingReg { }

		#endregion

		[Test]
		public void Bootstrap_ContainerSet_Null()
		{
			try
			{
				WhippedCreamDataLayer.Bootstrap(null, new System.Uri("http://www.google.com"));
				Assert.Fail("We expect a System.ArgumentNullException because we passed null in for the container.");
			}
			catch (System.ArgumentNullException) { }
		}
		[Test]
		public void Bootstrap_BaseUri_Null()
		{
			try
			{
				WhippedCreamDataLayer.Bootstrap(Mock.Of<IUnityContainer>(), null);
				Assert.Fail("We expect a System.ArgumentNullException because we passed null in for the base uri.");
			}
			catch (System.ArgumentNullException) { }
		}
		[Test]
		public void Bootstrap_ContainerSet()
		{
			IUnityContainer container = new UnityContainer();
			WhippedCreamDataLayer.Bootstrap(container, new System.Uri("http://www.google.com"));

			Assert.AreEqual(container, WhippedCreamDataLayer.Container, "The Container Static property should have been set to the container that was passed to the bootstrap method.");
		}
		[Test]
		public void Boostrap_UrlServiceIsRegistered()
		{
			IUnityContainer container = new UnityContainer();
			WhippedCreamDataLayer.Bootstrap(container, new System.Uri("http://www.google.com"));

			Assert.IsTrue(container.IsRegistered<IUrlService>(), @"
If the user passes in a base uri, then we need to generate the IUrlService for them and make sure
it is registered in the container.
");
		}
		[Test]
		[TestCase(typeof(IWhippedCreamDataLayer), TestName = "Bootstrap_ServicesRegistered_IWhippedCreamDataLayer")]
		[TestCase(typeof(IRepositoryFactory), TestName = "Bootstrap_ServicesRegistered_IRepositoryFactory")]
		[TestCase(typeof(IRepositoryContextFactory), TestName = "Bootstrap_ServicesRegistered_IRepositoryContextFactory")]
		[TestCase(typeof(IInMemoryPersistentMedium), TestName = "Bootstrap_ServicesRegistered_IInMemoryPersistentMedium")]
		[TestCase(typeof(IUrlService), TestName = "Bootstrap_ServicesRegistered_IUrlService")]
		public void Bootstrap_ServicesRegistered(System.Type targetType)
		{
			IUnityContainer container = new UnityContainer();
			WhippedCreamDataLayer.Bootstrap(container, new System.Uri("http://www.google.com"));

			Assert.IsTrue(container.IsRegistered(targetType), "The type ({0}) was not registered when it was expected to be.", targetType);
		}
		[Test]
		[TestCase(typeof(IWhippedCreamDataLayer), TestName = "Bootstrap_ServicesCanResolve_IWhippedCreamDataLayer")]
		[TestCase(typeof(IRepositoryFactory), TestName = "Bootstrap_ServicesCanResolve_IRepositoryFactory")]
		[TestCase(typeof(IRepositoryContextFactory), TestName = "Bootstrap_ServicesCanResolve_IRepositoryContextFactory")]
		[TestCase(typeof(IInMemoryPersistentMedium), TestName = "Bootstrap_ServicesCanResolve_IInMemoryPersistentMedium")]
		[TestCase(typeof(IUrlService), TestName = "Bootstrap_ServicesCanResolve_IUrlService")]
		public void Bootstrap_ServicesCanResolve(System.Type targetType)
		{
			IUnityContainer container = new UnityContainer();
			WhippedCreamDataLayer.Bootstrap(container, new System.Uri("http://www.google.com"));

			Assert.IsNotNull(container.Resolve(targetType), "The type ({0}) could not be resolved from the container.", targetType);
		}
		[Test]
		public void Constructor_StateIsSetToLive()
		{
			IWhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			Assert.AreEqual(DataLayerState.Live, dl.State, "The initial state of the Data layer should be set to Live.");
		}
		[Test]
		public void RegisterRepository_RepositoryDescriptionAdded()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			dl.RegisterRepository<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}
		[Test]
		public void GetServiceUrl_RepositoryDescriptionsAdded()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			wdl.GetServiceUrl<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}
		[Test]
		public void GetServiceUri_RepositoryDescriptionsAdded()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			wdl.GetServiceUri<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}
		[Test]
		public void GetServiceUrl()
		{
			System.Uri result = new System.Uri("http://www.asdfasdf.com");
			Mock<IUrlService> urlServiceMock = new Mock<IUrlService>();
			urlServiceMock.Setup(a => a.GetServiceUrl("TestRepositoryPath")).Returns("ExpectedResult");
			WhippedCreamDataLayer.Container = new UnityContainer();
			WhippedCreamDataLayer.Container.RegisterInstance<IUrlService>(urlServiceMock.Object);
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;

			Assert.AreEqual("ExpectedResult", wdl.GetServiceUrl<ITestRepositoryOne>(), @"
The result of the GetServiceUrl is not what was expected.
");
		}
		[Test]
		public void GetServiceUri()
		{
			System.Uri result = new System.Uri("http://www.asdfasdf.com");
			Mock<IUrlService> urlServiceMock = new Mock<IUrlService>();
			urlServiceMock.Setup(a => a.GetServiceUri("TestRepositoryPath")).Returns(result);
			WhippedCreamDataLayer.Container = new UnityContainer();
			WhippedCreamDataLayer.Container.RegisterInstance<IUrlService>(urlServiceMock.Object);
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;

			Assert.AreEqual(result, wdl.GetServiceUri<ITestRepositoryOne>(), @"
The result of the GetServiceUri is not what was expected.
");
		}
		[Test]
		public void CreateRepository_RepositoryDescriptionsAdded()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IRepositoryFactory rf = dl as IRepositoryFactory;

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			var repo = rf.CreateRepository<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}
		[Test]
		public void CreateRepository_UnregisteredRepository_Live_ExpectNull()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryFactory rf = dl as IRepositoryFactory;
			wdl.State = DataLayerState.Live;

			Assert.IsNull(rf.CreateRepository<ITestRepositoryNoLiveReg>());
		}
		[Test]
		public void CreateRepository_UnregisteredRepository_Testing_ExpectNull()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryFactory rf = dl as IRepositoryFactory;
			wdl.State = DataLayerState.Testing;

			Assert.IsNull(rf.CreateRepository<ITestRepositoryNoTestingReg>());
		}
		[Test]
		public void CreateRepository_Live()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryFactory rf = dl as IRepositoryFactory;
			wdl.State = DataLayerState.Live;

			Assert.IsInstanceOf<LiveITestRepositoryOne>(rf.CreateRepository<ITestRepositoryOne>());
		}
		[Test]
		public void CreateRepository_Testing()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryFactory rf = dl as IRepositoryFactory;
			wdl.State = DataLayerState.Testing;

			Assert.IsInstanceOf<TestingITestRepositoryOne>(rf.CreateRepository<ITestRepositoryOne>());
		}
		[Test]
		public void CreateRepositoryContext_RepositoryDescriptionsAdded()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IRepositoryContextFactory rf = dl as IRepositoryContextFactory;

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			var repo = rf.CreateRepository<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}
		[Test]
		public void CreateRepositoryContext_UnregisteredRepository_Live_ExpectNull()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryContextFactory rf = dl as IRepositoryContextFactory;
			wdl.State = DataLayerState.Live;

			Assert.IsNull(rf.CreateRepository<ITestRepositoryNoLiveReg>());
		}
		[Test]
		public void CreateRepositoryContext_UnregisteredRepository_Testing_ExpectNull()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryContextFactory rf = dl as IRepositoryContextFactory;
			wdl.State = DataLayerState.Testing;

			Assert.IsNull(rf.CreateRepository<ITestRepositoryNoTestingReg>());
		}
		[Test]
		public void CreateRepositoryContext_Live()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryContextFactory rf = dl as IRepositoryContextFactory;
			wdl.State = DataLayerState.Live;

			Assert.IsInstanceOf<LiveITestRepositoryOne>(rf.CreateRepository<ITestRepositoryOne>());
		}
		[Test]
		public void CreateRepositoryContext_Testing()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer wdl = dl;
			IRepositoryContextFactory rf = dl as IRepositoryContextFactory;
			wdl.State = DataLayerState.Testing;

			Assert.IsInstanceOf<TestingITestRepositoryOne>(rf.CreateRepository<ITestRepositoryOne>());
		}
		[Test]
		public void GetServiceHost_RegistersRepository()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer dlService = dl;

			WhippedCreamDataLayer.Container = new UnityContainer();
			WhippedCreamDataLayer.Container.RegisterInstance<IUrlService>(new DefaultUrlService(new System.Uri("http://localhost"), null));

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			var repo = dlService.GetServiceHost<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}
		[Test]
		public void GetServiceHost_ReturnsWhippedCreamDataServiceHost()
		{
			IWhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			WhippedCreamDataLayer.Container = new UnityContainer();

			WhippedCreamDataLayer.Container = new UnityContainer();
			WhippedCreamDataLayer.Container.RegisterInstance<IUrlService>(new DefaultUrlService(new System.Uri("http://localhost"), null));

			Assert.IsInstanceOf<WhippedCreamDataServiceHost>(dl.GetServiceHost<ITestRepositoryOne>(),
				"The returned object is not an instance of WhippedCreamDataServiceHost.");
		}
		[Test]
		public void GetServiceHost_UnregisteredService()
		{
			try
			{
				IWhippedCreamDataLayer dl = new WhippedCreamDataLayer();
				WhippedCreamDataLayer.Container = new UnityContainer();

				WhippedCreamDataLayer.Container = new UnityContainer();
				WhippedCreamDataLayer.Container.RegisterInstance<IUrlService>(new DefaultUrlService(new System.Uri("http://localhost"), null));

				dl.GetServiceHost<ITestRepositoryTwo>();
				Assert.Fail(@"
System.InvalidOperationException expected because the repository has not registered a service.
");
			}
			catch (System.InvalidOperationException) { }
		}

		[Test]
		public void GetRepositoryDescription_RegistersRepository()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer dlService = dl;

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			var repo = dlService.GetRepositoryDescription<ITestRepositoryOne>();

			Assert.IsTrue(dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)));
		}

		[Test]
		public void GetRepositoryDescription()
		{
			WhippedCreamDataLayer dl = new WhippedCreamDataLayer();
			IWhippedCreamDataLayer dlService = dl;

			if (dl.RepositoryDescriptions.ContainsKey(typeof(ITestRepositoryOne)))
				Assert.Fail("Unable to determine if the ITestRepositoryOne type has been registered in the Repository Descriptions.");

			var repo = dlService.GetRepositoryDescription<ITestRepositoryOne>();

			Assert.AreEqual(dl.RepositoryDescriptions[typeof(ITestRepositoryOne)], repo);
		}
	}
}

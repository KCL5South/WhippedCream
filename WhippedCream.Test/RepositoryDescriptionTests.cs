using System.Collections.Generic;
using NUnit.Framework;
namespace WhippedCream
{
	[TestFixture]
	public class RepositoryDescriptionTests
	{
		#region Testing Dependencies

		[Repository(DataLayerState.Live, typeof(int))]
		[Repository(DataLayerState.Testing, typeof(string))]
		[RepositoryContext(DataLayerState.Live, typeof(short))]
		[RepositoryContext(DataLayerState.Testing, typeof(double))]
		[RepositoryServicePath("TestRepositoryPath")]
		[RepositoryServiceType(typeof(string))]
		private interface ITestOne
		{ }

		[RepositoryServicePath("TestRepositoryPathAgain")]
		[RepositoryServiceType(typeof(System.IDisposable))]
		private interface ITestTwo { }

		private interface ITestThree { }

		#endregion

		[Test]
		public void RepositoryTypeSet()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestOne>();

			Assert.AreEqual(typeof(ITestOne), desc.RepositoryType, "The RepositoryType property was not set.");
		}

		[Test]
		public void RepositoryAttributesSet()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestOne>();

			IDictionary<DataLayerState, System.Type> result = desc.Repositories;

			Assert.IsNotNull(result, "The Repositories collection should not be null.");
			Assert.AreEqual(2, result.Count, "The number of registered repositories should be two.");
			Assert.AreEqual(typeof(int), result[DataLayerState.Live], "The type returned for the Live data layer state is not correct.");
			Assert.AreEqual(typeof(string), result[DataLayerState.Testing], "The type returned for the Testing data layer state is not correct.");
		}

		[Test]
		public void RepositoryContextAttributesSet()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestOne>();

			IDictionary<DataLayerState, System.Type> result = desc.RepositoryContexts;

			Assert.IsNotNull(result, "The Repositories collection should not be null.");
			Assert.AreEqual(2, result.Count, "The number of registered repositories should be two.");
			Assert.AreEqual(typeof(short), result[DataLayerState.Live], "The type returned for the Live data layer state is not correct.");
			Assert.AreEqual(typeof(double), result[DataLayerState.Testing], "The type returned for the Testing data layer state is not correct.");
		}

		[Test]
		public void ServiceTypeSet()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestOne>();

			Assert.AreEqual(typeof(string), desc.ServiceType, "The ServiceType property was not set correctly.");
		}

		[Test]
		public void ServiceTypeSet_Again()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestTwo>();

			Assert.AreEqual(typeof(System.IDisposable), desc.ServiceType, "The ServiceType property was not set correctly.");
		}

		[Test]
		public void ServiceType_NullWhenNotRegistered()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestThree>();

			Assert.IsNull(desc.ServiceType, @"
The ServiceType property should be null if the RepositoryServiceTypeAttribute is not registered on the repository.
");
		}

		[Test]
		public void ServicePathSet()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestOne>();

			Assert.AreEqual("TestRepositoryPath", desc.ServicePath, "The ServicePath property was not set correctly.");
		}

		[Test]
		public void ServicePathSet_Again()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestTwo>();

			Assert.AreEqual("TestRepositoryPathAgain", desc.ServicePath, "The ServicePath property was not set correctly.");
		}

		[Test]
		public void ServicePath_NullWhenNotRegistered()
		{
			IRepositoryDescription desc = new RepositoryDescription<ITestThree>();

			Assert.IsNull(desc.ServicePath, @"
The ServicePath property should be null if the RepositoryServicePathAttribute is not registered on the repository.
");
		}
	}
}

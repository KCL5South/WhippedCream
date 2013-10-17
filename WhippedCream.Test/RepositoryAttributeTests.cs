using NUnit.Framework;
using System.Collections.Generic;

namespace WhippedCream
{
	[TestFixture]
	public class RepositoryAttributeTests
	{
		#region Testing Dependencies

		[Repository(DataLayerState.Testing, typeof(string))]
		[Repository(DataLayerState.Live, typeof(short))]
		public interface ITestOne { }

		[Repository(DataLayerState.Testing, typeof(int))]
		[Repository(DataLayerState.Testing, typeof(string))]
		public interface ITestTwo { }

		#endregion

		[Test]
		public void Constructor_StateSet()
		{
			RepositoryAttribute att = new RepositoryAttribute(DataLayerState.Live, typeof(int));
			Assert.AreEqual(DataLayerState.Live, att.State, "The State Property of the attribute was not set correctly.");
		}
		
		[Test]
		public void Constructor_StateSet_Again()
		{
			RepositoryAttribute att = new RepositoryAttribute(DataLayerState.Testing, typeof(int));
			Assert.AreEqual(DataLayerState.Testing, att.State, "The State Property of the attribute was not set correctly.");
		}

		[Test]
		public void Constructor_RepositoryTypeSet()
		{
			RepositoryAttribute att = new RepositoryAttribute(DataLayerState.Live, typeof(string));
			Assert.AreEqual(typeof(string), att.RepositoryType, "The RepositoryType property of the attribute was not set correctly.");
		}
		
		[Test]
		public void Constructor_RepositoryTypeSet_Again()
		{
			RepositoryAttribute att = new RepositoryAttribute(DataLayerState.Live, typeof(int));
			Assert.AreEqual(typeof(int), att.RepositoryType, "The RepositoryType property of the attribute was not set correctly.");
		}

		[Test]
		public void GetRepositories_ReturnsCorrectValues()
		{
			IDictionary<DataLayerState, System.Type> result = RepositoryAttribute.GetRepositories<ITestOne>();
			Assert.IsNotNull(result, "the result should not be null ever.");
			Assert.AreEqual(2, result.Count, "The Number of entries should match the number of attributes on the interface.");
			Assert.AreEqual(typeof(string), result[DataLayerState.Testing], "The type registered with the Testing state is not correct.");
			Assert.AreEqual(typeof(short), result[DataLayerState.Live], "The type registered with the Live state is not correct.");
		}

		[Test]
		public void GetRepositories_IfThereAreMultipleOfOneState_OnlyOneIsReturned()
		{
			var result = RepositoryAttribute.GetRepositories<ITestTwo>();
			Assert.IsNotNull(result, "The Result should never be null.");
			Assert.AreEqual(1, result.Count, "There should have only been one entry returned because the state was duplicated.");
			Assert.IsTrue(result.ContainsKey(DataLayerState.Testing), "The Testing state should be the single key.");
		}
	}
}

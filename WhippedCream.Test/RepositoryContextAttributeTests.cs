using NUnit.Framework;
using System.Collections.Generic;

namespace WhippedCream
{
	[TestFixture]
	public class RepositoryContextAttributeTests
	{
		#region Testing Dependencies

		[RepositoryContext(DataLayerState.Testing, typeof(string))]
		[RepositoryContext(DataLayerState.Live, typeof(short))]
		public interface ITestOne { }

		[RepositoryContext(DataLayerState.Testing, typeof(int))]
		[RepositoryContext(DataLayerState.Testing, typeof(string))]
		public interface ITestTwo { }

		#endregion

		[Test]
		public void Constructor_StateSet()
		{
			RepositoryContextAttribute att = new RepositoryContextAttribute(DataLayerState.Live, typeof(int));
			Assert.AreEqual(DataLayerState.Live, att.State, "The State Property of the attribute was not set correctly.");
		}

		[Test]
		public void Constructor_StateSet_Again()
		{
			RepositoryContextAttribute att = new RepositoryContextAttribute(DataLayerState.Testing, typeof(int));
			Assert.AreEqual(DataLayerState.Testing, att.State, "The State Property of the attribute was not set correctly.");
		}

		[Test]
		public void Constructor_RepositoryTypeSet()
		{
			RepositoryContextAttribute att = new RepositoryContextAttribute(DataLayerState.Live, typeof(string));
			Assert.AreEqual(typeof(string), att.RepositoryType, "The RepositoryType property of the attribute was not set correctly.");
		}

		[Test]
		public void Constructor_RepositoryTypeSet_Again()
		{
			RepositoryContextAttribute att = new RepositoryContextAttribute(DataLayerState.Live, typeof(int));
			Assert.AreEqual(typeof(int), att.RepositoryType, "The RepositoryType property of the attribute was not set correctly.");
		}

		[Test]
		public void GetRepositoryContexts_ReturnsCorrectValues()
		{
			IDictionary<DataLayerState, System.Type> result = RepositoryContextAttribute.GetRepositoryContexts<ITestOne>();
			Assert.IsNotNull(result, "the result should not be null ever.");
			Assert.AreEqual(2, result.Count, "The Number of entries should match the number of attributes on the interface.");
			Assert.AreEqual(typeof(string), result[DataLayerState.Testing], "The type registered with the Testing state is not correct.");
			Assert.AreEqual(typeof(short), result[DataLayerState.Live], "The type registered with the Live state is not correct.");
		}

		[Test]
		public void GetRepositoryContexts_IfThereAreMultipleOfOneState_OnlyOneIsReturned()
		{
			var result = RepositoryContextAttribute.GetRepositoryContexts<ITestTwo>();
			Assert.IsNotNull(result, "The Result should never be null.");
			Assert.AreEqual(1, result.Count, "There should have only been one entry returned because the state was duplicated.");
			Assert.IsTrue(result.ContainsKey(DataLayerState.Testing), "The Testing state should be the single key.");
		}
	}
}

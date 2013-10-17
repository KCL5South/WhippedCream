using NUnit.Framework;
using System.Linq;
using System.Collections;
namespace WhippedCream.InMemoryDataLayer
{
	[TestFixture]
	public class InMemoryPersistentMediumTests
	{
		[Test]
		public void Constructor_StorageSet()
		{
			InMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			Assert.IsNotNull(medium.Storage, "The Storage Property should have been initialized on construction.");
		}

		[Test]
		public void GetStorage()
		{
			IInMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			var result = medium.GetStorage<string, int>();
			Assert.IsNotNull(result, "The result should never be null.");
		}

		[Test]
		public void GetStorage_SameCollection()
		{
			IInMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			var result = medium.GetStorage<string, int>();
			var result2 = medium.GetStorage<string, int>();

			Assert.IsTrue(object.ReferenceEquals(result, result2), "The two objects should be the same object.");
		}

		[Test]
		public void GetStorage_DifferentRepo_DifferentCollection()
		{
			IInMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			var result = medium.GetStorage<short, int>();
			var result2 = medium.GetStorage<string, int>();

			Assert.IsFalse(object.ReferenceEquals(result, result2), "The two objects should not be the same object.");
		}

		[Test]
		public void GetStorage_SameCollection_ItemsPersist()
		{
			IInMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			var result = medium.GetStorage<string, int>();
			var result2 = medium.GetStorage<string, int>();

			result.Add(1);

			Assert.AreEqual(1, result2.First(), "The first item in the second collection should be what was added.");
		}

		[Test]
		public void GetStorage_AddsCollectionToStorage()
		{
			InMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			IInMemoryPersistentMedium explicitMedium = medium;
			var result = explicitMedium.GetStorage<string, int>();
			Assert.IsTrue(medium.Storage.Contains(result as IList), "The result should exist within the medium's storage collection.");
		}

		[Test]
		public void ClearAll()
		{
			IInMemoryPersistentMedium medium = new InMemoryPersistentMedium();
			var result = medium.GetStorage<string, int>();
			var result2 = medium.GetStorage<string, short>();
			var result3 = medium.GetStorage<int, short>();

			result.Add(1);
			result2.Add(2);
			result3.Add(10);

			medium.ClearAll();

			Assert.AreEqual(0, result.Count(), "The collection should be empty.");
			Assert.AreEqual(0, result2.Count(), "The collection should be empty.");
			Assert.AreEqual(0, result3.Count(), "The collection should be empty.");
		}
	}
}
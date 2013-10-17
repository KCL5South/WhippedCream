using NUnit.Framework;

namespace WhippedCream
{
	[TestFixture]
	public class RepositoryServiceTypeAttributeTests
	{
		#region Testing Dependencies

		[RepositoryServiceTypeAttribute(typeof(string))]
		public interface ITestOne { }


		[RepositoryServiceTypeAttribute(typeof(System.IDisposable))]
		public interface ITestTwo { }

		#endregion


		[Test]
		public void Constructor_PathSet()
		{
			RepositoryServiceTypeAttribute attribute = new RepositoryServiceTypeAttribute(typeof(string));
			Assert.AreEqual(typeof(string), attribute.ServiceType, "The ServiceType property was not set.");
		}

		[Test]
		public void GetPath()
		{
			Assert.AreEqual(typeof(string), RepositoryServiceTypeAttribute.GetType<ITestOne>(), @"
The type returned is not what was registered to the repository.
");
		}

		[Test]
		public void GetPathAgain()
		{
			Assert.AreEqual(typeof(System.IDisposable), RepositoryServiceTypeAttribute.GetType<ITestTwo>(), @"
The type returned is not what was registered to the repository.
");
		}
	}
}

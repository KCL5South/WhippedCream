using NUnit.Framework;

namespace WhippedCream
{
	[TestFixture]
	public class RepositoryServicePathAttributeTests
	{
		#region Testing Dependencies

		[RepositoryServicePathAttribute("TestPathAgain")]
		public interface ITestOne { }


		[RepositoryServicePathAttribute("TestPathAgainAgain")]
		public interface ITestTwo{ }

		#endregion


		[Test]
		public void Constructor_PathSet()
		{
			RepositoryServicePathAttribute attribute = new RepositoryServicePathAttribute("TestPath");
			Assert.AreEqual("TestPath", attribute.ServicePath, "The ServicePath property was not set.");
		}

		[Test]
		public void GetPath()
		{
			Assert.AreEqual("TestPathAgain", RepositoryServicePathAttribute.GetPath<ITestOne>(), @"
The path returned is not what was registered to the repository.
");
		}
		
		[Test]
		public void GetPathAgain()
		{
			Assert.AreEqual("TestPathAgainAgain", RepositoryServicePathAttribute.GetPath<ITestTwo>(), @"
The path returned is not what was registered to the repository.
");
		}
	}
}

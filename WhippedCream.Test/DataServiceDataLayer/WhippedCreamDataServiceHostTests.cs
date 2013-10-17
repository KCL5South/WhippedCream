using System;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class WhippedCreamDataServiceHostTests
	{
		[Test]
		public void Constructor_NullContainer()
		{
			try
			{
				new WhippedCreamDataServiceHost(null, typeof(WhippedCreamDataServiceHostTests), new Uri("http://localhost:1234"));
				Assert.Fail("An ArgumentNullException was expected because we passed null in for the container parameter.");
			}
			catch(ArgumentNullException) { }
		}

		[Test]
		public void Constructor_ContainerSet()
		{
			var container = Mock.Of<IUnityContainer>();
			var target = new WhippedCreamDataServiceHost(container, typeof(WhippedCreamDataServiceHostTests), new Uri("http://localhost:1234"));
			Assert.AreEqual(container, target.Container, @"
The Container property was not set to the same object that was passed to the constructor.
");
		}
	}
}

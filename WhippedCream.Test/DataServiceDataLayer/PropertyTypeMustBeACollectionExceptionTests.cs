using System;
using NUnit.Framework;

namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class PropertyTypeMustBeACollectionExceptionTests
	{
		[Test]
		public void Constructor_NullType()
		{
			PropertyTypeMustBeACollectionException ex = new PropertyTypeMustBeACollectionException(null);
			Assert.AreEqual(string.Format(PropertyTypeMustBeACollectionException.MessageFormat, "<unknown>"),
							ex.Message);
		}
		[Test]
		public void Constructor_MessageCorrect()
		{
			PropertyTypeMustBeACollectionException ex = new PropertyTypeMustBeACollectionException(this.GetType());
			Assert.AreEqual(string.Format(PropertyTypeMustBeACollectionException.MessageFormat, this.GetType()),
							ex.Message);
		}
	}
}
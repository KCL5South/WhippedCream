using System;
using NUnit.Framework;

namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class UnknownPropertyExceptionTests
	{
		[Test]
		public void Constructor_NullPropertyName()
		{
			UnknownPropertyException ex = new UnknownPropertyException(null, this.GetType());

			Assert.AreEqual(string.Format(UnknownPropertyException.MessageFormat, "<unknown>", this),
							ex.Message, @"
If null is passed in for the property name, we will use '<unknown>' instead.
");
		}
		[Test]
		public void Constructor_EmptyPropertyName()
		{
			UnknownPropertyException ex = new UnknownPropertyException(string.Empty, this.GetType());

			Assert.AreEqual(string.Format(UnknownPropertyException.MessageFormat, "<unknown>", this),
							ex.Message, @"
If an empty string is passed in for the property name, we will use '<unknown>' instead.
");
		}
		[Test]
		public void Constructor_WhitespacePropertyName()
		{
			UnknownPropertyException ex = new UnknownPropertyException("\t", this.GetType());

			Assert.AreEqual(string.Format(UnknownPropertyException.MessageFormat, "<unknown>", this),
							ex.Message, @"
If the property name is only whitespace we will use '<unknown>' instead.
");
		}
		[Test]
		public void Constructor_NullTargetType()
		{
			UnknownPropertyException ex = new UnknownPropertyException("TestPropertyName", null);

			Assert.AreEqual(string.Format(UnknownPropertyException.MessageFormat, "TestPropertyName", "<unknown>"),
							ex.Message, @"
If the target type passed is null, we'll use '<unknown>' instead.
");
		}
		[Test]
		public void MessageIsCorrect()
		{
			UnknownPropertyException ex = new UnknownPropertyException("TestPropertyName", this.GetType());

			Assert.AreEqual(string.Format(UnknownPropertyException.MessageFormat, "TestPropertyName", this),
							ex.Message, @"
The message that was generated was not what was expected.
");
		}
	}
}
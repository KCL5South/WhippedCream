using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace WhippedCream
{
	[TestFixture]
	public class RepositoryContextExtensionsTests
	{
		#region Testing Dependencies

		public class TestModel { public string Name { get; set; } }
		public class DummyContext : IRepositoryContext
		{
			public List<Tuple<object, string>> LoadPropertyParameters =
				new List<Tuple<object, string>>();

			#region IRepositoryContext Members

			public void AddObject(string entitySetName, object entity)
			{
				throw new System.NotImplementedException();
			}

			public void DeleteObject(object entity)
			{
				throw new System.NotImplementedException();
			}

			public void SaveChanges()
			{
				throw new System.NotImplementedException();
			}

			public void LoadProperty(object entity, string propertyName)
			{
				LoadPropertyParameters.Add(new Tuple<object, string>(entity, propertyName));
			}

			#endregion
		}


		#endregion

		[Test]
		public void LoadProperty_NullSelector()
		{
			try
			{
				DummyContext context = new DummyContext();
				context.LoadProperty<TestModel>(new TestModel(), null);
				Assert.Fail("An ArgumentNullException was expected.");
			}
			catch (ArgumentNullException) { }
		}

		[Test]
		public void LoadProperty_InvalidExpression()
		{
			try
			{
				DummyContext context = new DummyContext();
				context.LoadProperty<TestModel>(new TestModel(), (a) => new object());
				Assert.Fail(@"
An ArgumentException was expected when we have an invalid expression.
The selector must be in the form of (entity) => entity.Property where Property is
a property on the object.
");
			}
			catch (ArgumentException) { }
		}

		[Test]
		public void LoadProperty_CorrectParametersArePassed()
		{
			TestModel model = new TestModel();
			DummyContext context = new DummyContext();
			context.LoadProperty(model, a => a.Name);

			Assert.AreEqual(1, context.LoadPropertyParameters.Count, @"
The context.LoadProperty method was not called.
");
			Assert.AreEqual(model, context.LoadPropertyParameters[0].Item1, @"
The entity passed to the context.LoadProperty method was not correct.
");
			Assert.AreEqual("Name", context.LoadPropertyParameters[0].Item2, @"
The property name passed to the context.LaodProperty method was not correct.
");
		}
	}
}
using System;
using NUnit.Framework;
using System.Linq;
using System.Data.Services;
using Microsoft.Practices.Unity;
using Moq;
using System.Collections.Generic;

namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class DataServiceUpdatableContextTests
	{
		private class DummyContext : WhippedCreamUpdatableContext
		{
			public DummyContext() : base(new UnityContainer()) { CollectionProperty = new List<string>(); }
			public DummyContext(IUnityContainer container) : base(container) { CollectionProperty = new List<string>(); }

			public int NotACollectionProperty { get; set; }
			public List<string> CollectionProperty { get; set; }
			public List<Tuple<string, string>> GetTypeParameters = new List<Tuple<string, string>>();
			public List<Tuple<WhippedCreamUpdatableContext.ODataOperation, object>> SaveEntityParameters = 
				new List<Tuple<WhippedCreamUpdatableContext.ODataOperation, object>>();

			public override Type GetType(string containerName, string fullNameType)
			{
				GetTypeParameters.Add(new Tuple<string, string>(containerName, fullNameType));
				if(fullNameType == typeof(TestModel).ToString())
					return typeof(TestModel);
				else
					return null;
			}
			public override void SaveEntity(WhippedCreamUpdatableContext.ODataOperation operation, object resource)
			{
				SaveEntityParameters.Add(new System.Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(operation, resource));
			}

			public List<TestModel> Models = new List<TestModel>();
		}

		public class TestModel
		{
			public string Name { get; set; }
		}
	
		[Test]
		public void MakeSureItInheritsFromIUpdatable()
		{
			Assert.IsTrue(typeof(WhippedCreamUpdatableContext).GetInterfaces().Any(a => a == typeof(IUpdatable)), @"
The DataServiceContext needs to inherit from IUpdatable.

Objects that use the Reflection Provider in WCF Data Services, need to inherit from IUpdatable in order
for the correct EntitySets to be used in complext Entity relationship scenarios.
");
		}

		[Test]
		public void Constructor_NullContainer()
		{
			try
			{
				DummyContext context = new DummyContext(null);
				Assert.Fail("An ArgumentNullException was expected when passing null in for the container.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void Constructor_PendingChangesSet()
		{
			DummyContext context = new DummyContext();
			Assert.IsNotNull(context.PendingChanges, @"
The PendingChanges property should have been initialized on construction.
");
		}

		[Test]
		public void Constructor_ContainerSet()
		{
			IUnityContainer container = Mock.Of<IUnityContainer>();
			DummyContext context = new DummyContext(container);

			Assert.AreEqual(container, context.Container, "The Container property has not been set.");
		}

		[Test]
		public void AddReferenceToCollection_NullResource()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(null, "PropertyName", new object());
				Assert.Fail("An ArgumentNullException was expected when passing null in for the target resource.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void AddReferenceToCollection_NullPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(new object(), null, new object());
				Assert.Fail("An ArgumentException was expected when passing null in for the property name.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void AddReferenceToCollection_EmptyPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(new object(), string.Empty, new object());
				Assert.Fail("An ArgumentException was expected when passing an empty string in for the property name.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void AddReferenceToCollection_WhitespacePropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(new object(), " ", new object());
				Assert.Fail("An ArgumentException was expected when passing an empty string in for the property name.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void AddReferenceToCollection_NullResourceToBeAdded()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(new object(), "PropertyName", null);
				Assert.Fail("An ArgumentNullException was expected when passing null in for resource to be added.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void AddReferenceToCollection_PropertyNotFound()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(this, System.Guid.NewGuid().ToString(), new object());
				Assert.Fail("An UnknownPropertyException is expected if a property name is given that doesn't exist on the resource object.");
			}
			catch(UnknownPropertyException) { }
		}

		[Test]
		public void AddReferenceToCollection_PropertyShouldBeAList()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.AddReferenceToCollection(context, "NotACollectionProperty", new object());
				Assert.Fail(@"
A PropertyTypeMustBeACollectionException is expected if a property name is given of a property that isn't a collection.
");
			}
			catch (PropertyTypeMustBeACollectionException) { }
		}

		[Test]
		public void AddReferenceToCollection_MakeSureObjectIsAdded()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			up.AddReferenceToCollection(context, "CollectionProperty", "TestString");

			Assert.AreEqual(1, context.CollectionProperty.Count, "The number of items in the collection property is not correct.");
			Assert.AreEqual("TestString", context.CollectionProperty[0], @"
The first string in the collection is not correct.
");
		}

		[Test]
		public void ClearChanges_ClearsChangesCollection()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			context.PendingChanges.Add(
					new System.Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(WhippedCreamUpdatableContext.ODataOperation.Delete, 
													     new object()));

			up.ClearChanges();

			Assert.AreEqual(0, context.PendingChanges.Count, @"
The number of items in the PendingChanges collection should be zero.
");
		}

		[Test]
		public void CreateResource_NullContainerName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.CreateResource(null, "TestName");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch(System.ArgumentException) { }
		}

		[Test]
		public void CreateResource_EmptyContainerName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.CreateResource(string.Empty, "TestName");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch(System.ArgumentException) { }
		}

		[Test]
		public void CreateResource_WhiteSpaceContainerName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.CreateResource(" ", "TestName");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch(System.ArgumentException) { }
		}

		[Test]
		public void CreateResource_NullFullTypeName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.CreateResource("TestContainer", null); 
				Assert.Fail("An ArgumentException was expected.");
			}
			catch(System.ArgumentException) { }
		}

		[Test]
		public void CreateResource_EmptyFullTypeName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.CreateResource("TestContainer", string.Empty); 
				Assert.Fail("An ArgumentException was expected.");
			}
			catch(System.ArgumentException) { }
		}

		[Test]
		public void CreateResource_WhiteSpaceFullTypeName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.CreateResource("TestContainer", "  ");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch(System.ArgumentException) { }
		}

		[Test]
		public void CreateResource_AddsItemAsSaveIntoPendingChanges()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			if(context.PendingChanges.Count != 0)
				Assert.Fail("Unable to determine if the object we're creating is being added to the pending changes collection.");

			up.CreateResource("TestContainer", typeof(TestModel).ToString());

			Assert.AreEqual(1, context.PendingChanges.Count, @"
We expect the item we're creating to be placed in the pending changes collection
as a new Insert.
");
			Assert.AreEqual(WhippedCreamUpdatableContext.ODataOperation.Insert, 
					context.PendingChanges.First().Item1, 
					"The operation type of the first item is not correct.");
			Assert.IsInstanceOf<TestModel>(context.PendingChanges.First().Item2,
						    "The object type of the first item is not correct.");
		}

		[Test]
		public void CreateResource_CallsGetType()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			var result = up.CreateResource("TestContainerName", typeof(TestModel).ToString()); 

			Assert.AreEqual(1, context.GetTypeParameters.Count, @"
We expect to have an entry in our CreateEntityParameters collection after we call CreateResource.
");
			Assert.AreEqual("TestContainerName", context.GetTypeParameters[0].Item1,
					"The Container Name was not passed to the create entity method.");
			Assert.AreEqual(typeof(TestModel).ToString(), context.GetTypeParameters[0].Item2,
					"The Full Type Name was not passed to the Create Entity method.");
		}

		[Test]
		public void DeleteResource_NullResource()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.DeleteResource(null);
				Assert.Fail("An ArgumentNullException was expected for a null resource.");
			}
			catch(System.ArgumentNullException) { }
		}

		[Test]
		public void DeleteResource_AddsDeleteEntryIntoPendingChanges()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			if(context.PendingChanges.Count != 0)
				Assert.Fail("Unable to determine if an operation item was added to the PendingChanges collection.");

			object target = new object();
			up.DeleteResource(target);

			Assert.AreEqual(1, context.PendingChanges.Count, 
					"There should have been an operation entry added to the PendingChanges collection.");
			Assert.AreEqual(WhippedCreamUpdatableContext.ODataOperation.Delete,
					context.PendingChanges.First().Item1,
					"The operation type should be a delete operation.");
			Assert.AreEqual(target,
					context.PendingChanges.First().Item2,
					"The object queued for deletion should have been the object passed to the DeleteResource method.");
		}

		[Test]
		public void GetResource_NullQuery()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.GetResource(null, null);
				Assert.Fail("An ArgumentNullException was expected for a null query.");
			}
			catch(System.ArgumentNullException) { }
		}

		[Test]
		public void GetResource_ReturnsFirstItem()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			context.Models.Add(new TestModel() { Name = "TestName" });
			context.Models.Add(new TestModel() { Name = "TestNameAgain" });

			var result = up.GetResource(context.Models.AsQueryable(), null);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<TestModel>(result, "The returned object should be a TestModel.");
			Assert.AreEqual("TestName", (result as TestModel).Name,
					"The Name property on the returned object should be equal to the first item that was added to the collection.");
		}

		[Test]
		public void GetResource_ReturnsNullIfCollectionIsEmpty()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			Assert.IsNull(up.GetResource(context.Models.AsQueryable(), null), 
				      "There were not items in the collection, so null should have been returned.");
		}

		[Test]
		public void GetValue_NullResource()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.GetValue(null, "Name");
				Assert.Fail("An ArgumentNullException was expected.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void GetValue_NullPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.GetValue(new object(), null);
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void GetValue_EmptyPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.GetValue(new object(), string.Empty);
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void GetValue_WhiteSpacePropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.GetValue(new object(), "\t");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void GetValue_UnknownPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				TestModel model = new TestModel() { Name = "TestName" };

				context.GetValue(model, "NotNameProperty");
				Assert.Fail("An UnknownPropertyException was expected.");
			}
			catch (UnknownPropertyException) { }
		}

		[Test]
		public void GetValue_CorrectValueReturned()
		{
			IUpdatable context = new DummyContext();
			TestModel model = new TestModel() { Name = "TestString" };

			Assert.AreEqual("TestString", context.GetValue(model, "Name"), 
					"The value of the Name property should have been returned.");
		}

		[Test]
		public void RemoveReferenceFromCollection_NullResource()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(null, "PropertyName", new object());
				Assert.Fail("An ArgumentNullException was expected when passing null in for the target resource.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_NullPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(new object(), null, new object());
				Assert.Fail("An ArgumentException was expected when passing null in for the property name.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_EmptyPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(new object(), string.Empty, new object());
				Assert.Fail("An ArgumentException was expected when passing an empty string in for the property name.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_WhitespacePropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(new object(), " ", new object());
				Assert.Fail("An ArgumentException was expected when passing an empty string in for the property name.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_NullResourceToBeAdded()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(new object(), "PropertyName", null);
				Assert.Fail("An ArgumentNullException was expected when passing null in for resource to be added.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_PropertyNotFound()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(this, System.Guid.NewGuid().ToString(), new object());
				Assert.Fail("An UnknownPropertyException is expected if a property name is given that doesn't exist on the resource object.");
			}
			catch(UnknownPropertyException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_PropertyShouldBeAList()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.RemoveReferenceFromCollection(context, "NotACollectionProperty", new object());
				Assert.Fail(@"
A PropertyTypeMustBeACollectionException is expected if a property name is given of a property that isn't a collection.
");
			}
			catch (PropertyTypeMustBeACollectionException) { }
		}

		[Test]
		public void RemoveReferenceFromCollection_MakeSureObjectIsRemoved()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			context.CollectionProperty.Add("TestString");
			if(context.CollectionProperty.Count != 1)
				Assert.Fail("Unable to determine if the 'TestString' instance was removed.");
			up.RemoveReferenceFromCollection(context, "CollectionProperty", "TestString");

			Assert.AreEqual(0, context.CollectionProperty.Count, "The number of items in the collection property is not correct.");
		}

		[Test]
		public void ResetResource_ReturnsWhatWasPassedToIt()
		{
			IUpdatable context = new DummyContext();
			object resource = new object();
			Assert.AreEqual(resource, context.ResetResource(resource),
					"The resource returned is not correct.");
		}

		[Test]
		public void ResolveResource_ReturnsWhatWasPassedToit()
		{
			IUpdatable context = new DummyContext();
			object resource = new object();
			Assert.AreEqual(resource, context.ResolveResource(resource),
					"The resource returned is not correct.");
		}

		[Test]
		public void SaveChanges_CallsSaveEntityCorrectNumberOfTimes()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			context.PendingChanges.Add(new Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(
						WhippedCreamUpdatableContext.ODataOperation.Insert, "TestString"));
			context.PendingChanges.Add(new Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(
						WhippedCreamUpdatableContext.ODataOperation.Delete, "TestStringAgain"));

			up.SaveChanges();

			Assert.AreEqual(2, context.SaveEntityParameters.Count, "The number of times SaveEntity was called should have been two.");
		}

		[Test]
		public void SaveChanges_CorrectParametersWerePassed()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			context.PendingChanges.Add(new Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(
						WhippedCreamUpdatableContext.ODataOperation.Insert, "TestString"));

			up.SaveChanges();

			Assert.AreEqual(WhippedCreamUpdatableContext.ODataOperation.Insert, 
					context.SaveEntityParameters[0].Item1,
					"The first item in the first parameter set should be an insert flag.");
			Assert.AreEqual("TestString",
					context.SaveEntityParameters[0].Item2,
					"The second item in the first parameter set should be \"TestString\".");
		}

		[Test]
		public void SaveChanges_PendingChangesIsCleared()
		{
			DummyContext context = new DummyContext();
			IUpdatable up = context;
			context.PendingChanges.Add(new Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(
						WhippedCreamUpdatableContext.ODataOperation.Insert, "TestString"));
			context.PendingChanges.Add(new Tuple<WhippedCreamUpdatableContext.ODataOperation, object>(
						WhippedCreamUpdatableContext.ODataOperation.Delete, "TestStringAgain"));

			up.SaveChanges();

			Assert.AreEqual(0, context.PendingChanges.Count, "The number of items in the pending collection should be zero after a call to SaveChanges."); 
		}

		[Test]
		public void SetReference_NullResource()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetReference(null, "Name", "Value");
				Assert.Fail("An ArgumentNullException was expected.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void SetReference_NullPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetReference(new object(), null, "Value");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void SetReference_EmptyPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetReference(new object(), string.Empty, "Value");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void SetReference_WhiteSpacePropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetReference(new object(), "\t", "value");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void SetReference_UnknownPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				TestModel model = new TestModel() { Name = "TestName" };

				context.SetReference(model, "NotNameProperty", "Value");
				Assert.Fail("An UnknownPropertyException was expected.");
			}
			catch (UnknownPropertyException) { }
		}

		[Test]
		public void SetReference_CorrectValueSet()
		{
			IUpdatable context = new DummyContext();
			TestModel model = new TestModel() { Name = "TestString" };

			context.SetReference(model, "Name", "TestStringAgain");

			Assert.AreEqual("TestStringAgain", model.Name,
					"The Name property should have been reset.");
		}

		[Test]
		public void SetValue_NullResource()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetValue(null, "Name", "Value");
				Assert.Fail("An ArgumentNullException was expected.");
			}
			catch (System.ArgumentNullException) { }
		}

		[Test]
		public void SetValue_NullPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetValue(new object(), null, "Value");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void SetValue_EmptyPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetValue(new object(), string.Empty, "Value");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void SetValue_WhiteSpacePropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				context.SetValue(new object(), "\t", "value");
				Assert.Fail("An ArgumentException was expected.");
			}
			catch (System.ArgumentException) { }
		}

		[Test]
		public void SetValue_UnknownPropertyName()
		{
			try
			{
				IUpdatable context = new DummyContext();
				TestModel model = new TestModel() { Name = "TestName" };

				context.SetValue(model, "NotNameProperty", "Value");
				Assert.Fail("An UnknownPropertyException was expected.");
			}
			catch (UnknownPropertyException) { }
		}

		[Test]
		public void SetValue_CorrectValueSet()
		{
			IUpdatable context = new DummyContext();
			TestModel model = new TestModel() { Name = "TestString" };

			context.SetValue(model, "Name", "TestStringAgain");

			Assert.AreEqual("TestStringAgain", model.Name,
					"The Name property should have been reset.");
		}
	}
}

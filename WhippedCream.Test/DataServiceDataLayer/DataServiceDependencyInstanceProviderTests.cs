using System;
using NUnit.Framework;
using Moq;
using Microsoft.Practices.Unity;
using System.ServiceModel.Dispatcher;

namespace WhippedCream.DataServiceDataLayer
{
	[TestFixture]
	public class DataServiceDependencyInstanceProviderTests
	{
		[Test]
		public void Constructor_ContainerSet()
		{
			IUnityContainer container = Mock.Of<IUnityContainer>();
			DataServiceDependencyInstanceProvider provider = new DataServiceDependencyInstanceProvider(container, typeof(int));

			Assert.AreEqual(container, provider.Container, @"
The Container property should have been populated with the same object that was passed to the constructor.
");
		}

		[Test]
		public void Constructor_TypeSet()
		{
			Type targetType = typeof(string);
			DataServiceDependencyInstanceProvider provider = new DataServiceDependencyInstanceProvider(Mock.Of<IUnityContainer>(), targetType);

			Assert.AreEqual(targetType, provider.ServiceType, @"
The ServiceType property should have been populated with the same type that was passed to the constructor.
");
		}

		[Test]
		public void Constructor_NullContainer()
		{
			try
			{
				DataServiceDependencyInstanceProvider provider = new DataServiceDependencyInstanceProvider(null, typeof(int));
				Assert.Fail("We expected a System.ArgumentNullException when we passed null for the container.");
			}
			catch(System.ArgumentNullException) { }
		}

		[Test]
		public void Constructor_NullType()
		{
			try
			{
				DataServiceDependencyInstanceProvider provider = new DataServiceDependencyInstanceProvider(Mock.Of<IUnityContainer>(), null);
				Assert.Fail("We expected a System.ArgumentNullException when we passed null for the service type.");
			}
			catch(System.ArgumentNullException) { }
		}

		[Test]
		public void GetInstance_SingleParameter_ShouldReturnResolvedObjectFromContainer()
		{
			Mock<IUnityContainer> containerMock = new Mock<IUnityContainer>();
			containerMock.Setup(a => a.Resolve(typeof(int), null)).Returns(55);
			IInstanceProvider target = new DataServiceDependencyInstanceProvider(containerMock.Object, typeof(int));

			var result = target.GetInstance(null);

			Assert.AreEqual(55, result, @"
We want to require that the instance provider use the unity container to resolve the instances asked for.

Since we didn't get 55 back from the call to GetInstance, we have to assume that the unity container wasn't
used to resolve for the instance.
");
		}
		
		[Test]
		public void GetInstance_SingleParameter_ShouldReturnResolvedObjectFromContainer_Again()
		{
			Mock<IUnityContainer> containerMock = new Mock<IUnityContainer>();
			containerMock.Setup(a => a.Resolve(typeof(int), null)).Returns(31);
			IInstanceProvider target = new DataServiceDependencyInstanceProvider(containerMock.Object, typeof(int));

			var result = target.GetInstance(null);

			Assert.AreEqual(31, result, @"
We want to require that the instance provider use the unity container to resolve the instances asked for.

Since we didn't get 31 back from the call to GetInstance, we have to assume that the unity container wasn't
used to resolve for the instance.
");
		}
		
		[Test]
		public void GetInstance_MultiParameter_ShouldReturnResolvedObjectFromContainer()
		{
			Mock<IUnityContainer> containerMock = new Mock<IUnityContainer>();
			containerMock.Setup(a => a.Resolve(typeof(int), null)).Returns(55);
			IInstanceProvider target = new DataServiceDependencyInstanceProvider(containerMock.Object, typeof(int));

			var result = target.GetInstance(null, null);

			Assert.AreEqual(55, result, @"
We want to require that the instance provider use the unity container to resolve the instances asked for.

Since we didn't get 55 back from the call to GetInstance, we have to assume that the unity container wasn't
used to resolve for the instance.
");
		}
		
		[Test]
		public void GetInstance_MultiParameter_ShouldReturnResolvedObjectFromContainer_Again()
		{
			Mock<IUnityContainer> containerMock = new Mock<IUnityContainer>();
			containerMock.Setup(a => a.Resolve(typeof(int), null)).Returns(31);
			IInstanceProvider target = new DataServiceDependencyInstanceProvider(containerMock.Object, typeof(int));

			var result = target.GetInstance(null, null);

			Assert.AreEqual(31, result, @"
We want to require that the instance provider use the unity container to resolve the instances asked for.

Since we didn't get 31 back from the call to GetInstance, we have to assume that the unity container wasn't
used to resolve for the instance.
");
		}
	}
}

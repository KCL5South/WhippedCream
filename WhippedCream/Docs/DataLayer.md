Whipped Cream {#DataLayer}
========================

In this document I'm going to spend as much time as needed to thoroughly explain Whipped Cream and how to use it within your data layer.  I'm making the assumption that the reader has an average understanding of C# and .Net.

Concepts
----------

**Inversion Of Control** and **The Dependency Injection Pattern**

Inversion of Control and Dependency Injection can be thought of as the same thing in the context of our project.

The purpose is to abstract the creation of dependencies.

Whipped Cream uses the [Unity Container](http://unity.codeplex.com) as an inversion of controler container.  It is very simple to use.

~~~{.cs}
public class ObjectA
{
	public ObjectA(ObjectB b)
	{
		ChildObject = b;
	}

	public ObjectB ChildObject { get; set; }
}

public class ObjectB
{
	public Constructor(...) { }

	public string Message { get { return "Hey!"; } }
}

IUnityContainer container = new UnityContainer();
container.RegisterType<ObjectB>();

ObjectA newObject = container.Resolve<ObjectA>();
~~~

In this example, we have two objects.  ObjectA and ObjectB.  In order to create an instance of ObjectA, we need an instance of ObjectB as you can see from ObjectA's constructor.

Now, let's say we don't know anything about ObjectB.  We don't have access to the constructor, so we don't know how to supply ObjectA with an instance of it.  Unity Container to the rescue.  After we register ObjectB with the unity container, we're able to create an instance of ObjectA.  The Unity container handles the construction of ObjectB and ObjectA for us.  Pretty slick huh?

Now, imagine that you have one hundred tests for an object.  Suddenly we decide that we need to add functionality to that object and need to write one hundred more tests.  However, the new functionality requires us to add a parameter to the object's constructor.  That means, we'll have to edit the previous 100 tests in order to pass the new parameter to the constructor.  What a pain.  If we had used the dependency injection pattern from the beginning, this scenario would be much less painful.  (Even though writing 100 more tests on top of anything is pretty painful.)

~~~{.cs}

[TestFixture]
public class ObjectATests
{
	IUnityContainer Container = new UnityContainer();

	ObjectA GetObjectUnderTest()
	{
		return Container.Resolve<ObjectA>();
	}

	[Test]
	public void TestingSomething()
	{
		//This single line keeps your tests less brittle by allowing
		//the creation of the object under test to be handled by
		//the IOC Container.
		var target = GetObjectUnderTest();

		...Do Some Testing on target here...
	}
}
~~~

**The Repository Pattern**

In a nutshell, the repository pattern is a design technique that forces the encapsulation of all data access processing within a single abstract construct called a repository.

MSDN has a great write up for it [here](http://msdn.microsoft.com/en-us/library/ff649690.aspx)

Within the context of Whipped Cream, you can think of a repository simply as *where you get your data*.  If you need data, you create a repository and it provides it to you.

From the perspective of the day to day developer, databases, web services, RSS feeds, files, and the like, don't exist anymore.  Only repositories.

In Whipped Cream we represent repositories as interfaces:

~~~{.cs}
public interface TestRepository
{
	IQueryable<TestModel> TestModels { get; }
}
~~~

Here we have a repository called TestRepository which serves up models of type *TestModel*.  Using [Linq](http://msdn.microsoft.com/en-us/library/vstudio/bb397926.aspx) you can create almost any query on that data you can think of.

Technologies Used
----------

**Entity Framework**

The *Entity Framework* is a great database access layer.  You can find all kinds of information on it [here](http://msdn.microsoft.com/en-US/data/ef).

In a nutshell, the Entity Framework leverages the [Entity Data Model](http://msdn.microsoft.com/en-us/library/ee382825.aspx) to manage interactions between a database and your client.

**Unity**

I've mentioned it already, but the [Unity](http://unity.codeplex.com/) project is a Dependency Injection library.  The *IUnityContainer* object comes from this library.  I suggest reading up on this because Inversion of Control is an awesome concept and makes managing dependencies so much easier.

**WCF Data Services**

You may have heard of [OData](http://www.odata.org/) before.  It leverages the RSS/ATOM technology of old to create web endpoints for serving up data.  Microsoft has leveraged this on top of it's [WCF](http://msdn.microsoft.com/en-us/library/dd456779.aspx) technology and now it's easier than ever to create [Rest Services](http://msdn.microsoft.com/en-us/magazine/dd315413.aspx) for your data.

You can find information on [WCF Data Services here](http://msdn.microsoft.com/en-us/data/odata.aspx).

**T4 Templates**

It's worth mentioning [T4 Text Templates](http://msdn.microsoft.com/en-us/library/vstudio/bb126445.aspx) because they do make it easier to create the objects necessary to use Whipped Cream.

Whipped Cream
----------

**Ingredients**

The Whipped Cream layer consists of four sub-layers:

- Live Repository (Sugar)
- Testing Repository (Milk)
- Live Context (Cream)
- Testing Context (Butter)

The Sugar and Cream layers represent the abstractions that are used when we're running our software in production.

The Milk and Butter layers represent the abstractions that are used when we're running our software in a Testing environment.

Conceptually speaking, there is no difference between a Repository and a Context.

> Contexts are Repositories

The difference is in when they are needed.

![Recipe](/Images/DataLayer_Diagram1.png "Recipe")

**Preperation**

To Start, we need a model that represents our data.

~~~{.cs}
public class TestModel
{
	public string Name { get; set; }
}
~~~

Next, we need a repository and we define our repositories in Whipped Cream as interfaces.  There are rules however:

- All Properties must be of type [IQueryable<T>](http://msdn.microsoft.com/en-us/library/bb351562.aspx).
- The Type queried for in the properties must be a class.
- Methods must either return an [IQueryable<T>](http://msdn.microsoft.com/en-us/library/bb351562.aspx), an [IEnumerable<T>](http://msdn.microsoft.com/en-us/library/9eekhta0.aspx), or an integer.
- Method parameters must be only primitive types. No entities can be passed as parameters in methods.
- Repositories must be decorated with a RepositoryAttribute for the Live state and the Testing state.
- Repositories must be decorated with a RepositoryContextAttribute for the Live state and the Testing state.
- Types supplied to the RepositoryAttribute and RepositoryContextAttribute attributes must be instances of the repository those attributes are decorating.

~~~{.cs}
[Repository(DataLayerState.Live, ...)]
[Repository(DataLayerState.Testing, ...)]
[RepositoryContext(DataLayerState.Live, ...)]
[RepositoryContext(DataLayerState.Testing, ...)]
public interface ITestRepository
{
	IQueryable<TestModel> Models { get; }

	int SomeOperation();
	IEnumerable<TestModel> PageModels(int skip, int take);
}
~~~

> *All Properties must have a type of `IQueryable<T>`*

This is a requirement of the Entity Data Model that the Entity Framework uses.  Typical usage of a repository is to query one of it's properties for a particular entity or set of entities that you're looking for.

~~~{.cs}
ITestRepository repo = ...

var result = repo.Models.Where(a => a.Name == "TestModelName");

...
~~~

If you wish to do more specialiezed queries or operations, you'll need to create a method on the repository to handle that for you.

> *The Type queried for in the properties must be a class*

This is another requirement of the Entity Data Model.

> *Methods must either return an `IQueryable<T>`, an `IEnumerable<T>`, or an integer*

This is a requirement of the Entity Data Model.

> *Method parameters must be only primitive types. No entities can be passed as parameter in methods*

This is a joint requirement of the Entity Data Model and WCF Data Services.

The term Primitive Type is a little missleading.  You can think of it as meaning only the C# base value types.  (Ex: int, int16, short, string...).

> *Repositories must be decorated with a `RepositoryAttribute` for the Live state and the Testing state*

The `RepositoryAttribute` is used to signify which object is going to be created and used in the Sugar and Milk states.  The first parameter of the constructor takes in a `DataLayerState` value.

~~~{.cs}
public enum DataLayerState
{
	Live,
	Testing
}
~~~

The second parameter takes in a `System.Type` value that represents which object to create for that state.

> *Repositories must be decorated with a `RepositoryContextAttribute` for the Live state and the Testing state*

The `RepositoryContextAttribute` is use to signify which object is going to be created and used in the Cream and Butter states.  The first parameter is a `DataLayerState` value and the second is the type to create.

> *Types supplied to the `RepositoryAttribute` and `RepositoryContextAttribute` attributes must be instances of the repository those attributes are decorating*

The `RepositoryAttribute` and `RepositoryContextAttribute` attributes are used to signify which implementation of the interface their decorating is going to be created.  Which depends on if the state is Live or Testing, and if we need a Repository or a Context.  Therefore, we require that those instances inherit from the that repository.

Once you have your repository setup, there is one more step to prepare Whipped Cream.  You need to make sure the library is bootstrapped.

~~~{.cs}
namespace WhippedCream
{
	public class WhippedCreamDataLayer
	{
		...
		public static void Bootstrap(IUnityContainer conatiner, System.Uri baseUri) { ... }
		public static void Bootstrap(IUnityContainer container, System.Uri baseUri, string servicePrefix){ ... }
		...
}
~~~

You need to call `WhippedCreamDataLayer.Bootstrap` with an instance of `IUnityContainer` and a base Uri.

The `BaseUri` represents the base uri that the WCF portion of the layer will use to resolve addresses for it's services.

If you use the `Bootstrap` overload that takes in a service prefix as well, then you'll be able to specify which prefix to append to the base uri for your service.

For example:

Let's say we have a repository with a `RepositoryServicePathAttribute`.  That attribute has a value of "TestRepository".  We also have defined our `BaseUri` to be "http://localhost:1234".  Finally, we have defined a service prefix of "api".  The resulting url will become "http://localhost:1234/api/TestRepository"

Entity Framework Access Layer
----------

There is no requirement that any of the repository implementations you create actually do anything.  They simply must implement your repository.  That being said, Whipped Cream does house some helper structures that make it easier to work with the Entity Framework.

After defining your .csdl, .ssdl, and .msl files (or simply the .edmx file), you'll need to define an implementation of [ObjectContext](http://msdn.microsoft.com/en-us/library/system.data.objects.objectcontext.aspx). (if it wasn't already defined for you in the designer of Visual Studio)  This is the object that does all of the heavy lifting for the Entity Framework.  If you do create an implementation of `ObjectContext`, have that implementation inherit from your repository and **bam** you have a repository implementation that you can use with Whipped Cream.

One caveat though.  The ObjectContext requires a connection string in it's constructor.  That makes it difficult for the Whipped Cream engine to create an instance of that implementation without knowing what that connection string is.

One helper that's defined within WhippedCream is the `IEntityFrameworkInitializerMap` interface.

~~~{.cs}
public interface IEntityFrameworkInitializerMap
{
	EntityFrameworkInitializer GetInitializer<TRepo>();
}

public class EntityFrameworkInitializer
{
	...

	public string ConnectionString { get; set; }
	public string Provider { get; set; }
	public string DefaultContainerName { get; set; }
	public string SSDLResource { get; set; }
	public string CSDLResource { get; set; }
	public string MSLResource { get; set; }

	public string GenerateConnnectionString() { ... }

	...
}
~~~

The `IEntityFrameworkInitializerMap` interface is meant to searve up instances of `EntityFrameworkInitializer` depending on the repository type.  The `EntityFrameworkInitializer` contains all the data necessary to create a connection string for an `ObjectContext`.

Whipped Cream doesn't register this interface within the global IOC Container, but the developer is free to implement a type inheriting from `IEntityFrameworkInitializerMap` that can be registered and used within the engine.

Here is an example of a repository implementation that extends `ObjectContext`:

~~~{.cs}
public partial class TestRepository : ObjectContext, ITestRepository, IRepositoryContext
{
    public UserRepository(WhippedCream.EntityFrameworkDataLayer.IEntityFrameworkInitializerMap map)
        : base(map.GetInitializer<IUserRepository>().GenerateConnectionString())
    {
    	this.DefaultContainerName = map.GetInitializer<IUserRepository>().DefaultContainerName;
    }

    public IQueryable<TestModel> Models
    {
    	get
    	{
    		return this.CreateObjectSet<TestModel>().AsQueryable();
    	}
    }

}
~~~

As long as IEntityFrameworkInitializer is register in the global IOC container, this object will be able to be created by Whipped Cream.

In Memory Access Layer
----------

When we're testing our software we don't want to be manipulating data within a production database.  It would be nice to keep the data within the domain of the application so that nothing outside of the software is affecting by the running tests.

However, when data is changed for the software, all places in the software that access that data need to have access to those changes.

~~~{.cs}
public interface IInMemoryPersistentMedium
{
	IList<TType> GetStorage<TRepo, TType>();
	void ClearAll();
}
~~~

The `IInMemoryPersistentMedium` object is used just for that.  The `GetStorage` method serves up a list of objects based on the generic parameters passed to the method.  The first generic parameter is the repository type.  The second is the type of entity that the list will house.  You can think of each list served up by this structure as individual tables within a database.

If our repository has two properties (`IQueryable<ObjectA>` and `IQueryable<ObjectB>`), then the corresponding tables for each property can be retrieved from this structure.

~~~{.cs}
public class TestRepositoryInMemoryImplementation
	: DataServiceUpdatableContext, ITestReposiotry
{
	public TestRepositoryInMemoryImplementation(IUnityContainer container,
												IInMemoryPersistentMedium medium)
		: base(container)
	{
		if(medium == null)
			throw new System.ArgumentNullException();

		Medium = medium;
	}

	public IInMemoryPersistentMedium Medium { get; private set; }

	public IQueryable<TestModel> Models
	{
		get
		{
			return Medium.GetStorage<ITestRepository, TestModel>().AsQueryable();
		}
	}

	#region DataServiceUpdatableContext Abstract Members
	...
	#endregion
}
~~~

One thing that is not immediatly apparent is that the In Memory Persistent Medium does not handle entity relationships at all.  We would expect the following test to pass but it would not.

~~~{.cs}
[Test]
public void InMemoryEntityRelationship()
{
	IInMemoryPersistentMedium medium = ...;
	UserModel user = new UserModel();
	user.Addresses.Add(new Address());
	user.Addresses.Add(new Address());

	medium.GetStorage<IUserRepository, User>().Add(user);

	Assert.AreEqual(2, medium.GetStorage<IUserRepository, Address>().Count);
}
~~~

![](/Images/DataLayer_Diagram2.png)
![](/Images/DataLayer_Diagram3.png "In Memory Entity Relationships")

It's important to remember this fact when working with the In Memory Access Layer.  You may be wondering how useful the In Memory Data Layer would be then if it can't mimic what the Entity Framework is doing.  Well, you would be right in wondering that and it wouldn't be useful.  However, as you'll soon see, the WCF Data Service Access Layer comes to the rescue.

WCF Data Service Access Layer
----------

We've defined two different implementations of `ITestRepository` so far.  An implementation using the Entity Framework and an implementation using Whipped Cream's In Memory Persistent Medium.  At this point, we can consider those two implementations to be our *Sugar* and *Milk* repositories.  Our Live and Testing repositories.

We can start to make a better distinction between a repository and a context now as well.  Let's consider repositories to be objects that directly access the data without any intermediary steps.  The Entity Framework directly accesses the database, and our In Memory Persitent Medium is literally a list of objects.

>	Repositories are objects that directly access their data without any intermediary steps.

The object that makes WCF Data Services go is the [DataService<T>](http://msdn.microsoft.com/en-us/library/cc646779.aspx) object.  Where *T* is the data source the service queries data from or, in our case, a repository.  Setting up a WCF Data Service is actually quite easy.  In an MVC application you would register it's route kind of like the following.

~~~{.cs}
routes.Add(new ServiceRoute("api/SomeRepository", new DataServiceHostFactory(), typeof(DataService<SomeRepository>)));
~~~

You would access that service with http://mvcproject/api/SomeRepository.

If the data source (repository) type you give the `DataService<T>` object is not an `ObjectContext` like with the Entity Framework, then WCF Data Services is forced to use reflection to figure out what data is being exposed by your data source (repository).  This is the feature that makes using our In Memory Persistent Medium work.

If your data source (repository) implements [IUpdatable](http://msdn.microsoft.com/en-us/library/system.data.services.iupdatable.aspx), then WCF Data Service is able to manage entity relationships correctly with your data source (repository).  Meaning that, if we use the `TestRepositoryInMemoryImplementation` object defined above as our data source (repository) all we'll have to do is make sure it implements `IUpdatable`.  

Well, you don't need to do that.  You may have noticed that `TestRepositoryInMemoryImplementation` inherits from `DataServiceUpdatableContext` which already implements `IUpdatable`.  `DataServiceUpdatableContext` only asks it's inheritors to define two methods.

- `GetType` and
- `SaveEntity`

See the code for `DataServiceUpdatableContext` to see documentation on what those two methods are used for.

Defining the service, much like the MVC example above, is not enough though.  WCF Data Services are rest services and serves up your data as XML or JSON.  That's pretty useless unless we're able to deserialize that text into models in our code. Much like the `ObjectContext` of the Entity Framework, WCF Data Services has the [DataServiceContext](http://msdn.microsoft.com/en-us/library/system.data.services.client.dataservicecontext.aspx).  The `DataServiceContext` is the object that communicates with WCF Data Service endpoints and deserializes the XML/JSON data into usable models in your code.

~~~{.cs}
public class TestRepositoryODataContext
	: DataServiceContext, ITestRepository
{
	public TestRepositoryODataContext(IWhippedCreamDataLayer dataLayer)
		: base(dataLayer.GetServiceUri<ITestRepository>()) { }

	public IQueryable<TestModel> Models
	{
		get
		{
			return this.CreateQuery<TestModel>("Models");
		}
	}
}
~~~

You may have noticed that, much like the `ObjectContext` needs a connection string, the `DataServiceContext` need's a service uri in order to be constructed.  Whipped Cream provides a helper method that can be used to generate the appropriate `Uri` for the service.  However, we need to decorate our repository definition with a `RepositoryServicePathAttribute` in order for it to work.

>	The RepositoryServicePathAttribute attribute is NOT required in order to use Whipped Cream.  It is only required for the `IUrlService` to generate an accurate url for your repository's service.

~~~{.cs}
[Repository(DataLayerState.Live, ...)]
[Repository(DataLayerState.Testing, ...)]
[RepositoryContext(DataLayerState.Live, ...)]
[RepositoryContext(DataLayerState.Testing, ...)]
[RepositoryServicePath("TestRepository")]
public interface ITestRepository
{
	IQueryable<TestModel> Models { get; }

	int SomeOperation();
	TestModel PageModels(int skip, int take);
}
~~~

What about the service that our `DataServiceContext` will query?  Well, we'll need to define that too.

~~~{.cs}
public partial class TestRepositoryDataService : DataService<ITestRepository>
{
	public IUnityContainer Container { get; private set; }

	public TestRepositoryDataService(IUnityContainer container)
	{
		Container = container;
	}

	protected override ITestRepository CreateDataSource()
	{
		IRepositoryFactory factory = Container.Resolve<IRepositoryFactory>();
		return factory.CreateRepository<ITestRepository>();
	}

	[WebGet]
	public int SomeOperation()
	{
		return this.CurrentDataSource.SomeOperation();
	}
	
	[WebGet]
	public IEnumerable<TestModel> PageModels(int skip, int take)
	{
		return this.CurrentDataSource.PageModels(skip, take);
	}
}
~~~

Notice that we have to manually define the operations `SomeOperation` and `PageModels` on the data service?  Don't forget to decorate those operations with a `WebGetAttribute` as well.

The Data Service and Data Context completes our *Butter* (Testing Context).  The only thing left is *Cream* (Live Context).

In this scenario, we don't need a sepperate implementation for *Cream*.  In our Live environment we always want our data to get the data directly from the database, so we can reuse our implementation for *Sugar* (Live Repository) for *Cream* and get the results we want.  As a developer, we just need to make sure we're only getting our repository implementations from the `IRepositoryContextFactory` so that we can take advantage of *Butter*.

![](/Images/DataLayer_Diagram4.png "Completed Recipe")

>	In the outlined scenario, the developer would need to make sure they only get instances of repositories from the `IRepositoryContextFactory` and not the `IRepositoryFactory`.

>	The implementation of the `DataService<T>` outlined above requires an IOC Container as a parameter in it's constructor.  The base objects used to run a `DataService<T>` expect such an implementation to have a default constructor.  Whipped Cream defines a few objects so the developer can use dependency injection with the WCF Data Service object stack.
>	- WhippedCreamDataServiceHost
>	- WhippedCreamDataServiceHostFactory
>	
>	Setting up the route in an MVC application for such a service would look similar to the following code.

~~~{.cs}		
routes.Add(new ServiceRoute("api/SomeRepository", Container.Resolve<WhippedCreamDataServiceHostFactory>(), typeof(TestRepositoryDataService)));
~~~

>	NOTE: A word of caution.  The previous code snippet would expose a service to the web that has control over the website's data.  Make sure this service is not exposed in your *Production* environment.  That could be catostrophic.

Testing
-------

The whole point of Whipped Cream is to make testing your application easier.  However, it only encapsulates one aspect of that testing.  Your application's data access.  Making your tests work is a different story.

Most in-process testing should be easy.

~~~{.cs}
[TestFixture]
public class InProcessUnitOrIntegrationTests
{
	[SetUp]
	public void SetUp()
	{
		IWhippedCreamDataLayer dl = WhippedCreamDataLayer.Container.Resolve<IWhippedCreamDataLayer>();
		//This is the important thing.  Make sure Whipped Cream is in the Testing state.
		dl.State = DataLayerState.Testing;
	}

	[Test]
	public void SomeUnitOrIntegrationTest()
	{
		...
	}
}
~~~

Simply place Whipped Cream into the *Testing* state and you're good to go.  What happens, though, when you need to test your application out of process?

An example would be a web application that is loaded into a remote testing environment and you need to run your tests either locally, or from some other remote location.  On top of that, what if that remote testing environment actually runs Whipped Cream in the Live state?  (Because, the *Testing* and *Live* flags don't mean anything beyond which repositories/contexts to create.)

![](/Images/DataLayer_Diagram5.png "Hypothetical Remote Testing Strategy")

In the image above, the Blue items represent the domain of the remote web application.  The Green items represent the domain of the tests.  The solid lines represent dependencies and the dashed lines represent data flow.

In this scenario you would still need to make sure you set Whipped Cream within your testing environment to *Testing*, but you would also need to make sure you set your remote enviroment to *Testing* as well.

You would need to expose an endpoint on your remote web application that allowed a remote agent to set the remote web application's Whipped Cream to *Testing*.

- If it's an MVC Application, expose an action on a controller to do the trick.
- Create a module that would intercept a url and set the state according to some logic.
- Setup a service that can be called by your remote test.

As you can see there are many options on how to do this, which is why Whipped Cream dosen't do this automatically for you.

Another out-of-process example I wanted to touch on is the Same Location scenario.  Let's say we have an application that can be ran along side the tests.  It would still be in a different process, but on the same machine.

![](/Images/DataLayer_Diagram6.png "Hypothetical Out-Of-Process Testing Strategy")

This would pose a unique situation where the testing environment would create the service that the application would call to access the data.  It would not require any extra code within the application under test.  It would only require the application under test to know, ahead of time, the address of the service to call and for the testing environment to make sure that service is available.

>	It is possible to run web applications in a scenario similar to the one just explained.  So, does this mean remote testing environments wouldn't be needed?  Hello Continuous Integration.

T4 Text Templating
------------------

As outlined above, we had to implement four different objects (three being implementations of `ITestRepository` and one WCF Data Service) in order to accomplish our goal of a maintainable and highly testable data centric application.  Whew.  That's a lot of objects for each repository.  If you have an application with many repositories the number of objects can quickly grow.  Even worse, incorperating a new repository would be time consuming.

[T4 Text Templating](http://msdn.microsoft.com/en-us/library/vstudio/bb126445.aspx) is a Microsoft technology that is widely used for code generation.  Visual Studio contains some out-of-box template implementations that work on the Entity Framwork.  For Whipped Cream, we were able to work with that implementation in order to come up with T4 Text Templates for the following items:

-	All of the Models
-	The Repository
-	Sugar and Milk (The Live Repository and Context)
-	Cream (The Testing Repository)
-	Milk (The Testing Context), and
-	The WCF Data Service to backup Milk

Using these templates eliviates the strain of incorperating new repositories into your application.
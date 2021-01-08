using NUnit.Framework;
using Microsoft.Practices.Unity;
using System.Data.Services;
using WhippedCream.Data.User;
using System.Linq;
using System.ServiceModel.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using WhippedCream.DataServiceDataLayer;
namespace WhippedCream
{
	namespace Data.User
	{
		public partial class UserRepositoryInMemoryRepo
		{
			partial void QueryAwards_Setup()
			{
				this._QueryAwards = (a, number) =>
					{
						if (number.HasValue)
							return a.Awards.Take(number.Value);
						else
							return Enumerable.Empty<Awards>();
					};
			}

			partial void MultiplyByTwo_Setup()
			{
				this._MultiplyByTwo = (a, number) =>
					{
						if (number.HasValue)
							return number.Value * 2;
						else
							return 0;
					};
			}
		}
	}

	[TestFixture]
	public class DataServiceContextTests
	{
		[Test]	
		public void CanCallEnumerableOperation()
		{
			IWhippedCreamDataLayer dl = Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>();
			dl.State = DataLayerState.Testing;

			using (var host = dl.GetServiceHost<IUserRepository>())
			{
				host.Open();

				WhippedCream.InMemoryDataLayer.IInMemoryPersistentMedium medium =
					Bootstrapper.Container.Resolve<WhippedCream.InMemoryDataLayer.IInMemoryPersistentMedium>();

				var table = medium.GetStorage<IUserRepository, Awards>();
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });
				table.Add(new Awards() { AwardId = Guid.NewGuid() });

				IRepositoryContextFactory factory = Bootstrapper.Container.Resolve<IRepositoryContextFactory>();
				IUserRepository repo = factory.CreateRepository<IUserRepository>();

				var result = repo.QueryAwards(5).ToArray();

				Assert.IsNotNull(result);
				Assert.AreEqual(5, result.Count());

				CollectionAssert.AreEqual(table.Take(5).Select(a => a.AwardId), result.Select(a => a.AwardId));
			}
		}

		[Test]	
		public void CanCallIntOperation()
		{
			IWhippedCreamDataLayer dl = Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>();
			dl.State = DataLayerState.Testing;

			using (var host = dl.GetServiceHost<IUserRepository>())
			{
				host.Open();

				IRepositoryContextFactory factory = Bootstrapper.Container.Resolve<IRepositoryContextFactory>();
				IUserRepository repo = factory.CreateRepository<IUserRepository>();
				int result = repo.MultiplyByTwo(31);

				Assert.AreEqual(62, result);
			}
		}

		[Test]	
		public void CanCallEnumerableOperation_Live()
		{
			IWhippedCreamDataLayer dl = Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>();
			dl.State = DataLayerState.Live;

			WhippedCream.EntityFrameworkDataLayer.IEntityFrameworkInitializerMap map =
				Bootstrapper.Container.Resolve<WhippedCream.EntityFrameworkDataLayer.IEntityFrameworkInitializerMap>();
			WhippedCream.EntityFrameworkDataLayer.EntityFrameworkInitializer init = map.GetInitializer<IUserRepository>();

			IEnumerable<Awards> table = null;
			using (ObjectContext context = new ObjectContext(init.GenerateConnectionString()))
			{
				context.DefaultContainerName = init.DefaultContainerName;

				table = context.CreateObjectSet<Awards>().Take(5).ToArray();
			}

			using (var host = dl.GetServiceHost<IUserRepository>())
			{
				host.Open();

				IRepositoryContextFactory factory = Bootstrapper.Container.Resolve<IRepositoryContextFactory>();
				IUserRepository repo = factory.CreateRepository<IUserRepository>();
				var result = repo.QueryAwards(5).ToArray();

				Assert.IsNotNull(result);
				Assert.AreEqual(5, result.Count());

				CollectionAssert.AreEqual(table.Select(a => a.AwardId), result.Select(a => a.AwardId));
			}
		}

		[Test]	
		public void CanCallIntOperation_Live()
		{
			IWhippedCreamDataLayer dl = Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>();
			dl.State = DataLayerState.Live;

			WhippedCream.EntityFrameworkDataLayer.IEntityFrameworkInitializerMap map =
				Bootstrapper.Container.Resolve<WhippedCream.EntityFrameworkDataLayer.IEntityFrameworkInitializerMap>();
			WhippedCream.EntityFrameworkDataLayer.EntityFrameworkInitializer init = map.GetInitializer<IUserRepository>();

			int expectedResult = 0;
			using (ObjectContext context = new ObjectContext(init.GenerateConnectionString()))
			{
				context.DefaultContainerName = init.DefaultContainerName;

				expectedResult = context.ExecuteFunction("MultiplyByTwo", new ObjectParameter("number", 31));
			}

			using (var host = dl.GetServiceHost<IUserRepository>())
			{
				host.Open();

				IRepositoryContextFactory factory = Bootstrapper.Container.Resolve<IRepositoryContextFactory>();
				IUserRepository repo = factory.CreateRepository<IUserRepository>();
				int result = repo.MultiplyByTwo(31);

				Assert.AreEqual(expectedResult, result);
			}
		}
	}
}

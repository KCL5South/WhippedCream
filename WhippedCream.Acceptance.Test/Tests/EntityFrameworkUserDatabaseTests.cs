using AutoPoco.Engine;
using NUnit.Framework;
using WhippedCream.InMemoryDataLayer;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using WhippedCream.Data.User;
using System.Data.Entity;
namespace WhippedCream
{
	[TestFixture]
	public class EntityFrameworkUserDatabaseTests
	{
		private void Test_FirstItemCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property,
														  System.Func<TModel, System.Guid> sortProperty)
			where TRepo : class, IRepository
			where TModel : class
		{
			using (ObjectContext context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				IObjectSet<TModel> set = context.CreateObjectSet<TModel>();
				using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<TRepo>())
				{
					(repo as DbContext).Configuration.ProxyCreationEnabled = false;

					var expected = set.OrderBy(sortProperty).First();
					var actual = property(repo).OrderBy(sortProperty).First();

					Assert.AreEqual(expected, actual, @"
The first item in the {0} property is not what was expected.
", typeof(TModel).Name);
				}
			}
		}
		private void Test_LastItemCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property,
														 System.Func<TModel, System.Guid> sortProperty)
			where TRepo : class, IRepository
			where TModel : class
		{
			using (ObjectContext context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				IObjectSet<TModel> set = context.CreateObjectSet<TModel>();
				using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<TRepo>())
				{
					int count = set.Count();
					Assert.AreEqual(set.OrderBy(sortProperty).Skip(count - 1).Take(1), property(repo).OrderBy(sortProperty).Skip(count - 1).Take(1), @"
The last item in the {0} property is not what was expected.
", typeof(TModel).Name);
				}
			}
		}
		private void Test_QueryCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property,
													  System.Func<TModel, System.Guid> sortProperty)
			where TRepo : class, IRepository
			where TModel : class
		{
			using (ObjectContext context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				IObjectSet<TModel> set = context.CreateObjectSet<TModel>();
				using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<TRepo>())
				{
					Assert.AreEqual(set.OrderBy(sortProperty).Skip(10).Take(5), property(repo).OrderBy(sortProperty).Skip(10).Take(5), @"
The query of the {0} property did not produce the correct results.
", typeof(TModel).Name);
				}
			}
		}

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>().State = DataLayerState.Live;
		}

		[Test]	
		public void Addresses()
		{
			Test_FirstItemCorrect<IUserRepository, Address>(a => a.Addresses, a => a.AddressId);
			Test_LastItemCorrect<IUserRepository, Address>(a => a.Addresses, a => a.AddressId);
			Test_QueryCorrect<IUserRepository, Address>(a => a.Addresses, a => a.AddressId);
		}

		[Test]	
		public void AgentAgencyAssociations()
		{
			Test_FirstItemCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations, a => a.AssociationId);
			Test_LastItemCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations, a => a.AssociationId);
			Test_QueryCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations, a => a.AssociationId);
		}

		[Test]	
		public void AgentIds()
		{
			Test_FirstItemCorrect<IUserRepository, AgentId>(a => a.AgentIds, a => a.AgentIdId);
			Test_LastItemCorrect<IUserRepository, AgentId>(a => a.AgentIds, a => a.AgentIdId);
			Test_QueryCorrect<IUserRepository, AgentId>(a => a.AgentIds, a => a.AgentIdId);
		}

		[Test]	
		public void Awards()
		{
			Test_FirstItemCorrect<IUserRepository, Awards>(a => a.Awards, a => a.AwardId);
			Test_LastItemCorrect<IUserRepository, Awards>(a => a.Awards, a => a.AwardId);
			Test_QueryCorrect<IUserRepository, Awards>(a => a.Awards, a => a.AwardId);
		}

		[Test]	
		public void Badges()
		{
			Test_FirstItemCorrect<IUserRepository, Badges>(a => a.Badges, a => a.BadgeId);
			Test_LastItemCorrect<IUserRepository, Badges>(a => a.Badges, a => a.BadgeId);
			Test_QueryCorrect<IUserRepository, Badges>(a => a.Badges, a => a.BadgeId);
		}

		[Test]	
		public void ContactNumbers()
		{
			Test_FirstItemCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers, a => a.ContactNumberId);
			Test_LastItemCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers, a => a.ContactNumberId);
			Test_QueryCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers, a => a.ContactNumberId);
		}

		[Test]	
		public void EMailAddress()
		{
			Test_FirstItemCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses, a => a.EMailAddressId);
			Test_LastItemCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses, a => a.EMailAddressId);
			Test_QueryCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses, a => a.EMailAddressId);
		}

		[Test]	
		public void Files()
		{
			Test_FirstItemCorrect<IUserRepository, Files>(a => a.Files, a => a.FileKey);
			Test_LastItemCorrect<IUserRepository, Files>(a => a.Files, a => a.FileKey);
			Test_QueryCorrect<IUserRepository, Files>(a => a.Files, a => a.FileKey);
		}

		[Test]	
		public void FilesFileData()
		{
			Test_FirstItemCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas, a => a.FileKey);
			Test_LastItemCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas, a => a.FileKey);
			Test_QueryCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas, a => a.FileKey);
		}

		[Test]	
		public void FileWizardInfo()
		{
			Test_FirstItemCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes, a => a.FileKey);
			Test_LastItemCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes, a => a.FileKey);
			Test_QueryCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes, a => a.FileKey);
		}

		[Test]	
		public void Preference()
		{
			Test_FirstItemCorrect<IUserRepository, Preference>(a => a.Preferences, a => a.PreferenceId);
			Test_LastItemCorrect<IUserRepository, Preference>(a => a.Preferences, a => a.PreferenceId);
			Test_QueryCorrect<IUserRepository, Preference>(a => a.Preferences, a => a.PreferenceId);
		}

		[Test]	
		public void User()
		{
			Test_FirstItemCorrect<IUserRepository, User>(a => a.Users, a => a.UserId);
			Test_LastItemCorrect<IUserRepository, User>(a => a.Users, a => a.UserId);
			Test_QueryCorrect<IUserRepository, User>(a => a.Users, a => a.UserId);
		}

		[Test]	
		public void Website()
		{
			Test_FirstItemCorrect<IUserRepository, Website>(a => a.Websites, a => a.WebsiteId);
			Test_LastItemCorrect<IUserRepository, Website>(a => a.Websites, a => a.WebsiteId);
			Test_QueryCorrect<IUserRepository, Website>(a => a.Websites, a => a.WebsiteId);
		}

		[Test]	
		public void QueryAwards()
		{
			using (var context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<IUserRepository>())
				{
					IObjectSet<Awards> awardSet = context.CreateObjectSet<Awards>();
					CollectionAssert.AreEqual(awardSet.OrderBy(a => a.AwardId).Take(10), repo.QueryAwards(10));
				}
			}
		}
		[Test]	
		public void QueryAwardsAgain()
		{
			using (var context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<IUserRepository>())
				{
					IObjectSet<Awards> awardSet = context.CreateObjectSet<Awards>();
					CollectionAssert.AreEqual(awardSet.OrderBy(a => a.AwardId).Take(17), repo.QueryAwards(17));
				}
			}
		}
		[Test]	
		public void MultiplyByTwo()
		{
			using (var context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<IUserRepository>())
				{
					ObjectParameter parameter = new ObjectParameter("number", 13);
					Assert.AreEqual(context.ExecuteFunction("MultiplyByTwo", parameter), repo.MultiplyByTwo(13), @"
	The result from the repository call was not the same as a direct call to the database.
	");
				}
			}
		}
		[Test]	
		public void MultiplyByTwoAgain()
		{
			using (var context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<IUserRepository>())
				{
					ObjectParameter parameter = new ObjectParameter("number", 27);
					Assert.AreEqual(context.ExecuteFunction("MultiplyByTwo", parameter), repo.MultiplyByTwo(27), @"
	The result from the repository call was not the same as a direct call to the database.
	");
				}
			}
		}
		[Test]	
		public void MultiplyByTwoQuiet()
		{
			using (var context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<IUserRepository>())
				{
					IObjectSet<MultiplyByTwoResult> resultSet = context.CreateObjectSet<MultiplyByTwoResult>();
					repo.MultiplyByTwoQuiet(31);

					MultiplyByTwoResult result = resultSet.ToArray().Last();
					Assert.AreEqual(62, result.Result, "The Value pulled from the database should be 62");
				}
			}
		}
		[Test]	
		public void MultiplyByTwoQuietAgain()
		{
			using (var context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<IUserRepository>())
				{
					IObjectSet<MultiplyByTwoResult> resultSet = context.CreateObjectSet<MultiplyByTwoResult>();
					repo.MultiplyByTwoQuiet(33);

					MultiplyByTwoResult result = resultSet.ToArray().Last();
					Assert.AreEqual(66, result.Result, "The Value pulled from the database should be 66");
				}
			}
		}
	}
}

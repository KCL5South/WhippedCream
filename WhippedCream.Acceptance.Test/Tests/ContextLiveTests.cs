using AutoPoco.Engine;
using NUnit.Framework;
using WhippedCream.InMemoryDataLayer;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Collections.Generic;
using System.Data.Objects;
using WhippedCream.Data.User;
using System.Data.Entity;
namespace WhippedCream
{
	[TestFixture]
	public class ContextLiveTests
	{
		private DataServiceDataLayer.WhippedCreamDataServiceHost Host;

		private void Test_FirstItemCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property,
														  System.Func<TModel, System.Guid> sortProperty)
			where TRepo : class, IRepository
			where TModel : class
		{
			using (ObjectContext context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				IObjectSet<TModel> set = context.CreateObjectSet<TModel>();
				using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<TRepo>())
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
		private void Test_QueryCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property,
													  System.Func<TModel, System.Guid> sortProperty)
			where TRepo : class, IRepository
			where TModel : class
		{
			using (ObjectContext context = Bootstrapper.GenerateObjectContext<IUserRepository>())
			{
				IObjectSet<TModel> set = context.CreateObjectSet<TModel>();
				using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<TRepo>())
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
			try
			{
				Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>().State = DataLayerState.Live;
				Host = new DataServiceDataLayer.WhippedCreamDataServiceHost(Bootstrapper.Container,
																			typeof(UserRepositoryDataService),
																			Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>().GetServiceUri<IUserRepository>());
				Host.Open();
			}
			catch
			{
				TearDown();
				throw;
			}
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			if(Host.State != System.ServiceModel.CommunicationState.Faulted)
				(Host as System.IDisposable).Dispose();
		}

		[Test]	
		public void Addresses()
		{
			Test_FirstItemCorrect<IUserRepository, Address>(a => a.Addresses, a => a.AddressId);
			Test_QueryCorrect<IUserRepository, Address>(a => a.Addresses, a => a.AddressId);
		}

		[Test]	
		public void AgentAgencyAssociations()
		{
			Test_FirstItemCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations, a => a.AssociationId);
			Test_QueryCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations, a => a.AssociationId);
		}

		[Test]	
		public void AgentIds()
		{
			Test_FirstItemCorrect<IUserRepository, AgentId>(a => a.AgentIds, a => a.AgentIdId);
			Test_QueryCorrect<IUserRepository, AgentId>(a => a.AgentIds, a => a.AgentIdId);
		}

		[Test]	
		public void Awards()
		{
			Test_FirstItemCorrect<IUserRepository, Awards>(a => a.Awards, a => a.AwardId);
			Test_QueryCorrect<IUserRepository, Awards>(a => a.Awards, a => a.AwardId);
		}

		[Test]	
		public void Badges()
		{
			Test_FirstItemCorrect<IUserRepository, Badges>(a => a.Badges, a => a.BadgeId);
			Test_QueryCorrect<IUserRepository, Badges>(a => a.Badges, a => a.BadgeId);
		}

		[Test]	
		public void ContactNumbers()
		{
			Test_FirstItemCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers, a => a.ContactNumberId);
			Test_QueryCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers, a => a.ContactNumberId);
		}

		[Test]	
		public void EMailAddress()
		{
			Test_FirstItemCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses, a => a.EMailAddressId);
			Test_QueryCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses, a => a.EMailAddressId);
		}

		[Test]	
		public void Files()
		{
			Test_FirstItemCorrect<IUserRepository, Files>(a => a.Files, a => a.FileKey);
			Test_QueryCorrect<IUserRepository, Files>(a => a.Files, a => a.FileKey);
		}

		[Test]	
		public void FilesFileData()
		{
			Test_FirstItemCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas, a => a.FileKey);
			Test_QueryCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas, a => a.FileKey);
		}

		[Test]	
		public void FileWizardInfo()
		{
			Test_FirstItemCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes, a => a.FileKey);
			Test_QueryCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes, a => a.FileKey);
		}

		[Test]	
		public void Preference()
		{
			Test_FirstItemCorrect<IUserRepository, Preference>(a => a.Preferences, a => a.PreferenceId);
			Test_QueryCorrect<IUserRepository, Preference>(a => a.Preferences, a => a.PreferenceId);
		}

		[Test]	
		public void User()
		{
			Test_FirstItemCorrect<IUserRepository, User>(a => a.Users, a => a.UserId);
			Test_QueryCorrect<IUserRepository, User>(a => a.Users, a => a.UserId);
		}

		[Test]	
		public void Website()
		{
			Test_FirstItemCorrect<IUserRepository, Website>(a => a.Websites, a => a.WebsiteId);
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

		[Test]	
		public void AddObject_Address()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Addresses.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Addresses", new Address() { UserId = targetUser.UserId, AddressId = System.Guid.NewGuid() });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Addresses.Count());
			}
		}

		[Test]	
		public void AddObject_AgentId()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.AgentIds.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("AgentIds", new AgentId() { UserId = targetUser.UserId, AgentIdId = System.Guid.NewGuid(), Value = "TestValue" });

				
				context.SaveChanges();

				Assert.AreEqual(++count, repo.AgentIds.Count());
			}
		}

		[Test]	
		public void AddObject_ContactNumber()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.ContactNumbers.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("ContactNumbers", new ContactNumber() { UserId = targetUser.UserId, ContactNumberId = System.Guid.NewGuid(), Number = "TestNumber" });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.ContactNumbers.Count());
			}
		}

		[Test]	
		public void AddObject_EMailAddress()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.EMailAddresses.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("EMailAddresses", new EMailAddress() { UserId = targetUser.UserId, EMailAddressId = System.Guid.NewGuid(), Address = "TestAddress" });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.EMailAddresses.Count());
			}
		}

		[Test]	
		public void AddObject_Preference()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Preferences.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Preferences", new Preference() { UserId = targetUser.UserId, PreferenceId = System.Guid.NewGuid() });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Preferences.Count());
			}
		}

		[Test]	
		public void AddObject_Website()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Websites.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Websites", new Website() { UserId = targetUser.UserId, WebsiteId = System.Guid.NewGuid(), Address = "TestAddress" });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Websites.Count());
			}
		}

		[Test]	
		public void AddObject_Badges()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Badges.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Badges", new Badges() { UserId = targetUser.UserId, BadgeId = System.Guid.NewGuid() });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Badges.Count());
			}
		}

		[Test]	
		public void AddObject_Awards()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Awards.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Awards", new Awards() { AwardId = System.Guid.NewGuid() });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Awards.Count());
			}
		}

		[Test]	
		public void AddObject_Files()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Files.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Files", new Files() { UserId = targetUser.UserId, FileKey = System.Guid.NewGuid(), EntryDate = System.DateTime.Now });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Files.Count());
			}
		}

		[Test]	
		public void AddObject_FilesFileData()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.FilesFileDatas.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("FilesFileDatas", new FilesFileData() { FileKey = System.Guid.NewGuid(), FileData = new byte[] { 1 } });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.FilesFileDatas.Count());
			}
		}

		[Test]	
		public void AddObject_FilesWizardInfo()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.FilesWizardInfoes.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("FilesWizardInfoes", new FilesWizardInfo() { FileKey = System.Guid.NewGuid() });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.FilesWizardInfoes.Count());
			}
		}

		[Test]	
		public void AddObject_AgentAgencyAssociation()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.AgentAgencyAssociations.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("AgentAgencyAssociations", new AgentAgencyAssociation() { AssociationId = System.Guid.NewGuid(), AgencyId = "Test", AgentId = "TestAgain" });
				context.SaveChanges();

				Assert.AreEqual(++count, repo.AgentAgencyAssociations.Count());
			}
		}

		[Test]	
		public void DeleteObject_Address()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Addresses.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new Address() { UserId = targetUser.UserId, AddressId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Addresses", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Addresses.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.Addresses.Count());
			}
		}

		[Test]	
		public void DeleteObject_AgentId()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.AgentIds.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new AgentId() { UserId = targetUser.UserId, AgentIdId = System.Guid.NewGuid(), Value = "TestValue" };
				context.AddObject("Users", targetUser);
				context.AddObject("AgentIds", target);

				
				context.SaveChanges();

				Assert.AreEqual(++count, repo.AgentIds.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.AgentIds.Count());
			}
		}

		[Test]	
		public void DeleteObject_ContactNumber()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.ContactNumbers.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new ContactNumber() { UserId = targetUser.UserId, ContactNumberId = System.Guid.NewGuid(), Number = "TestNumber" };
				context.AddObject("Users", targetUser);
				context.AddObject("ContactNumbers", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.ContactNumbers.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.ContactNumbers.Count());
			}
		}

		[Test]	
		public void DeleteObject_EMailAddress()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.EMailAddresses.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new EMailAddress() { UserId = targetUser.UserId, EMailAddressId = System.Guid.NewGuid(), Address = "TestAddress" };
				context.AddObject("Users", targetUser);
				context.AddObject("EMailAddresses", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.EMailAddresses.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.EMailAddresses.Count());
			}
		}

		[Test]	
		public void DeleteObject_Preference()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Preferences.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new Preference() { UserId = targetUser.UserId, PreferenceId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Preferences", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Preferences.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.Preferences.Count());
			}
		}

		[Test]	
		public void DeleteObject_Website()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Websites.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new Website() { UserId = targetUser.UserId, WebsiteId = System.Guid.NewGuid(), Address = "TestAddress" };
				context.AddObject("Users", targetUser);
				context.AddObject("Websites", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Websites.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.Websites.Count());
			}
		}

		[Test]	
		public void DeleteObject_Badges()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Badges.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new Badges() { UserId = targetUser.UserId, BadgeId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Badges", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Badges.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.Badges.Count());
			}
		}

		[Test]	
		public void DeleteObject_Awards()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Awards.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new Awards() { AwardId = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("Awards", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Awards.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.Awards.Count());
			}
		}

		[Test]	
		public void DeleteObject_Files()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.Files.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new Files() { UserId = targetUser.UserId, FileKey = System.Guid.NewGuid(), EntryDate = System.DateTime.Now };
				context.AddObject("Users", targetUser);
				context.AddObject("Files", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.Files.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.Files.Count());
			}
		}

		[Test]	
		public void DeleteObject_FilesFileData()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.FilesFileDatas.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new FilesFileData() { FileKey = System.Guid.NewGuid(), FileData = new byte[] { 1 } };
				context.AddObject("Users", targetUser);
				context.AddObject("FilesFileDatas", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.FilesFileDatas.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.FilesFileDatas.Count());
			}
		}

		[Test]	
		public void DeleteObject_FilesWizardInfo()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.FilesWizardInfoes.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new FilesWizardInfo() { FileKey = System.Guid.NewGuid() };
				context.AddObject("Users", targetUser);
				context.AddObject("FilesWizardInfoes", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.FilesWizardInfoes.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.FilesWizardInfoes.Count());
			}
		}

		[Test]	
		public void DeleteObject_AgentAgencyAssociation()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				int count = repo.AgentAgencyAssociations.Count(); 
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				var target = new AgentAgencyAssociation() { AssociationId = System.Guid.NewGuid(), AgencyId = "Test", AgentId = "TestAgain" };
				context.AddObject("Users", targetUser);
				context.AddObject("AgentAgencyAssociations", target);
				context.SaveChanges();

				Assert.AreEqual(++count, repo.AgentAgencyAssociations.Count());

				context.DeleteObject(target);
				context.SaveChanges();

				Assert.AreEqual(--count, repo.AgentAgencyAssociations.Count());
			}
		}

		[Test]	
		public void LoadProperty_Addresss()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				Address[] targets = new Address[] 
				{ 
					new Address() 
					{
						AddressId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new Address() 
					{
						AddressId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("Addresses", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.Addresses);

				Assert.AreEqual(repo.Addresses.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.Addresses.Count());
			}
		}
	}
}

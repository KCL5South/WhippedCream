using AutoPoco.Engine;
using NUnit.Framework;
using WhippedCream.InMemoryDataLayer;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Collections.Generic;
using WhippedCream.Data.User;
using System.Data.Services.Client;
namespace WhippedCream
{
	[TestFixture]
	public class ContextTestingTests
	{
		private IInMemoryPersistentMedium Storage;
		private DataServiceDataLayer.WhippedCreamDataServiceHost Host;

		private void Test_FirstItemCorrect<TRepo, TModel>(System.Func<IQueryable<TModel>> expectedQuery, System.Func<TRepo, IQueryable<TModel>> property)
			where TRepo : class, IRepository
			where TModel : class
		{
			using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<TRepo>())
			{
				TModel expected = expectedQuery().First() as TModel;
				TModel actual = property(repo).First() as TModel;

				Assert.IsNotNull(expected);
				Assert.IsNotNull(actual);

				Assert.AreEqual(expected, actual, @"
The first item in the {0} property is not what was expected.
", typeof(TModel).Name);
			}
		}

		private void Test_QueryCorrect<TRepo, TModel>(System.Func<IQueryable<TModel>> expectedQuery, System.Func<TRepo, IQueryable<TModel>> property)
			where TRepo : class, IRepository
		{
			using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<TRepo>())
			{
				Assert.AreEqual(expectedQuery().Skip(10).Take(5), property(repo).Skip(10).Take(5), @"
The query of the {0} property did not produce the correct results.
", typeof(TModel).Name);
			}
		}

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			try
			{
				Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>().State = DataLayerState.Testing;
				Storage = Bootstrapper.Container.Resolve<IInMemoryPersistentMedium>();
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

		[SetUp]
		public void SetUp()
		{
			Storage.ClearAll();

			IGenerationSession session = Bootstrapper.GenerateFactory().CreateSession();
			foreach (User user in session.Collection<User>(100))
			{
				Storage.GetStorage<IUserRepository, User>().Add(user);

				foreach (Address target in user.Addresses.ToArray())
				{
					user.Addresses.Remove(target);
					target.User = null;
					Storage.GetStorage<IUserRepository, Address>().Add(target);
				}
				foreach (AgentId target in user.AgentIds.ToArray())
				{
					user.AgentIds.Remove(target);
					target.User = null;
					Storage.GetStorage<IUserRepository, AgentId>().Add(target);
				}
				foreach (ContactNumber target in user.ContactNumbers.ToArray())
					Storage.GetStorage<IUserRepository, ContactNumber>().Add(target);
				foreach (EMailAddress target in user.EMailAddresses.ToArray())
					Storage.GetStorage<IUserRepository, EMailAddress>().Add(target);
				foreach (Preference target in user.Preferences.ToArray())
					Storage.GetStorage<IUserRepository, Preference>().Add(target);
				foreach (Website target in user.Websites.ToArray())
					Storage.GetStorage<IUserRepository, Website>().Add(target);
				foreach (Badges target in user.Badges.ToArray())
					Storage.GetStorage<IUserRepository, Badges>().Add(target);
			}
			foreach (Awards target in session.Collection<Awards>(100))
			{
				Storage.GetStorage<IUserRepository, Awards>().Add(target);
			}
			foreach (Files target in session.Collection<Files>(100))
			{
				Storage.GetStorage<IUserRepository, Files>().Add(target);
			}
			foreach (FilesFileData target in session.Collection<FilesFileData>(100))
			{
				Storage.GetStorage<IUserRepository, FilesFileData>().Add(target);
			}
			foreach (FilesWizardInfo target in session.Collection<FilesWizardInfo>(100))
			{
				Storage.GetStorage<IUserRepository, FilesWizardInfo>().Add(target);
			}
			foreach (AgentAgencyAssociation target in session.Collection<AgentAgencyAssociation>(100))
			{
				Storage.GetStorage<IUserRepository, AgentAgencyAssociation>().Add(target);
			}
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			if (Host.State != System.ServiceModel.CommunicationState.Faulted)
				(Host as System.IDisposable).Dispose();
		}

		[Test]	
		public void Addresses()
		{
			Test_FirstItemCorrect<IUserRepository, Address>(() => Storage.GetStorage<IUserRepository, Address>().AsQueryable().OrderBy(a => a.AddressId), 
															a => a.Addresses.OrderBy(b => b.AddressId));
			Test_QueryCorrect<IUserRepository, Address>(() => Storage.GetStorage<IUserRepository, Address>().AsQueryable().OrderBy(a => a.AddressId), 
														a => a.Addresses.OrderBy(b => b.AddressId));
		}

		[Test]	
		public void AgentAgencyAssociations()
		{
			Test_FirstItemCorrect<IUserRepository, AgentAgencyAssociation>(() => Storage.GetStorage<IUserRepository, AgentAgencyAssociation>().AsQueryable().OrderBy(a => a.AssociationId),
															a => a.AgentAgencyAssociations.OrderBy(b => b.AssociationId));
			Test_QueryCorrect<IUserRepository, AgentAgencyAssociation>(() => Storage.GetStorage<IUserRepository, AgentAgencyAssociation>().AsQueryable().OrderBy(a => a.AssociationId),
														a => a.AgentAgencyAssociations.OrderBy(b => b.AssociationId));
		}

		[Test]	
		public void AgentIds()
		{
			Test_FirstItemCorrect<IUserRepository, AgentId>(() => Storage.GetStorage<IUserRepository, AgentId>().AsQueryable().OrderBy(a => a.AgentIdId),
															a => a.AgentIds.OrderBy(b => b.AgentIdId));
			Test_QueryCorrect<IUserRepository, AgentId>(() => Storage.GetStorage<IUserRepository, AgentId>().AsQueryable().OrderBy(a => a.AgentIdId),
														a => a.AgentIds.OrderBy(b => b.AgentIdId));
		}

		[Test]	
		public void Awards()
		{
			Test_FirstItemCorrect<IUserRepository, Awards>(() => Storage.GetStorage<IUserRepository, Awards>().AsQueryable().OrderBy(a => a.AwardId),
															a => a.Awards.OrderBy(b => b.AwardId));
			Test_QueryCorrect<IUserRepository, Awards>(() => Storage.GetStorage<IUserRepository, Awards>().AsQueryable().OrderBy(a => a.AwardId),
														a => a.Awards.OrderBy(b => b.AwardId));
		}

		[Test]	
		public void Badges()
		{
			Test_FirstItemCorrect<IUserRepository, Badges>(() => Storage.GetStorage<IUserRepository, Badges>().AsQueryable().OrderBy(a => a.BadgeId),
															a => a.Badges.OrderBy(b => b.BadgeId));
			Test_QueryCorrect<IUserRepository, Badges>(() => Storage.GetStorage<IUserRepository, Badges>().AsQueryable().OrderBy(a => a.BadgeId),
														a => a.Badges.OrderBy(b => b.BadgeId));
		}

		[Test]	
		public void ContactNumbers()
		{
			Test_FirstItemCorrect<IUserRepository, ContactNumber>(() => Storage.GetStorage<IUserRepository, ContactNumber>().AsQueryable().OrderBy(a => a.ContactNumberId),
															a => a.ContactNumbers.OrderBy(b => b.ContactNumberId));
			Test_QueryCorrect<IUserRepository, ContactNumber>(() => Storage.GetStorage<IUserRepository, ContactNumber>().AsQueryable().OrderBy(a => a.ContactNumberId),
														a => a.ContactNumbers.OrderBy(b => b.ContactNumberId));
		}

		[Test]	
		public void EMailAddress()
		{
			Test_FirstItemCorrect<IUserRepository, EMailAddress>(() => Storage.GetStorage<IUserRepository, EMailAddress>().AsQueryable().OrderBy(a => a.EMailAddressId),
															a => a.EMailAddresses.OrderBy(b => b.EMailAddressId));
			Test_QueryCorrect<IUserRepository, EMailAddress>(() => Storage.GetStorage<IUserRepository, EMailAddress>().AsQueryable().OrderBy(a => a.EMailAddressId),
														a => a.EMailAddresses.OrderBy(b => b.EMailAddressId));
		}

		[Test]	
		public void Files()
		{
			Test_FirstItemCorrect<IUserRepository, Files>(() => Storage.GetStorage<IUserRepository, Files>().AsQueryable().OrderBy(a => a.FileKey),
															a => a.Files.OrderBy(b => b.FileKey));
			Test_QueryCorrect<IUserRepository, Files>(() => Storage.GetStorage<IUserRepository, Files>().AsQueryable().OrderBy(a => a.FileKey),
														a => a.Files.OrderBy(b => b.FileKey));
		}

		[Test]	
		public void FilesFileData()
		{
			Test_FirstItemCorrect<IUserRepository, FilesFileData>(() => Storage.GetStorage<IUserRepository, FilesFileData>().AsQueryable().OrderBy(a => a.FileKey),
															a => a.FilesFileDatas.OrderBy(b => b.FileKey));
			Test_QueryCorrect<IUserRepository, FilesFileData>(() => Storage.GetStorage<IUserRepository, FilesFileData>().AsQueryable().OrderBy(a => a.FileKey),
														a => a.FilesFileDatas.OrderBy(b => b.FileKey));
		}

		[Test]	
		public void FileWizardInfo()
		{
			Test_FirstItemCorrect<IUserRepository, FilesWizardInfo>(() => Storage.GetStorage<IUserRepository, FilesWizardInfo>().AsQueryable().OrderBy(a => a.FileKey),
															a => a.FilesWizardInfoes.OrderBy(b => b.FileKey));
			Test_QueryCorrect<IUserRepository, FilesWizardInfo>(() => Storage.GetStorage<IUserRepository, FilesWizardInfo>().AsQueryable().OrderBy(a => a.FileKey),
														a => a.FilesWizardInfoes.OrderBy(b => b.FileKey));
		}

		[Test]	
		public void Preference()
		{
			Test_FirstItemCorrect<IUserRepository, Preference>(() => Storage.GetStorage<IUserRepository, Preference>().AsQueryable().OrderBy(a => a.PreferenceId),
															a => a.Preferences.OrderBy(b => b.PreferenceId));
			Test_QueryCorrect<IUserRepository, Preference>(() => Storage.GetStorage<IUserRepository, Preference>().AsQueryable().OrderBy(a => a.PreferenceId),
														a => a.Preferences.OrderBy(b => b.PreferenceId));
		}

		[Test]	
		public void User()
		{
			Test_FirstItemCorrect<IUserRepository, User>(() => Storage.GetStorage<IUserRepository, User>().AsQueryable().OrderBy(a => a.UserId),
															a => a.Users.OrderBy(b => b.UserId));
			Test_QueryCorrect<IUserRepository, User>(() => Storage.GetStorage<IUserRepository, User>().AsQueryable().OrderBy(a => a.UserId),
														a => a.Users.OrderBy(b => b.UserId));
		}

		[Test]	
		public void Website()
		{
			Test_FirstItemCorrect<IUserRepository, Website>(() => Storage.GetStorage<IUserRepository, Website>().AsQueryable().OrderBy(a => a.WebsiteId),
															a => a.Websites.OrderBy(b => b.WebsiteId));
			Test_QueryCorrect<IUserRepository, Website>(() => Storage.GetStorage<IUserRepository, Website>().AsQueryable().OrderBy(a => a.WebsiteId),
														a => a.Websites.OrderBy(b => b.WebsiteId));
		}

		public System.Collections.IEnumerable AddObjectTestCaseSource()
		{
			yield return new TestCaseData(new Address() { UserId = System.Guid.NewGuid(), AddressId = System.Guid.NewGuid() }, "Addresses");
			yield return new TestCaseData(new AgentId() { UserId = System.Guid.NewGuid(), AgentIdId= System.Guid.NewGuid() }, "AgentIds");
			yield return new TestCaseData(new ContactNumber() { UserId = System.Guid.NewGuid(), ContactNumberId = System.Guid.NewGuid() }, "ContactNumbers");
			yield return new TestCaseData(new EMailAddress() { UserId = System.Guid.NewGuid(), EMailAddressId = System.Guid.NewGuid() }, "EMailAddresses");
			yield return new TestCaseData(new Preference() { UserId = System.Guid.NewGuid(), PreferenceId = System.Guid.NewGuid() }, "Preferences");
			yield return new TestCaseData(new Website() { UserId = System.Guid.NewGuid(), WebsiteId = System.Guid.NewGuid() }, "Websites");
			yield return new TestCaseData(new Badges() { UserId = System.Guid.NewGuid(), BadgeId = System.Guid.NewGuid() }, "Badges");
			yield return new TestCaseData(new Awards() { AwardId = System.Guid.NewGuid() }, "Awards");
			yield return new TestCaseData(new Files() { FileKey = System.Guid.NewGuid() }, "Files");
			yield return new TestCaseData(new FilesFileData() { FileKey = System.Guid.NewGuid() }, "FilesFileDatas");
			yield return new TestCaseData(new FilesWizardInfo() { FileKey = System.Guid.NewGuid() }, "FilesWizardInfoes");
			yield return new TestCaseData(new AgentAgencyAssociation() { AssociationId = System.Guid.NewGuid() }, "AgentAgencyAssociations");
		}

		[Test]	
		[TestCaseSource("AddObjectTestCaseSource")]
		public void AddObject<T>(T model, string entityCollection)
		{
			int count = Storage.GetStorage<IUserRepository, T>().Count;
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				context.AddObject(entityCollection, model);
				context.SaveChanges();

				Assert.AreEqual(++count, Storage.GetStorage<IUserRepository, T>().Count);
			}
		}

		public System.Collections.IEnumerable DeleteObjectTestCaseSource()
		{
			yield return new TestCaseData(new Address() { UserId = System.Guid.NewGuid(), AddressId = System.Guid.NewGuid() }, "Addresses");
			yield return new TestCaseData(new AgentId() { UserId = System.Guid.NewGuid(), AgentIdId= System.Guid.NewGuid() }, "AgentIds");
			yield return new TestCaseData(new ContactNumber() { UserId = System.Guid.NewGuid(), ContactNumberId = System.Guid.NewGuid() }, "ContactNumbers");
			yield return new TestCaseData(new EMailAddress() { UserId = System.Guid.NewGuid(), EMailAddressId = System.Guid.NewGuid() }, "EMailAddresses");
			yield return new TestCaseData(new Preference() { UserId = System.Guid.NewGuid(), PreferenceId = System.Guid.NewGuid() }, "Preferences");
			yield return new TestCaseData(new Website() { UserId = System.Guid.NewGuid(), WebsiteId = System.Guid.NewGuid() }, "Websites");
			yield return new TestCaseData(new Badges() { UserId = System.Guid.NewGuid(), BadgeId = System.Guid.NewGuid() }, "Badges");
			yield return new TestCaseData(new Awards() { AwardId = System.Guid.NewGuid() }, "Awards");
			yield return new TestCaseData(new Files() { FileKey = System.Guid.NewGuid() }, "Files");
			yield return new TestCaseData(new FilesFileData() { FileKey = System.Guid.NewGuid() }, "FilesFileDatas");
			yield return new TestCaseData(new FilesWizardInfo() { FileKey = System.Guid.NewGuid() }, "FilesWizardInfoes");
			yield return new TestCaseData(new AgentAgencyAssociation() { AssociationId = System.Guid.NewGuid() }, "AgentAgencyAssociations");
		}

		[Test]	
		[TestCaseSource("DeleteObjectTestCaseSource")]
		public void DeleteObject<T>(T model, string entityCollection)
		{
			int count = Storage.GetStorage<IUserRepository, T>().Count;
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				context.AddObject(entityCollection, model);
				context.SaveChanges();

				Assert.AreEqual(++count, Storage.GetStorage<IUserRepository, T>().Count);

				context.DeleteObject(model);
				context.SaveChanges();

				Assert.AreEqual(--count, Storage.GetStorage<IUserRepository, T>().Count);
			}
		}

		[Test]	
		public void LoadProperty_AgentIds()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				AgentId[] targets = new AgentId[] 
				{ 
					new AgentId() 
					{
						AgentIdId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new AgentId() 
					{
						AgentIdId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("AgentIds", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.AgentIds);

				Assert.AreEqual(repo.AgentIds.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.AgentIds.Count());
			}
		}

		[Test]	
		public void LoadProperty_Badges()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				Badges[] targets = new Badges[] 
				{ 
					new Badges() 
					{
						BadgeId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new Badges() 
					{
						BadgeId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("Badges", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.Badges);

				Assert.AreEqual(repo.Badges.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.Badges.Count());
			}
		}

		[Test]	
		public void LoadProperty_ContactNumbers()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				ContactNumber[] targets = new ContactNumber[] 
				{ 
					new ContactNumber() 
					{
						ContactNumberId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new ContactNumber() 
					{
						ContactNumberId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("ContactNumbers", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.ContactNumbers);

				Assert.AreEqual(repo.ContactNumbers.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.ContactNumbers.Count());
			}
		}

		[Test]	
		public void LoadProperty_EMailAddresses()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				EMailAddress[] targets = new EMailAddress[] 
				{ 
					new EMailAddress() 
					{
						EMailAddressId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new EMailAddress() 
					{
						EMailAddressId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("EMailAddresses", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.EMailAddresses);

				Assert.AreEqual(repo.EMailAddresses.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.EMailAddresses.Count());
			}
		}

		[Test]	
		public void LoadProperty_Preferences()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				Preference[] targets = new Preference[] 
				{ 
					new Preference() 
					{
						PreferenceId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new Preference() 
					{
						PreferenceId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("Preferences", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.Preferences);

				Assert.AreEqual(repo.Preferences.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.Preferences.Count());
			}
		}

		[Test]	
		public void LoadProperty_Websites()
		{
			using (IUserRepository repo = Bootstrapper.Container.Resolve<IRepositoryContextFactory>().CreateRepository<IUserRepository>())
			{
				IRepositoryContext context = repo as IRepositoryContext;
				Assert.IsNotNull(context);

				User targetUser = new User() { UserId = System.Guid.NewGuid() };
				Website[] targets = new Website[] 
				{ 
					new Website() 
					{
						WebsiteId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					},
					new Website() 
					{
						WebsiteId = System.Guid.NewGuid(),
						UserId = targetUser.UserId 
					}
				};
				context.AddObject("Users", targetUser);
				context.SaveChanges();
				foreach(var target in targets)
					context.AddObject("Websites", target);
				context.SaveChanges();

				context.LoadProperty<User>(targetUser, a => a.Websites);

				Assert.AreEqual(repo.Websites.Where(a => a.UserId == targetUser.UserId).Count(), targetUser.Websites.Count());
			}
		}
	}
}
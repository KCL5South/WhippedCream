using AutoPoco.Engine;
using NUnit.Framework;
using WhippedCream.InMemoryDataLayer;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Collections.Generic;
using WhippedCream.Data.User;
namespace WhippedCream
{
	[TestFixture]
	public class InMemoryUserDatabaseTests
	{
		private IInMemoryPersistentMedium Storage;

		private void Test_FirstItemCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property)
			where TRepo : class, IRepository
		{
			using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<TRepo>())
			{
				Assert.AreEqual(Storage.GetStorage<TRepo, TModel>().First(), property(repo).First(), @"
The first item in the {0} property is not what was expected.
", typeof(TModel).Name);
			}
		}

		private void Test_LastItemCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property)
			where TRepo : class, IRepository
		{
			using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<TRepo>())
			{
				Assert.AreEqual(Storage.GetStorage<TRepo, TModel>().Last(), property(repo).Last(), @"
The last item in the {0} property is not what was expected.
", typeof(TModel).Name);
			}
		}

		private void Test_QueryCorrect<TRepo, TModel>(System.Func<TRepo, IQueryable<TModel>> property)
			where TRepo : class, IRepository
		{
			using (TRepo repo = Bootstrapper.Container.Resolve<IRepositoryFactory>().CreateRepository<TRepo>())
			{
				Assert.AreEqual(Storage.GetStorage<TRepo, TModel>().Skip(10).Take(5), property(repo).Skip(10).Take(5), @"
The query of the {0} property did not produce the correct results.
", typeof(TModel).Name);
			}
		}

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			Bootstrapper.Container.Resolve<IWhippedCreamDataLayer>().State = DataLayerState.Testing;
			Storage = Bootstrapper.Container.Resolve<IInMemoryPersistentMedium>();
		}

		[SetUp]
		public void SetUp()
		{
			Storage.ClearAll();

			IGenerationSession session = Bootstrapper.GenerateFactory().CreateSession();
			foreach (User user in session.Collection<User>(100))
			{
				Storage.GetStorage<IUserRepository, User>().Add(user);

				foreach (Address target in user.Addresses)
					Storage.GetStorage<IUserRepository, Address>().Add(target);
				foreach (AgentId target in user.AgentIds)
					Storage.GetStorage<IUserRepository, AgentId>().Add(target);
				foreach (ContactNumber target in user.ContactNumbers)
					Storage.GetStorage<IUserRepository, ContactNumber>().Add(target);
				foreach (EMailAddress target in user.EMailAddresses)
					Storage.GetStorage<IUserRepository, EMailAddress>().Add(target);
				foreach (Preference target in user.Preferences)
					Storage.GetStorage<IUserRepository, Preference>().Add(target);
				foreach (Website target in user.Websites)
					Storage.GetStorage<IUserRepository, Website>().Add(target);
				foreach (Badges target in user.Badges)
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

		[Test]
		public void Addresses()
		{
			Test_FirstItemCorrect<IUserRepository, Address>(a => a.Addresses);
			Test_LastItemCorrect<IUserRepository, Address>(a => a.Addresses);
			Test_QueryCorrect<IUserRepository, Address>(a => a.Addresses);
		}

		[Test]
		public void AgentAgencyAssociations()
		{
			Test_FirstItemCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations);
			Test_LastItemCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations);
			Test_QueryCorrect<IUserRepository, AgentAgencyAssociation>(a => a.AgentAgencyAssociations);
		}

		[Test]
		public void AgentIds()
		{
			Test_FirstItemCorrect<IUserRepository, AgentId>(a => a.AgentIds);
			Test_LastItemCorrect<IUserRepository, AgentId>(a => a.AgentIds);
			Test_QueryCorrect<IUserRepository, AgentId>(a => a.AgentIds);
		}

		[Test]
		public void Awards()
		{
			Test_FirstItemCorrect<IUserRepository, Awards>(a => a.Awards);
			Test_LastItemCorrect<IUserRepository, Awards>(a => a.Awards);
			Test_QueryCorrect<IUserRepository, Awards>(a => a.Awards);
		}

		[Test]
		public void Badges()
		{
			Test_FirstItemCorrect<IUserRepository, Badges>(a => a.Badges);
			Test_LastItemCorrect<IUserRepository, Badges>(a => a.Badges);
			Test_QueryCorrect<IUserRepository, Badges>(a => a.Badges);
		}

		[Test]
		public void ContactNumbers()
		{
			Test_FirstItemCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers);
			Test_LastItemCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers);
			Test_QueryCorrect<IUserRepository, ContactNumber>(a => a.ContactNumbers);
		}

		[Test]
		public void EMailAddress()
		{
			Test_FirstItemCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses);
			Test_LastItemCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses);
			Test_QueryCorrect<IUserRepository, EMailAddress>(a => a.EMailAddresses);
		}

		[Test]
		public void Files()
		{
			Test_FirstItemCorrect<IUserRepository, Files>(a => a.Files);
			Test_LastItemCorrect<IUserRepository, Files>(a => a.Files);
			Test_QueryCorrect<IUserRepository, Files>(a => a.Files);
		}

		[Test]
		public void FilesFileData()
		{
			Test_FirstItemCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas);
			Test_LastItemCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas);
			Test_QueryCorrect<IUserRepository, FilesFileData>(a => a.FilesFileDatas);
		}

		[Test]
		public void FileWizardInfo()
		{
			Test_FirstItemCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes);
			Test_LastItemCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes);
			Test_QueryCorrect<IUserRepository, FilesWizardInfo>(a => a.FilesWizardInfoes);
		}

		[Test]
		public void Preference()
		{
			Test_FirstItemCorrect<IUserRepository, Preference>(a => a.Preferences);
			Test_LastItemCorrect<IUserRepository, Preference>(a => a.Preferences);
			Test_QueryCorrect<IUserRepository, Preference>(a => a.Preferences);
		}

		[Test]
		public void User()
		{
			Test_FirstItemCorrect<IUserRepository, User>(a => a.Users);
			Test_LastItemCorrect<IUserRepository, User>(a => a.Users);
			Test_QueryCorrect<IUserRepository, User>(a => a.Users);
		}

		[Test]
		public void Website()
		{
			Test_FirstItemCorrect<IUserRepository, Website>(a => a.Websites);
			Test_LastItemCorrect<IUserRepository, Website>(a => a.Websites);
			Test_QueryCorrect<IUserRepository, Website>(a => a.Websites);
		}
	}
}
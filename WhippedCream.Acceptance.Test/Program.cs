using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Linq;
using AutoPoco;
using AutoPoco.Engine;
using AutoPoco.KCL.DataSources;
using Microsoft.Practices.Unity;
using WhippedCream.Data.User;
namespace WhippedCream
{
    public static class Program
    {
        private static IGenerationSessionFactory GenerateFactory()
        {
            return AutoPocoContainer.Configure(x =>
            {
                x.Conventions(c =>
                {
					c.UseDefaultConventions();
                });
                x.AddFromAssemblyContainingType<User>();

                x.Include<Address>()
                    .Setup(a => a.AddressId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.City).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.POBox).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.PrimaryAddress).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.SecondaryAddress).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.State).Use<AutoPoco.DataSources.RandomStringSource>(2, 2)
                    .Setup(a => a.Suite).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.Type).Use<RandomInt32DataSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId))
                    .Setup(a => a.ZipCode).Use<AutoPoco.DataSources.AutoSource<ZipCode>>();

                x.Include<AgentAgencyAssociation>()
                    .Setup(a => a.AgencyId).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.AgentId).Use<AutoPoco.DataSources.RandomStringSource>(6, 6)
                    .Setup(a => a.AssociationId).Use<AutoPoco.DataSources.GuidSource>();

                x.Include<AgentId>()
                    .Setup(a => a.AgentIdId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.Type).Use<RandomInt32DataSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId))
                    .Setup(a => a.Value).Use<AutoPoco.DataSources.RandomStringSource>(6, 6);

                x.Include<Awards>()
                    .Setup(a => a.AwardId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.DateReceived).Use<DataSources.RandomNullableDateTimeSource>()
                    .Setup(a => a.Description).Use<AutoPoco.DataSources.RandomStringSource>(5, 10)
                    .Setup(a => a.Type).Use<DataSources.RandomNullableIntSource>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId));

                x.Include<Badges>()
                    .Setup(a => a.BadgeId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.DateReceived).Use<DataSources.RandomNullableDateTimeSource>()
                    .Setup(a => a.Description).Use<AutoPoco.DataSources.RandomStringSource>(5, 10)
                    .Setup(a => a.Type).Use<DataSources.RandomNullableIntSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId));

                x.Include<ContactNumber>()
                    .Setup(a => a.ContactNumberId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.Extension).Use<AutoPoco.DataSources.RandomStringSource>(4, 5)
                    .Setup(a => a.Number).Use<AutoPoco.DataSources.RandomStringSource>(10, 10)
                    .Setup(a => a.Type).Use<RandomInt32DataSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId));

                x.Include<EMailAddress>()
                    .Setup(a => a.Address).Use<AutoPoco.DataSources.RandomStringSource>(20, 25)
                    .Setup(a => a.EMailAddressId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.Type).Use<RandomInt32DataSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId));

                x.Include<Files>()
                    .Setup(a => a.EntryDate).Use<DataSources.RandomDateTimeSource>()
                    .Setup(a => a.FileKey).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.FileType).Use<RandomInt32DataSource>()
                    .Setup(a => a.UserId).Use<AutoPoco.DataSources.GuidSource>();

                x.Include<FilesFileData>()
                    .Setup(a => a.FileKey).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.FileData).Use<DataSources.RandomByteArraySource>(1000, 4000);

                x.Include<FilesWizardInfo>()
                    .Setup(a => a.ClientFirstName).Use<AutoPoco.DataSources.FirstNameSource>()
                    .Setup(a => a.ClientLastName).Use<AutoPoco.DataSources.LastNameSource>()
                    .Setup(a => a.FileKey).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.RunDate).Use<DataSources.RandomNullableDateTimeSource>();

                x.Include<Name>()
                    .Setup(a => a.First).Use<AutoPoco.DataSources.RandomStringSource>(10, 10)
                    .Setup(a => a.Last).Use<AutoPoco.DataSources.RandomStringSource>(10, 10)
                    .Setup(a => a.Middle).Use<AutoPoco.DataSources.RandomStringSource>(10, 10)
                    .Setup(a => a.Prefix).Use<AutoPoco.DataSources.RandomStringSource>(2, 3)
                    .Setup(a => a.Suffix).Use<AutoPoco.DataSources.RandomStringSource>(0, 10);

                x.Include<Preference>()
                    .Setup(a => a.PreferenceId).Use<AutoPoco.DataSources.GuidSource>()
                    .Setup(a => a.Type).Use<RandomInt32DataSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId))
                    .Setup(a => a.Value).Use<AutoPoco.DataSources.RandomStringSource>(10, 25);

				x.Include<User>()
					.Setup(a => a.UsernameId).Use<AutoPoco.DataSources.GuidSource>()
					.Setup(a => a.UserId).Use<AutoPoco.DataSources.GuidSource>()
					.Setup(a => a.Name).Use<AutoPoco.DataSources.AutoSource<Name>>()
					.Setup(a => a.Type).Use<DataSources.RandomNullableIntSource>()
					.Setup(a => a.Addresses).Use<DataSources.CollectionDataSource<Address>>(0, 5)
					.Setup(a => a.AgentIds).Use<DataSources.CollectionDataSource<AgentId>>(0, 5)
					.Setup(a => a.Badges).Use<DataSources.CollectionDataSource<Badges>>(0, 5)
					.Setup(a => a.ContactNumbers).Use<DataSources.CollectionDataSource<ContactNumber>>(0, 5)
					.Setup(a => a.EMailAddresses).Use<DataSources.CollectionDataSource<EMailAddress>>(0, 5)
					.Setup(a => a.Preferences).Use<DataSources.CollectionDataSource<Preference>>(0, 5)
					.Setup(a => a.Websites).Use<DataSources.CollectionDataSource<Website>>(0, 5);

                x.Include<Website>()
                    .Setup(a => a.Address).Use<AutoPoco.DataSources.RandomStringSource>(25, 50)
                    .Setup(a => a.Type).Use<RandomInt32DataSource>()
                    .Setup(a => a.User).Use<AutoPoco.DataSources.ParentSource<User>>()
                    .Setup(a => a.UserId).Use<DataSources.IdSameAsParentSource<User, System.Guid>>(new System.Func<User, System.Guid>(a => a.UserId))
                    .Setup(a => a.WebsiteId).Use<AutoPoco.DataSources.GuidSource>();

                x.Include<ZipCode>()
                    .Setup(a => a.Primary).Use<AutoPoco.DataSources.RandomStringSource>(5, 5)
                    .Setup(a => a.Secondary).Use<AutoPoco.DataSources.RandomStringSource>(4, 4);
            });

        }

        public static int Main(string[] args)
        {
			try
			{
				Bootstrapper.Bootstrap();

				using (ObjectContext userDBContext = Bootstrapper.GenerateObjectContext<IUserRepository>())
				{
					IObjectSet<User> userSet = userDBContext.CreateObjectSet<User>();
					IObjectSet<Awards> awardsSet = userDBContext.CreateObjectSet<Awards>();
					IObjectSet<Files> filesSet = userDBContext.CreateObjectSet<Files>();
					IObjectSet<FilesFileData> filesFileData = userDBContext.CreateObjectSet<FilesFileData>();
					IObjectSet<FilesWizardInfo> filesWizardInfoData = userDBContext.CreateObjectSet<FilesWizardInfo>();
					IObjectSet<AgentAgencyAssociation> aaAssSet = userDBContext.CreateObjectSet<AgentAgencyAssociation>();
					IObjectSet<LoginEntry> loginSet = userDBContext.CreateObjectSet<LoginEntry>();

					IGenerationSessionFactory factory = GenerateFactory();

					IGenerationSession session = factory.CreateSession();

					foreach (User user in session.Collection<User>(100))
					{
						userSet.AddObject(user);
					}

					foreach (Awards target in session.Collection<Awards>(100))
					{
						awardsSet.AddObject(target);
					}

					foreach (Files target in session.Collection<Files>(100))
					{
						filesSet.AddObject(target);
					}

					foreach (FilesFileData target in session.Collection<FilesFileData>(100))
					{
						filesFileData.AddObject(target);
					}

					foreach (FilesWizardInfo target in session.Collection<FilesWizardInfo>(100))
					{
						filesWizardInfoData.AddObject(target);
					}

					foreach (AgentAgencyAssociation target in session.Collection<AgentAgencyAssociation>(100))
					{
						aaAssSet.AddObject(target);
					}

					userDBContext.SaveChanges();
				}

				System.Console.WriteLine("Successfully populated the User Database.");

				return 0;
			}
			catch (System.Exception ex)
			{
				string message = ex.Message;
				System.Exception curEx = ex.InnerException;
				while (curEx != null)
				{
					message = string.Format("{0}{1}{2}", message, System.Environment.NewLine, curEx.Message);
					curEx = curEx.InnerException;
				}
				System.Console.Error.WriteLine(message);
				return 1;
			}
        }
    }
}
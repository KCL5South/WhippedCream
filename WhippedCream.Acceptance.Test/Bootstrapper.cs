using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using Microsoft.Practices.Unity;
using System.Linq;
using AutoPoco.Engine;
using AutoPoco;
using AutoPoco.KCL.DataSources;
using NUnit.Framework;
using WhippedCream.Data.User;
using System.Net.NetworkInformation;
namespace WhippedCream
{
    [SetUpFixture]
    public class Bootstrapper
    {
        public static IUnityContainer Container { get; private set; }
        
        internal static IGenerationSessionFactory GenerateFactory()
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
					.Setup(a => a.Addresses).Use<DataSources.CollectionDataSource<Address>>(1, 5)
					.Setup(a => a.AgentIds).Use<DataSources.CollectionDataSource<AgentId>>(1, 5)
					.Setup(a => a.Badges).Use<DataSources.CollectionDataSource<Badges>>(1, 5)
					.Setup(a => a.ContactNumbers).Use<DataSources.CollectionDataSource<ContactNumber>>(1, 5)
					.Setup(a => a.EMailAddresses).Use<DataSources.CollectionDataSource<EMailAddress>>(1, 5)
					.Setup(a => a.Preferences).Use<DataSources.CollectionDataSource<Preference>>(1, 5)
					.Setup(a => a.Websites).Use<DataSources.CollectionDataSource<Website>>(1, 5);

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

        internal static ObjectContext GenerateObjectContext<TRepo>()
        {
            EntityFrameworkDataLayer.EntityFrameworkInitializer initializer =
                       Bootstrapper.Container.Resolve<EntityFrameworkDataLayer.IEntityFrameworkInitializerMap>()
						   .GetInitializer<TRepo>();

            EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder();
            builder.ProviderConnectionString = initializer.ConnectionString;
            builder.Provider = initializer.Provider;
            builder.Metadata = string.Join("|", initializer.CSDLResource,
                                                initializer.SSDLResource,
                                                initializer.MSLResource);

            ObjectContext result = new ObjectContext(builder.ToString());
            result.DefaultContainerName = initializer.DefaultContainerName;
            result.ContextOptions.LazyLoadingEnabled = false;
            result.ContextOptions.ProxyCreationEnabled = false;
            return result;
        }

        [SetUp]
        public static void Bootstrap()
        {
			if (WhippedCreamDataLayer.Container == null)
			{
				Container = new UnityContainer();
				Container.RegisterInstance<EntityFrameworkDataLayer.IEntityFrameworkInitializerMap>(new EFInitializerMap());
				WhippedCreamDataLayer.Bootstrap(Container, new System.Uri(string.Format("http://localhost:{0}", GetAvailablePort())));
			}
			else
				Container = WhippedCreamDataLayer.Container;
        }

		private static short GetAvailablePort()
		{
			short port = 1000;
			IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

			while (port < short.MaxValue)
			{
				bool available = true;
				foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
				{
					if (tcpi.LocalEndPoint.Port == port)
					{
						available = false;
						break;
					}
				}
				if (available)
					break;

				port++;
			}

			if (port == short.MaxValue)
				throw new System.InvalidOperationException("Unable to find an unused port.");
			return port;
		}
    }
}

using System.Data.EntityClient;
using NUnit.Framework;
namespace WhippedCream.EntityFrameworkDataLayer
{
    [TestFixture]
    public class EntityFrameworkInitializerTests
    {
        [Test]
        public void Equals_Equatable_True()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };

            Assert.IsTrue(initializer.Equals(initializer));
        }

        [Test]
        public void Equals_Equatable_False_ConnectionString()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.ConnectionString = "DummyConnectionString";

            Assert.IsFalse(initializer.Equals(initializer2));
        }

        [Test]
        public void Equals_Equatable_False_DefaultContainerName()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.DefaultContainerName = "DummyDefaultContainerName";

            Assert.IsFalse(initializer.Equals(initializer2));
        }

        [Test]
        public void Equals_Equatable_False_SSDLResource()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.SSDLResource = "DummySSDLResource";

            Assert.IsFalse(initializer.Equals(initializer2));
        }

        [Test]
        public void Equals_Equatable_False_CSDLResource()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.CSDLResource = "DummyCSDLResource";

            Assert.IsFalse(initializer.Equals(initializer2));
        }

        [Test]
        public void Equals_Equatable_False_MSLResource()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.MSLResource = "DummyMSLResource";

            Assert.IsFalse(initializer.Equals(initializer2));
        }

        [Test]
        public void Equals_Equatable_False_Provider()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.Provider = "DummyProvider";

            Assert.IsFalse(initializer.Equals(initializer2));
        }
        
        [Test]
        public void Equals_True()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };

            Assert.IsTrue(initializer.Equals((object)initializer));
        }

        [Test]
        public void Equals_False_ConnectionString()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.ConnectionString = "DummyConnectionString";

            Assert.IsFalse(initializer.Equals((object)initializer2));
        }

        [Test]
        public void Equals_False_DefaultContainerName()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.DefaultContainerName = "DummyDefaultContainerName";

            Assert.IsFalse(initializer.Equals((object)initializer2));
        }

        [Test]
        public void Equals_False_SSDLResource()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.SSDLResource = "DummySSDLResource";

            Assert.IsFalse(initializer.Equals((object)initializer2));
        }

        [Test]
        public void Equals_False_CSDLResource()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.CSDLResource = "DummyCSDLResource";

            Assert.IsFalse(initializer.Equals((object)initializer2));
        }

        [Test]
        public void Equals_False_MSLResource()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.MSLResource = "DummyMSLResource";

            Assert.IsFalse(initializer.Equals((object)initializer2));
        }

        [Test]
        public void Equals_False_Provider()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };
            EntityFrameworkInitializer initializer2 = initializer;
            initializer2.Provider = "DummyProvider";

            Assert.IsFalse(initializer.Equals((object)initializer2));
        }

        [Test]
        public void GetHashCodeTest()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString",
                Provider = "TestProvider",
                DefaultContainerName = "TestDefaultContainerName",
                SSDLResource = "TestSSDLResource",
                CSDLResource = "CSDLResource",
                MSLResource = "MSLResource"
            };

            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * initializer.ConnectionString.GetHashCode();
                hashCode += 1000000009 * initializer.DefaultContainerName.GetHashCode();
                hashCode += 1000000021 * initializer.SSDLResource.GetHashCode();
                hashCode += 1000000033 * initializer.CSDLResource.GetHashCode();
                hashCode += 1000000037 * initializer.MSLResource.GetHashCode();
                hashCode += 1000000041 * initializer.Provider.GetHashCode();
            }

            Assert.AreEqual(hashCode, initializer.GetHashCode());
        }

        [Test]
        public void GetHashCodeTestAgain()
        {
            EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
            {
                ConnectionString = "TestConnectionString1",
                Provider = "TestProvider1",
                DefaultContainerName = "TestDefaultContainerName1",
                SSDLResource = null,
                CSDLResource = "CSDLResource1",
                MSLResource = "MSLResource1"
            };

            int hashCode = 0;
            unchecked
            {
                hashCode += 1000000007 * initializer.ConnectionString.GetHashCode();
                hashCode += 1000000009 * initializer.DefaultContainerName.GetHashCode();
                hashCode += 1000000021 * string.Empty.GetHashCode();
                hashCode += 1000000033 * initializer.CSDLResource.GetHashCode();
                hashCode += 1000000037 * initializer.MSLResource.GetHashCode();
                hashCode += 1000000041 * initializer.Provider.GetHashCode();
            }

            Assert.AreEqual(hashCode, initializer.GetHashCode());
        }

		[Test]
		public void GenerateConnectionStringTest()
		{
			EntityFrameworkInitializer initializer = new EntityFrameworkInitializer()
			{
				ConnectionString = "TestConnectionString",
				Provider = "TestProvider",
				DefaultContainerName = "TestDefaultContainerName",
				SSDLResource = "TestSSDLResource",
				CSDLResource = "CSDLResource",
				MSLResource = "MSLResource"
			};

			EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder();
			builder.Provider = initializer.Provider;
			builder.ProviderConnectionString = initializer.ConnectionString;
			builder.Metadata = string.Join("|", initializer.CSDLResource,
												initializer.MSLResource,
												initializer.SSDLResource);

			Assert.AreEqual(builder.ConnectionString, initializer.GenerateConnectionString(), @"
The generated connection string is not correct.
");
		}
    }
}
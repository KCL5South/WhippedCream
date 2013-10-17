using NUnit.Framework;
using AutoPoco.Engine;
using System.Linq;

namespace WhippedCream.Data.User
{
	[TestFixture]
	public class ModelTests
	{
		public class ModelTestCaseSource<T>
		{
			public System.Collections.IEnumerable GetTestCases()
			{
				IGenerationSessionFactory factory1 = Bootstrapper.GenerateFactory();
				IGenerationSessionFactory factory2 = Bootstrapper.GenerateFactory();

				IGenerationSession session1 = factory1.CreateSession();
				IGenerationSession session2 = factory2.CreateSession();

				T[] set1 = session1.Collection<T>(10).ToArray();
				T[] set2 = session2.Collection<T>(10).ToArray();

				for(int i = 0; i < 10; i++)
				{
					yield return new TestCaseData(set1[i], set2[i]);
				}
			}
		}

		[Test]
		[TestCaseSource(typeof(ModelTestCaseSource<Address>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<AgentAgencyAssociation>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<AgentId>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Awards>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Badges>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<ContactNumber>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<EMailAddress>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Files>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<FilesFileData>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<FilesWizardInfo>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<LoginEntry>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<MultiplyByTwoResult>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Name>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Preference>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<User>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Website>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<ZipCode>), "GetTestCases")]
		public void ObjectEqualsTest<T>(T first, T second)
		{
			Assert.IsTrue(first.Equals((object)second));	
		}

		[Test]
		[TestCaseSource(typeof(ModelTestCaseSource<Address>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<AgentAgencyAssociation>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<AgentId>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Awards>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Badges>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<ContactNumber>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<EMailAddress>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Files>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<FilesFileData>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<FilesWizardInfo>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<LoginEntry>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<MultiplyByTwoResult>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Name>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Preference>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<User>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Website>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<ZipCode>), "GetTestCases")]
		public void EquatableEqualsTest<T>(T first, T second)
		{
			Assert.IsTrue(first.Equals(second));
		}

		[Test]
		[TestCaseSource(typeof(ModelTestCaseSource<Address>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<AgentAgencyAssociation>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<AgentId>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Awards>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Badges>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<ContactNumber>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<EMailAddress>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Files>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<FilesFileData>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<FilesWizardInfo>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<LoginEntry>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<MultiplyByTwoResult>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Name>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Preference>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<User>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<Website>), "GetTestCases")]
		[TestCaseSource(typeof(ModelTestCaseSource<ZipCode>), "GetTestCases")]
		public void HashCodeTest<T>(T first, T second)
		{
			Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
		}
	}
}
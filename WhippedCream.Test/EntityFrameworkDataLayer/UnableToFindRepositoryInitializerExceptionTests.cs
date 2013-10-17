using NUnit.Framework;
namespace WhippedCream.EntityFrameworkDataLayer
{
    [TestFixture]
    public class UnableToFindRepositoryInitializerExceptionTests
    {
        [Test]
        public void MessageFormatedCorrectly()
        {
            UnableToFindRepositoryInitializerException ex =
                new UnableToFindRepositoryInitializerException(typeof(int));

            Assert.AreEqual(string.Format(UnableToFindRepositoryInitializerException.MessageFormat, typeof(int)),
                ex.Message);
        }
    }
}
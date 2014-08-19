using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    [TestFixture]
    public class DatabaseReaderTests : TestBase
    {
        /// <summary>
        /// Tests loading a database from the path provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="expectedNumberOfTargets"></param>
        [Test]
		[TestCase(@"testDatabase-100-3.mtdb", 100, Ignore = false)]
        // Updated due to user setting for NET cutoff being introduced
		[TestCase(@"Output.mtdb", 5730, Ignore = false)]
        public void TestLoadDatabase(string path, int expectedNumberOfTargets)
        {
            var reader          = new SqLiteTargetDatabaseReader();
            var database        = reader.ReadDb(GetTestSuiteDataPath(path));
            var numberOfTargets = database.ConsensusTargets.Count;
            Assert.AreEqual(expectedNumberOfTargets, numberOfTargets);
        }
    }
}

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
        [TestCase(@"testDatabase-100-3.mtdb", 100, Ignore = "false")]
        // Updated due to default setting for MSGF Filter being changed to QValue
        [TestCase(@"Output.mtdb", 8520, Ignore = "false")]
        public void TestLoadDatabase(string path, int expectedNumberOfTargets)
        {
            var reader          = new SqLiteTargetDatabaseReader();
            var database        = reader.ReadDb(GetTestSuiteDataPath(path));
            var numberOfTargets = database.ConsensusTargets.Count;
            Assert.AreEqual(expectedNumberOfTargets, numberOfTargets);
        }
    }
}

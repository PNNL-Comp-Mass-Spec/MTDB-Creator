using System.Diagnostics;
using MTDBAccessIO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    [TestFixture]
    public class AccessReaderTests : TestBase
    {
        [Test]
        [TestCase(@"Output.mdb", 8520)]
        public void TestReadReal(string path, int expectedNumberOfTargets)
        {
            if (IsRunningAsService())
            {
                Assert.Ignore("Cannot run Access tests from a non-interactive session");
            }

            var reader = new AccessTargetDatabaseReader();
            var dataset = reader.ReadDb(GetTestSuiteDataPath(path));
            var numberOfTargets = dataset.ConsensusTargets.Count;
            Assert.AreEqual(expectedNumberOfTargets, numberOfTargets);
        }

        public static bool IsRunningAsService()
        {
            return Process.GetCurrentProcess().SessionId == 0;
        }
    }
}


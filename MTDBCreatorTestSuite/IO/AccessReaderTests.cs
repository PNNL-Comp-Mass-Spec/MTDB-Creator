using System;
using MTDBAccessIO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    [TestFixture]
    public class AccessReaderTests : TestBase
    {
        [Test]
        [TestCase(@"Output.mdb", 8520, Ignore = false)]
        public void TestReadReal(string path, int expectedNumberOfTargets)
        {
            var reader = new AccessTargetDatabaseReader();
            var dataset = reader.ReadDb(GetTestSuiteDataPath(path));
            var numberOfTargets = dataset.ConsensusTargets.Count;
            Assert.AreEqual(expectedNumberOfTargets, numberOfTargets);
        }
    }
}


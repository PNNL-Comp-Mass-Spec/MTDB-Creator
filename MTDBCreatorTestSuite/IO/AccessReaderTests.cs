using System;
using MTDBAccessIO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    [TestFixture]
    public class AccessReaderTests : TestBase
    {
        private static bool m_ignore = false;
        [TestFixtureSetUp]
        public void IgnoreTests()
        {
            m_ignore = Environment.Is64BitProcess;
        }

        [Test]
        [TestCase(@"Output.mdb", 8520, Ignore = false)]
        public void TestReadReal(string path, int expectedNumberOfTargets)
        {
            if (!m_ignore)
            {
                var reader = new AccessTargetDatabaseReader();
                var dataset = reader.ReadDb(GetTestSuiteDataPath(path));
                var numberOfTargets = dataset.ConsensusTargets.Count;
                Assert.AreEqual(expectedNumberOfTargets, numberOfTargets);
            }
            else
            {
                Console.WriteLine("Test ignored; requires x86 processor for Access writing");
            }
        }
    }
}


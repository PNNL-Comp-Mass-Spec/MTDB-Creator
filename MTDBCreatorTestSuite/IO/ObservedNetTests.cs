using System.Diagnostics;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    namespace MTDBCreatorTestSuite.IO
    {
        [TestFixture]
        public class ObservedNetTests : TestBase
        {
            /// <summary>
            /// Tests loading a database from the path provided.
            /// </summary>
            /// <param name="path"></param>
            [Test]
            [TestCase(@"Scanstest\NoScanFile\QC_Shew_13_04_c_500ng_4Jun14_Samwise_14-02-17_msgfdb_syn.txt")]
            [TestCase(@"Scanstest\WithScanFile\QC_Shew_13_04_c_500ng_4Jun14_Samwise_14-02-17_msgfdb_syn.txt")]
            public void TestLoadScans(string path)
            {
                var filePath    = GetPath(path);
                var options     = new Options();
                var reader      = PhrpReaderFactory.Create(filePath, options);
                var data        = reader.Read(filePath);
                foreach (var e in data.Evidences)
                {
                    Debug.Assert(e.ObservedNet >= 0.0);
                    Debug.Assert(e.ObservedNet <= 1.0);
                }
            }
        }
    }
}

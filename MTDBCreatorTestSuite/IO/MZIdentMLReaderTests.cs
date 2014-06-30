using System.Diagnostics;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    /// <summary>
    /// These tests integrates reading using Phrp and creating an analysis in MTDBCreator
    /// </summary>
    [TestFixture]
    public class MZIdentMLReaderTests : TestBase
    {
        [Test]
        //[TestCase(@"MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfdb_syn.txt", 1819)] // Used to cross-verify the results
        //[TestCase(@"MSGFPlus\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfdb_syn.txt", 12414)]    // Used to cross-verify the results
        //[TestCase(@"MSGFPlus\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfdb_syn.txt", 12544)]    // Used to cross-verify the results
        [TestCase(@"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", 1819)]
        [TestCase(@"Mzml\QC_Shew_13_05-1_03_8Jun14_Samwise_14-02-16_msgfplus.mzid", 12414)]
        [TestCase(@"Mzml\QC_Shew_13_05-2_03_8Jun14_Samwise_14-02-17_msgfplus.mzid", 12544)]
        public void TestLoadingSingleFile(string jobPath,  int expectedEvidences)
        {
            PeptideCache.Clear();
            var options             = new Options();
            var jobDirectoryPath    = GetPath(jobPath);

            var pathName    = jobDirectoryPath;
            var reader      = PhrpReaderFactory.Create(pathName, options);
            var data        = reader.Read(pathName);
            Debug.Assert(data.Evidences.Count == expectedEvidences);
        }
    }
}
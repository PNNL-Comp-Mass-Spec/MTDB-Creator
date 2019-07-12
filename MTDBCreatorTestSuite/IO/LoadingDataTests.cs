using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
using MTDBFrameworkBase.Data;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.IO
{
    /// <summary>
    /// These tests integrates reading using Phrp and creating an analysis in MTDBCreator
    /// </summary>
    [TestFixture]
    public class LoadingDataTests : TestBase
    {
        [Test]
        [TestCase(@"Xtandem", "ManyXtandemList.txt", 3, 2147, 3580, 3433)]
        [TestCase(@"Sequest", "ManySequestList.txt", 3, 3733, 3315, 4114)]
        public void TestLoadingFiles(string jobDirectory, string jobList, int numJobs, params int[] expectedEvidences)
        {
            PeptideCache.Clear();
            var options             = new Options();
            var jobDirectoryPath    = GetPath(jobDirectory);
            var jobListPath         = GetPath(jobList);
            var num                 = 0;
            using (var sr = new StreamReader(jobListPath))
            {
                var pathName = sr.ReadLine();
                while (pathName != null && num < numJobs)
                {
                    pathName    = Path.Combine(jobDirectoryPath, pathName);
                    var reader  = PhrpReaderFactory.Create(pathName, options);
                    var data    = reader.Read(pathName);
                    Assert.AreEqual(expectedEvidences[num], data.Evidences.Count);

                    pathName = sr.ReadLine();
                    num++;
                }
            }
        }

        [Test]
        // Test case updated due to change in default selection of filter for MSGF+ to QValue
        [TestCase(@"MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus_syn.txt", 2189)]
        // Test case updated due to change in default selection of filter for MSGF+ to QValue
        [TestCase(@"Mzml\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfplus.mzid", 2189)]
        [TestCase(@"Xtandem\QC_Shew_12_02_pt5_2b_20Dec12_Leopard_12-11-10_xt.txt", 4927)]
        [TestCase(@"Sequest\QC_Shew_10_02a_2Nov10_Cougar_10-09-06_syn.txt", 3733)]
        public void TestLoadingSingleFile(string jobPath,  int expectedEvidences)
        {
            PeptideCache.Clear();
            var options             = new Options();
            var jobDirectoryPath    = GetPath(jobPath);

            var pathName    = jobDirectoryPath;
            var reader      = PhrpReaderFactory.Create(pathName, options);
            var data        = reader.Read(pathName);
            Assert.AreEqual(expectedEvidences, data.Evidences.Count);
        }
    }
}
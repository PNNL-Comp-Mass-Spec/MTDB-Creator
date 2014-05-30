using System.IO;
using MTDBFramework.Data;
using MTDBFramework.IO;
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
        [TestCase(@"MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfdb_syn.txt", null, 1)]
        [TestCase(@"Xtandem\QC_Shew_12_02_pt5_2b_20Dec12_Leopard_12-11-10_xt.txt", null, 1)]
        [TestCase(@"Sequest\QC_Shew_10_02a_2Nov10_Cougar_10-09-06_syn.txt", null, 1)]
        public void TestLoadingFiles(string jobDirectory, string jobList, int numJobs)
        {
            PeptideCache.Clear();
            var options             = new Options();
            var jobDirectoryPath    = GetPath(jobDirectory);
            if (jobList == null)
            {
                var pathName    = jobDirectoryPath;
                var reader      = PhrpReaderFactory.Create(pathName, options);
                reader.Read(pathName);
            }
            else
            {
                var jobListPath = GetPath(jobList);
                var num         = 0;
                using (var sr = new StreamReader(jobListPath))
                {
                    var pathName = sr.ReadLine();
                    while (pathName != null && num < numJobs)
                    {
                        pathName    = Path.Combine(jobDirectoryPath, pathName);
                        var reader  = PhrpReaderFactory.Create(pathName, options);
                        reader.Read(pathName);

                        pathName = sr.ReadLine();
                        num++;
                    }
                }
            }
        }
    }
}
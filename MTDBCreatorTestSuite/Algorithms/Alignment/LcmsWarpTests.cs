using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Algorithms.Alignment
{
    [TestFixture]
    public sealed class LcmsWarpTests
    {
        [TestCase(@"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_13_02_2a_03Mar14\QC_Shew_13_02_2a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt",
            @"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_13_02_2b_03Mar14\QC_Shew_13_02_2b_03Mar14_Leopard_14-02-02_msgfdb_syn.txt",
            @"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_13_02_3a_03Mar14\QC_Shew_13_02_3a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt",
            @"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_13_02_pt1_2_1_22Feb14\QC_Shew_13_02_pt1_2_1_22Feb14_Leopard_14-02-01_msgfdb_syn.txt",
            @"C:\UnitTestFolder\LCMSWarpTesting\QC_Shew_13_04_pt1_3_2_24Feb14\QC_Shew_13_04_pt1_3_2_24Feb14_Leopard_14-02-02_msgfdb_syn.txt"
            )]
        [TestCase(@"C:\UnitTestFolder\MSGFPlus\61928_SCU_WS_UPool_24_17Sep13_Cheetah_13-07-22_msgfdb_syn.txt")]
        public void TestLcmsWarpWrite(params string[] paths)
        {
            var datasets = new List<LcmsDataSet>();
            IEnumerable<AnalysisJobItem> jobs = new List<AnalysisJobItem>();     
            var options = new Options();
            var analysisProcessor = new AnalysisJobProcessor(options);
            var i = 0;
            var individualJobs = new List<AnalysisJobItem>();
            foreach (var path in paths)
            {
                var job = new AnalysisJobItem(path, LcmsIdentificationTool.MsgfPlus);
                individualJobs.Add(job);
            }
            jobs = individualJobs;

            jobs = analysisProcessor.Process(jobs);

            var databaseProcess = new MtdbProcessor(options);

            databaseProcess.Process(jobs.Select(job => job.DataSet).ToList());
        }
    }
}

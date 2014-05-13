using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.IO;
using NUnit.Framework;

namespace MTDBCreatorTestSuite.Algorithms.Alignment
{
    [TestFixture]
    public sealed class LcmsWarpTests : TestBase
    {
        [TestCase(@"QC_Shew_13_02_2a_03Mar14\QC_Shew_13_02_2a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt",
            @"QC_Shew_13_02_2b_03Mar14\QC_Shew_13_02_2b_03Mar14_Leopard_14-02-02_msgfdb_syn.txt",
            @"QC_Shew_13_02_3a_03Mar14\QC_Shew_13_02_3a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt",
            @"QC_Shew_13_02_pt1_2_1_22Feb14\QC_Shew_13_02_pt1_2_1_22Feb14_Leopard_14-02-01_msgfdb_syn.txt",
            @"QC_Shew_13_04_pt1_3_2_24Feb14\QC_Shew_13_04_pt1_3_2_24Feb14_Leopard_14-02-02_msgfdb_syn.txt"
            )]
        public void TestLcmsWarpWrite(params string[] paths)
        {
            var fullPaths        = new List<string>();
            var fullOutputPaths  = new List<string>();
            foreach(var path in paths)
            {
                fullPaths.Add(GetPath(path));
                fullOutputPaths.Add(GetOutputPath(path));
            }
            var options = new Options();
            var analysisProcessor = new AnalysisJobProcessor(options);
            var individualJobs = new List<AnalysisJobItem>();
            foreach (var path in fullPaths)
            {
                var job = new AnalysisJobItem(path, LcmsIdentificationTool.MsgfPlus);
                individualJobs.Add(job);
            }
            IEnumerable<AnalysisJobItem> jobs = individualJobs;

            jobs = analysisProcessor.Process(jobs);

            var databaseProcess = new MtdbProcessor(options);

            databaseProcess.Process(jobs.Select(job => job.DataSet).ToList());
        }
    }
}

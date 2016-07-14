using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using MTDBAccessIO;
using NUnit.Framework;
using PHRPReader;

namespace MTDBCreatorTestSuite.IO
{
    [TestFixture]
    public class AccessWriterTests : TestBase
    {
        [Test]
        [TestCase(@"testAccess-1-1-1.mdb", 1, 1, 1)]
        [TestCase(@"testAccess-1-3-1.mdb", 1, 3, 1)]
        [TestCase(@"testAccess-1-1-3.mdb", 1, 1, 3)]
        [TestCase(@"testAccess-3-1-1.mdb", 3, 1, 1)]
        [TestCase(@"testAccess-3-3-1.mdb", 3, 3, 1)]
        public void TestWriteAccessDb(string path, int numberConsensus, int numberProteins, int numberEvidences)
        {
            var writer = new AccessTargetDatabaseWriter();
            var options = new Options();
            var database = new TargetDatabase();
            var evidenceCount = 1;

            for (var i = 1; i <= numberConsensus; i++)
            {
                var target = new ConsensusTarget {Id = i};

                for (var k = 1; k <= numberProteins; k++)
                {
                    var protein = new ProteinInformation
                    {
                        ProteinName = "Access_Test" + k,
                        CleavageState = clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants.Full,
                        TerminusState =
                            clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.ProteinNTerminus
                    };
                    protein.Consensus.Add(target);

                    for (var j = 1; j <= numberEvidences; j++)
                    {
                        var evidence = new Evidence
                        {
                            AnalysisId = j,
                            Charge = 1,
                            Id = evidenceCount,
                            CleanPeptide = "MIKEDEGAN" + evidenceCount,
                            Sequence = "BIGBIRD" + evidenceCount,
                            Mz = 405,
                            Scan = evidenceCount++,
                            PeptideInfo = new TargetPeptideInfo()
                        };

                        evidence.AddProtein(protein);
                        target.AddEvidence(evidence);
                    }

                    target.AddProtein(protein);
                    target.CalculateStatistics();
                }
                target.TheoreticalMonoIsotopicMass = 100.0;
                target.AverageNet = .6;
                target.PredictedNet = .7;

                database.ConsensusTargets.Add(target);
            }

            if (File.Exists(GetTestSuiteDataPath(path)))
            {
                File.Delete(GetTestSuiteDataPath(path));
            }


            writer.Write(database, options, GetTestSuiteDataPath(path));
        }


        [Test]
        [TestCase(@"QC_Shew_13_02_2a_03Mar14\QC_Shew_13_02_2a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt",
            @"QC_Shew_13_02_2b_03Mar14\QC_Shew_13_02_2b_03Mar14_Leopard_14-02-02_msgfdb_syn.txt", Ignore = false
            )]
        public void TestWriteAccessReal(params string[] paths)
        {
            var fullPaths = new List<string>();
            var fullOutputPaths = new List<string>();
            foreach (var path in paths)
            {
                fullPaths.Add(GetPath(path));
                fullOutputPaths.Add(GetOutputPath(path));
            }
            var options = new Options();

            var analysisProcessor = new AnalysisJobProcessor(options);
            var individualJobs = fullPaths.Select(path => new AnalysisJobItem(path, LcmsIdentificationTool.MsgfPlus)).ToList();
            var bWorker = new BackgroundWorker();

            var jobs = analysisProcessor.Process(individualJobs, bWorker);

            var databaseProcess = new MtdbProcessor(options);

            var database = databaseProcess.Process(jobs.Select(job => job.DataSet).ToList(), bWorker);

            var writer = new AccessTargetDatabaseWriter();

            writer.Write(database, options, GetTestSuiteDataPath(@"Output.mdb"));
        }
    }
}

using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using NUnit.Framework;
using PHRPReader;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PNNLOmics.Algorithms.Regression;


namespace MTDBCreatorTestSuite.IO
{
    [TestFixture]
    public class DatabaseWriterTests : TestBase
    {
        /// <summary>
        /// Tests loading a database from the path provided.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="numberOfTargets">Number of targets to put in the database</param>
        /// <param name="numberOfEvidences">Number of evidences to put in each target.</param>
        /// All ignored right now until I get mtdbs locally
        [Test]
        [TestCase(@"..\..\..\TestData\testDatabase-1-1.mtdb", 1, 1, Ignore = false)]
        [TestCase(@"..\..\..\TestData\testDatabase-1-2.mtdb", 1, 2, Ignore = false)]
        [TestCase(@"..\..\..\TestData\testDatabase-2-1.mtdb", 2, 1, Ignore = false)]
        [TestCase(@"..\..\..\TestData\testDatabase-100-3.mtdb", 100, 3, Ignore = false)]
        public void TestWriteDatabase(string path, int numberOfTargets, int numberOfEvidences)
        {
            var reader       = new SqLiteTargetDatabaseWriter();
            var options      = new Options();            
            var database     = new TargetDatabase();
            var proteinCount  = 1;
            var evidenceCount = 1;

            for (var i = 1; i <= numberOfTargets; i++)
            {
                var target = new ConsensusTarget {Id = i};

                var protein = new ProteinInformation
                {
                    ProteinName = "SO_Test" + proteinCount++, 
                    CleavageState = clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants.Full,
                    TerminusState = clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants.ProteinNTerminus,
                };
                protein.Consensus.Add(target);

                for (var j = 1; j <= numberOfEvidences; j++)
                {
                    var evidence        = new Evidence
                    {
                        AnalysisId      = j,
                        Charge          = 1,
                        Id              = evidenceCount,
                        CleanPeptide    = "MIKEDEGAN" + evidenceCount,
                        Sequence        = "BIGBIRD" + evidenceCount,
                        Mz              = 405,
                        Scan            = evidenceCount++,
                        PeptideInfo =  new TargetPeptideInfo()
                        
                    };

                    evidence.AddProtein(protein);
                    target.AddEvidence(evidence);
                }
                target.AddProtein(protein);
                target.CalculateStatistics();
                database.ConsensusTargets.Add(target);
            }
            reader.Write(database, options, path);            
        }

        [Test]
        [TestCase(@"QC_Shew_13_02_2a_03Mar14\QC_Shew_13_02_2a_03Mar14_Leopard_14-02-01_msgfdb_syn.txt",
            @"QC_Shew_13_02_2b_03Mar14\QC_Shew_13_02_2b_03Mar14_Leopard_14-02-02_msgfdb_syn.txt", Ignore = false
            )]
        public void TestWriteReal(params string[] paths)
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

            SqLiteTargetDatabaseWriter writer = new SqLiteTargetDatabaseWriter();

            writer.Write(database, options, @".\Output.mtdb");

        }
    }
}
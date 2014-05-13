using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using NUnit.Framework;
using PHRPReader;

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
        [TestCase(@"m:\testDatabase-1-1.mtdb", 1, 1, Ignore = true)]
        [TestCase(@"m:\testDatabase-1-2.mtdb", 1, 2, Ignore = true)]
        [TestCase(@"m:\testDatabase-2-1.mtdb", 2, 1, Ignore = true)]
        [TestCase(@"m:\testDatabase-100-3.mtdb", 100, 3, Ignore = true)]
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
    }
}
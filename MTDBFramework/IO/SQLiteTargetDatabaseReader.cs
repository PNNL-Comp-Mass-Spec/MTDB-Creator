#region Namespaces
using System.Collections.Generic;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;
#endregion

namespace MTDBFramework.IO
{
    /// <summary>
    /// Creates a target database from a SQLite formatted target database.
    /// </summary>
    public sealed class SqLiteTargetDatabaseReader : ITargetDatabaseReader
    {
        private readonly TargetDatabase m_targetDb = new TargetDatabase();
        private readonly Dictionary<string, LcmsDataSet> m_lcmsDataDic = new Dictionary<string, LcmsDataSet>();
        private string m_lastReadFile;

        /// <summary>
        /// Reads a target database from the path provided.
        /// </summary>
        /// <param name="path">Path to SQLite database file.</param>
        /// <returns>Target Database</returns>
        public TargetDatabase ReadDb(string path)
        {
            ReadSqLite(path);
            return m_targetDb;
        }

        /// <summary>
        /// Reads a LCMS Dataset from the path provided.
        /// </summary>
        /// <param name="path">Path to SQLite database file.</param>
        /// <returns>Target Database</returns>
        public IEnumerable<LcmsDataSet> Read(string path)
        {
            ReadSqLite(path);
            var datasets = new List<LcmsDataSet>();

            foreach (var member in m_lcmsDataDic)
            {
                datasets.Add(member.Value);
            }

            return datasets;
        }

        private void ReadSqLite(string path)
        {
            // Don't read again if we just read the file
            if (path == m_lastReadFile)
            {
                return;
            }
            // Reset the data
            m_targetDb.ClearTargets();
            m_lcmsDataDic.Clear();

            //var sessionFactory = DatabaseReaderFactory.CreateSessionFactory(path);
            DatabaseFactory.DatabaseFile = path;
            DatabaseFactory.ReadOrAppend = true;
            var sessionFactory = DatabaseFactory.CreateSessionFactory(DatabaseType.SQLite);

            var readConsensus = new List<ConsensusTarget>();
            var readPair = new List<ConsensusProteinPair>();
            var readProtein = new List<ProteinInformation>();
            var readEvidence = new List<Evidence>();
            var readPtms = new List<PostTranslationalModification>();
            var readPtmPairs = new List<ConsensusPtmPair>();
            var readOptions = new List<Options>();

            var consensusDic = new Dictionary<int, ConsensusTarget>();
            var consensusProteinDic = new Dictionary<int, List<ConsensusProteinPair>>();
            var consensusPtmDic = new Dictionary<int, List<ConsensusPtmPair>>();
            var proteinDic = new Dictionary<int, ProteinInformation>();
            var ptmDic = new Dictionary<int, PostTranslationalModification>();

            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ProteinInformation>().List(readProtein);
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    session.CreateCriteria<PostTranslationalModification>().List(readPtms);
                    session.CreateCriteria<Options>().List(readOptions);
                    session.CreateCriteria<ConsensusProteinPair>().List(readPair);
                    session.CreateCriteria<ConsensusPtmPair>().List(readPtmPairs);
                    session.CreateCriteria<Evidence>().List(readEvidence);
                    transact.Commit();
                }

                /*
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ConsensusProteinPair>().List(readPair);
                    session.CreateCriteria<ConsensusPtmPair>().List(readPtmPairs);
                    session.CreateCriteria<Evidence>().List(readEvidence);
                    transact.Commit();
                }
                */

                foreach (var consensus in readConsensus)
                {
                    consensus.Ptms.Clear();
                    //consensus.Evidences.Clear();
                    consensus.Evidences = new List<Evidence>();
                    consensus.Sequence = consensus.CleanSequence;
                    m_targetDb.AddConsensusTarget(consensus);
                    consensusDic.Add(consensus.Id, consensus);
                }

                foreach (var pair in readPair)
                {
                    if (!consensusProteinDic.ContainsKey(pair.Consensus.Id))
                    {
                        consensusProteinDic.Add(pair.Consensus.Id, new List<ConsensusProteinPair>());
                    }
                    consensusProteinDic[pair.Consensus.Id].Add(pair);
                }

                foreach (var pair in readPtmPairs)
                {
                    if (!consensusPtmDic.ContainsKey(pair.Target.Id))
                    {
                        consensusPtmDic.Add(pair.Target.Id, new List<ConsensusPtmPair>());
                    }
                    consensusPtmDic[pair.Target.Id].Add(pair);
                }

                foreach (var protein in readProtein)
                {
                    proteinDic.Add(protein.Id, protein);
                }

                foreach (var ptm in readPtms)
                {
                    ptmDic.Add(ptm.Id, ptm);
                }

                foreach (var consensus in consensusPtmDic)
                {
                    foreach (var pair in consensus.Value)
                    {
                        var ptm = new PostTranslationalModification
                        {
                            Mass = ptmDic[pair.PostTranslationalModification.Id].Mass,
                            Name = ptmDic[pair.PostTranslationalModification.Id].Name,
                            Formula = ptmDic[pair.PostTranslationalModification.Id].Formula,
                            Location = pair.Location,
                            Parent = consensusDic[pair.Target.Id]
                        };

                        consensusDic[pair.Target.Id].Ptms.Add(ptm);
                    }
                }

                foreach (var evidence in readEvidence)
                {
                    foreach(var pair in consensusProteinDic[evidence.Parent.Id])
                    {
                        var protein = proteinDic[pair.Protein.Id];
                        protein.ResidueEnd = pair.ResidueEnd;
                        protein.ResidueStart = pair.ResidueStart;
                        protein.TerminusState = (clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants)pair.TerminusState;
                        protein.CleavageState = (clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants)pair.CleavageState;
                        //protein.Id = 0;
                        evidence.AddProtein(protein);
                        consensusDic[evidence.Parent.Id].AddProtein(protein);
                    }
                    evidence.MonoisotopicMass = consensusDic[evidence.Parent.Id].TheoreticalMonoIsotopicMass;
                    evidence.Ptms = consensusDic[evidence.Parent.Id].Ptms;

                    if (!m_lcmsDataDic.ContainsKey(evidence.DataSet.Name))
                    {
                        var dataset = new LcmsDataSet(true);
                        m_lcmsDataDic.Add(evidence.DataSet.Name, dataset);
                        m_lcmsDataDic[evidence.DataSet.Name].Name = evidence.DataSet.Name;
                        m_lcmsDataDic[evidence.DataSet.Name].Tool = evidence.DataSet.Tool;
                    }
                    m_lcmsDataDic[evidence.DataSet.Name].Evidences.Add(evidence);
                    consensusDic[evidence.Parent.Id].AddEvidence(evidence);
                }
            }
            // Set the member variable to avoid double reads.
            m_lastReadFile = path;
        }
    }
}

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
        /// <summary>
        /// Reads a target database from the path provided.
        /// </summary>
        /// <param name="path">Path to SQLite database file.</param>
        /// <returns>Target Database</returns>
        public TargetDatabase ReadDB(string path)
        {
			//var sessionFactory = DatabaseReaderFactory.CreateSessionFactory(path);
			DatabaseFactory.DatabaseFile = path;
            DatabaseFactory.ReadOrAppend = true;
			var sessionFactory = DatabaseFactory.CreateSessionFactory(MTDBFramework.Data.DatabaseType.SQLite);
            var database = new TargetDatabase();

            var readConsensus = new List<ConsensusTarget>();
            var readPair = new List<ConsensusProteinPair>();
            var readProt = new List<ProteinInformation>();
            var readEvidence = new List<Evidence>();
            var readPtms = new List<PostTranslationalModification>();

            var datasetDic = new Dictionary<string, LcmsDataSet>();
            var consensusDic = new Dictionary<int, ConsensusTarget>();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ProteinInformation>().List(readProt);
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    transact.Commit();
                }
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ConsensusProteinPair>().List(readPair);
                    transact.Commit();
                }

                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<PostTranslationalModification>().List(readPtms);
                    transact.Commit();
                }

                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<Evidence>().List(readEvidence);
                    transact.Commit();
                }

                foreach (var consensus in readConsensus)
                {
                    consensus.PTMs.Clear();
                    database.AddConsensusTarget(consensus);
                    consensusDic.Add(consensus.Id, consensus);
                }

                foreach (var ptm in readPtms)
                {
                    consensusDic[ptm.Parent.Id].PTMs.Add(ptm);
                }

                foreach (var evidence in readEvidence)
                {
                    if (!datasetDic.ContainsKey(evidence.DataSet.Name))
                    {
                        datasetDic.Add(evidence.DataSet.Name, new LcmsDataSet());
                        datasetDic[evidence.DataSet.Name].Name = evidence.DataSet.Name;
                        datasetDic[evidence.DataSet.Name].Tool = evidence.DataSet.Tool;
                    }
                    datasetDic[evidence.DataSet.Name].Evidences.Add(evidence);
                    consensusDic[evidence.Parent.Id].AddEvidence(evidence);
                    evidence.PTMs = consensusDic[evidence.Parent.Id].PTMs;
                }

                
            }

            var datasets = new List<LcmsDataSet>();

            foreach (var member in datasetDic)
            {
                datasets.Add(member.Value);
            }

            return database;
        }

		/// <summary>
		/// Reads a LCMS Dataset from the path provided.
		/// </summary>
		/// <param name="path">Path to SQLite database file.</param>
		/// <returns>Target Database</returns>
        public IEnumerable<LcmsDataSet> Read(string path)
        {
			//var sessionFactory = DatabaseReaderFactory.CreateSessionFactory(path);
            DatabaseFactory.DatabaseFile = path;
            DatabaseFactory.ReadOrAppend = true;
			var sessionFactory = DatabaseFactory.CreateSessionFactory(MTDBFramework.Data.DatabaseType.SQLite);
            var database = new TargetDatabase();

            var readConsensus = new List<ConsensusTarget>();
            var readPair = new List<ConsensusProteinPair>();
            var readProt = new List<ProteinInformation>();
            var readEvidence = new List<Evidence>();
            var readPtms = new List<PostTranslationalModification>();

            var datasetDic = new Dictionary<string, LcmsDataSet>();
            var consensusDic = new Dictionary<int, ConsensusTarget>();
            var consensusProtDic = new Dictionary<int, List<ConsensusProteinPair>>();
            var protDic = new Dictionary<int, ProteinInformation>();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ProteinInformation>().List(readProt);
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    transact.Commit();
                }
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ConsensusProteinPair>().List(readPair);
                    transact.Commit();
                }

                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<PostTranslationalModification>().List(readPtms);
                    transact.Commit();
                }

                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<Evidence>().List(readEvidence);
                    transact.Commit();
                }

                foreach (var consensus in readConsensus)
                {
                    consensus.PTMs.Clear();
                    consensus.Evidences.Clear();
                    consensus.Sequence = consensus.CleanSequence;
                    database.AddConsensusTarget(consensus);
                    consensusDic.Add(consensus.Id, consensus);
                }

                foreach (var pair in readPair)
                {
                    if(!consensusProtDic.ContainsKey(pair.ConsensusId))
                    {
                        consensusProtDic.Add(pair.ConsensusId, new List<ConsensusProteinPair>());
                    }
                    consensusProtDic[pair.ConsensusId].Add(pair);
                }

                foreach (var prot in readProt)
                {
                    protDic.Add(prot.Id, prot);
                }

                foreach (var ptm in readPtms)
                {
                    consensusDic[ptm.Parent.Id].PTMs.Add(ptm);
                }

                foreach (var evidence in readEvidence)
                {
                    foreach(var pair in consensusProtDic[evidence.Parent.Id])
                    {
                        var prot = protDic[pair.ProteinId];
                        prot.ResidueEnd = pair.ResidueEnd;
                        prot.ResidueStart = pair.ResidueStart;
                        prot.TerminusState = (clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants)pair.TerminusState;
                        prot.CleavageState = (clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants)prot.CleavageState;
                        prot.Id = 0;
                        evidence.AddProtein(prot);
                    }
                    evidence.MonoisotopicMass = consensusDic[evidence.Parent.Id].TheoreticalMonoIsotopicMass;
                    evidence.PTMs = consensusDic[evidence.Parent.Id].PTMs;

                    if (!datasetDic.ContainsKey(evidence.DataSet.Name))
                    {
                        var dataset = new LcmsDataSet(true);
                        datasetDic.Add(evidence.DataSet.Name, dataset);
                        datasetDic[evidence.DataSet.Name].Name = evidence.DataSet.Name;
                        datasetDic[evidence.DataSet.Name].Tool = evidence.DataSet.Tool;
                    }
                    datasetDic[evidence.DataSet.Name].Evidences.Add(evidence);
                    consensusDic[evidence.Parent.Id].AddEvidence(evidence);
                }
            }

            var datasets = new List<LcmsDataSet>();

            foreach (var member in datasetDic)
            {
                datasets.Add(member.Value);
            }

            return datasets;
        }
    }
}

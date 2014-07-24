﻿#region Namespaces

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
		private readonly TargetDatabase _TargetDB = new TargetDatabase();
		private readonly Dictionary<string, LcmsDataSet> _LcmsDataDic = new Dictionary<string, LcmsDataSet>();
		
		/// <summary>
		/// Reads a target database from the path provided.
		/// </summary>
		/// <param name="path">Path to SQLite database file.</param>
		/// <returns>Target Database</returns>
		public TargetDatabase ReadDB(string path)
		{
			ReadSQLite(path);
			return _TargetDB;
		}

		/// <summary>
		/// Reads a LCMS Dataset from the path provided.
		/// </summary>
		/// <param name="path">Path to SQLite database file.</param>
		/// <returns>Target Database</returns>
		public IEnumerable<LcmsDataSet> Read(string path)
		{
			ReadSQLite(path);

			var datasets = new List<LcmsDataSet>();

			foreach (var member in _LcmsDataDic)
			{
				datasets.Add(member.Value);
			}

			return datasets;
		}

		private void ReadSQLite(string path)
		{
			//var sessionFactory = DatabaseReaderFactory.CreateSessionFactory(path);
			DatabaseFactory.DatabaseFile = path;
            DatabaseFactory.ReadOrAppend = true;
			var sessionFactory = DatabaseFactory.CreateSessionFactory(MTDBFramework.Data.DatabaseType.SQLite);
            
            var readConsensus = new List<ConsensusTarget>();
            var readPair = new List<ConsensusProteinPair>();
            var readProt = new List<ProteinInformation>();
            var readEvidence = new List<Evidence>();
            var readPtms = new List<PostTranslationalModification>();
            var readPtmPairs = new List<ConsensusPtmPair>();
			var readOptions = new List<Options>();

            var consensusDic = new Dictionary<int, ConsensusTarget>();
            var consensusProtDic = new Dictionary<int, List<ConsensusProteinPair>>();
            var consensusPtmDic = new Dictionary<int, List<ConsensusPtmPair>>();
            var protDic = new Dictionary<int, ProteinInformation>();
            var ptmDic = new Dictionary<int, PostTranslationalModification>();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ProteinInformation>().List(readProt);
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    session.CreateCriteria<PostTranslationalModification>().List(readPtms);
	                session.CreateCriteria<Options>().List(readOptions);
                    transact.Commit();
                }

                using (var transact = session.BeginTransaction())
                {
					session.CreateCriteria<ConsensusProteinPair>().List(readPair);
					session.CreateCriteria<ConsensusPtmPair>().List(readPtmPairs);
                    session.CreateCriteria<Evidence>().List(readEvidence);
                    transact.Commit();
                }

                foreach (var consensus in readConsensus)
                {
                    consensus.PTMs.Clear();
                    consensus.Evidences.Clear();
                    consensus.Sequence = consensus.CleanSequence;
                    _TargetDB.AddConsensusTarget(consensus);
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
                
                foreach (var pair in readPtmPairs)
                {
                    if (!consensusPtmDic.ContainsKey(pair.ConsensusId))
                    {
                        consensusPtmDic.Add(pair.ConsensusId, new List<ConsensusPtmPair>());
                    }
                    consensusPtmDic[pair.ConsensusId].Add(pair);
                }

                foreach (var prot in readProt)
                {
                    protDic.Add(prot.Id, prot);
                }

                foreach (var ptm in readPtms)
                {
                    ptmDic.Add(ptm.Id, ptm);
                }

                foreach (var consensus in consensusPtmDic)
                {
                    foreach (var pair in consensus.Value)
                    {
                        var ptm = new PostTranslationalModification();

                        ptm.Mass = ptmDic[pair.PtmId].Mass;
                        ptm.Name = ptmDic[pair.PtmId].Name;
                        ptm.Formula = ptmDic[pair.PtmId].Formula;
                        ptm.Location = pair.Location;
                        ptm.Parent = consensusDic[pair.ConsensusId];
                        consensusDic[pair.ConsensusId].PTMs.Add(ptm);
                    }
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

                    if (!_LcmsDataDic.ContainsKey(evidence.DataSet.Name))
                    {
                        var dataset = new LcmsDataSet(true);
                        _LcmsDataDic.Add(evidence.DataSet.Name, dataset);
                        _LcmsDataDic[evidence.DataSet.Name].Name = evidence.DataSet.Name;
                        _LcmsDataDic[evidence.DataSet.Name].Tool = evidence.DataSet.Tool;
                    }
                    _LcmsDataDic[evidence.DataSet.Name].Evidences.Add(evidence);
                    consensusDic[evidence.Parent.Id].AddEvidence(evidence);
                }
            }
		}
    }
}

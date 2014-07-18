#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using MTDBFramework.Database;

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
        public TargetDatabase Read(string path)
        {            
            var sessionFactory = DatabaseReaderFactory.CreateSessionFactory(path);
            var database = new TargetDatabase();
             
            var readConsensus = new List<ConsensusTarget>();
            var readPair = new List<ConsensusProteinPair>();
            var readProt = new List<ProteinInformation>();
            var readEvidence = new List<Evidence>();

            using(var session = sessionFactory.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ProteinInformation>().List(readProt);
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    transact.Commit();
                }
                using(var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ConsensusProteinPair>().List(readPair);
                    transact.Commit();
                }
                
                using (var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<Evidence>().List(readEvidence);
                    transact.Commit();
                }
            }

            foreach (var evidence in readConsensus)
            {
                database.AddConsensusTarget(evidence);
            }

            var datasets = new List<LcmsDataSet>();

            var datasetDic = new Dictionary<string, LcmsDataSet>();

            foreach (var evidence in readEvidence)
            {
                if(!datasetDic.ContainsKey(evidence.DataSet.Name))
                {
                    datasetDic.Add(evidence.DataSet.Name, new LcmsDataSet());
                    datasetDic[evidence.DataSet.Name].Name = evidence.DataSet.Name;
                    datasetDic[evidence.DataSet.Name].Tool = evidence.DataSet.Tool;
                }
                datasetDic[evidence.DataSet.Name].Evidences.Add(evidence);
            }

            foreach(var member in datasetDic)
            {
                datasets.Add(member.Value);
            }

            return database;
        }
    }
}

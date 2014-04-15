#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    public class SQLiteTargetDatabaseReader : ITargetDatabaseReader
    {
        public TargetDatabase Read(string path)
        {
            DatabaseCreatorFactory.DbFile = path;
            var sessionFactory = DatabaseCreatorFactory.CreateSessionFactory();
            TargetDatabase reader = new TargetDatabase();

            List<ConsensusTarget> readConsensus = new List<ConsensusTarget>();
            List<Evidence>          readTarget    = new List<Evidence>();
            using(var session = sessionFactory.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                }
            }
            foreach(ConsensusTarget evidence in readConsensus)
            {
                reader.ConsensusTargets.Add(evidence);
            }

            return reader;

            //reader.ConsensusTargets = readConsensus;
            //return null;
        }
    }
}

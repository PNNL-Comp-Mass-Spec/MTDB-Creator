#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    public class SqLiteTargetDatabaseReader : ITargetDatabaseReader
    {
        public TargetDatabase Read(string path)
        {
            DatabaseCreatorFactory.DbFile = path;
            var sessionFactory = DatabaseCreatorFactory.CreateSessionFactory();
            var reader = new TargetDatabase();

            var readConsensus = new List<ConsensusTarget>();
            var          readTarget    = new List<Evidence>();
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
        }
    }
}

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
            //DatabaseCreatorFactory.DbFile = path;
            //var sessionFactory = DatabaseCreatorFactory.CreateSessionFactory();
            //TargetDatabase reader = new TargetDatabase();

            //List<ConsensusTarget> readConsensus = new List<ConsensusTarget>();
            //List<Target>          readTarget    = new List<Target>();
            //using(var session = sessionFactory.OpenSession())
            //{
            //    using(var transact = session.BeginTransaction())
            //    {
            //        session.CreateCriteria<ConsensusTarget>().List(readConsensus);
            //    }
            //}
            //reader.ConsensusTargets = readConsensus;
            return null;
        }
    }
}

#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using MTDBFramework.Database;

#endregion

namespace MTDBFramework.IO
{
    public class SqLiteTargetDatabaseReader : ITargetDatabaseReader
    {
        public TargetDatabase Read(string path)
        {            
            var sessionFactory = DatabaseReaderFactory.CreateSessionFactory(path);
            var reader = new TargetDatabase();

            var readConsensus = new List<ConsensusTarget>();
            using(var session = sessionFactory.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {                    
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    transact.Commit();
                }
            }
            foreach(var evidence in readConsensus)
            {
                reader.ConsensusTargets.Add(evidence);
            }

            return reader;
        }
    }
}

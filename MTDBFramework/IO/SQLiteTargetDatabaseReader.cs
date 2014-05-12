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
            using(var session = sessionFactory.OpenSession())
            {
                using(var transact = session.BeginTransaction())
                {                    
                    session.CreateCriteria<ConsensusTarget>().List(readConsensus);
                    transact.Commit();
                }

                foreach (var evidence in readConsensus)
                {
                    database.AddConsensusTarget(evidence);
                }
            }


            return database;
        }
    }
}

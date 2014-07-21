using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace MTDBFramework.Database
{
    public class DatabaseReaderFactory
    {        
        public static ISessionFactory CreateSessionFactory(string path)
        {
            //TODO: Add a switch to create different configurations for alternative database types.
            //Currently this database is only configured for the ConsensusTarget class. The others
            // will be added after more conclusive tests.
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(path))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusTargetMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PostTranslationalModificationMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ProteinInformationMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusProteinPairMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EvidenceMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OptionsMap>())
                .BuildSessionFactory();
        }
    }
}
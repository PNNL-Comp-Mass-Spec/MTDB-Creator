#region Namespaces

using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    public class DatabaseCreatorFactory
    {

        public static string DatabaseFile = "SQLiteTest.mtdb"; //This is a default path

        public static ISessionFactory CreateSessionFactory(DatabaseType type)
        {
            //TODO: Add a switch to create different configurations for alternative database types.
            //Currently this database is only configured for the ConsensusTarget class. The others
            // will be added after more conclusive tests.
            switch (type)
            {
                case DatabaseType.SQLite:
                    {
                        return Fluently.Configure()
                            .Database(SQLiteConfiguration.Standard
                                .UsingFile(DatabaseFile))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusTargetMap>())
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ProteinInformationMap>())
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusProteinPairMap>())
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EvidenceMap>())
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OptionsMap>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
                    }
                default:
                    return null;
            }
        }
        private static void BuildSchema(Configuration config)
        {

            // Recreating the schema in between OpenSessions is !BAD! it will clear all
            // of your entries in the database. This configuration will only create the
            // schema if the file does NOT exist already.
            if (File.Exists(DatabaseFile))
            {
                File.Delete(DatabaseFile);
            }
            new SchemaExport(config).Create(false, true);
        }
    }
}
#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using PNNLOmics.Data;

#endregion

namespace MTDBFramework.Database
{
    public class DatabaseCreatorFactory
    {

        public static string DbFile = "SQLiteTest.db"; //This is a default path
        private ISessionFactory sessionFactory = CreateSessionFactory();

        public static ISessionFactory CreateSessionFactory()
        {
            //TODO: Add a switch to create different configurations for alternative database types.
            //Currently this database is only configured for the ConsensusTarget class. The others
            // will be added after more conclusive tests.
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(DbFile))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusTargetMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TargetMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TargetPeptideInfoMap>())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<OptionsMap>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {

            // Recreating the schema in between OpenSessions is !BAD! it will clear all
            // of your entries in the database. This configuration will only create the
            // schema if the file does NOT exist already.
            if (File.Exists(DbFile))
            {
                File.Delete(DbFile);
            }
            new SchemaExport(config).Create(false, true);
        }

    }
}
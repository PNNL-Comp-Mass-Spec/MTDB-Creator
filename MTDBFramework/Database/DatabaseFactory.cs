#region Namespaces

using System;
using System.IO;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
	/// <summary>
	/// Configuration of NHibernate for working with a SQLite database
	/// </summary>
    public class DatabaseFactory
    {
		/// <summary>
		/// The file to use for the database
		/// </summary>
        public static string DatabaseFile = "SQLiteTest.mtdb"; //This is a default path
		/// <summary>
		/// Whether to read from/append to an existing file, or overwrite it
		/// </summary>
        public static bool ReadOrAppend = true;

		/// <summary>
		/// Configure and return a session factory for NHibernate
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
        public static ISessionFactory CreateSessionFactory(DatabaseType type)
        {
            // TODO: Add a switch to create different configurations for alternative database types.
            // Currently this database is only configured for the ConsensusTarget class. The others
            // will be added after more conclusive tests.
            switch (type)
            {
                case DatabaseType.SQLite:
                    {
                        return Fluently.Configure()
                            .Database(SQLiteConfiguration.Standard
                                .UsingFile(DatabaseFile)
                                .AdoNetBatchSize(2500))
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<OptionsMap>())
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<TargetDatasetMap>())
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusTargetMap>())
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<PostTranslationalModificationMap>())
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ProteinInformationMap>())
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusProteinPairMap>()
								.Conventions.AddFromAssemblyOf<CustomForeignKeyConvention>())
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ConsensusPtmPairMap>()
								.Conventions.AddFromAssemblyOf<CustomForeignKeyConvention>())
							.Mappings(m => m.FluentMappings.AddFromAssemblyOf<EvidenceMap>()
								.Conventions.AddFromAssemblyOf<CustomForeignKeyConvention>())
                            .ExposeConfiguration(BuildSchema)
                            .BuildSessionFactory();
                    }
                default:
                    return null;
            }
        }

        private static void BuildSchema(Configuration config)
        {
			// Allow use for both reading and writing, allowing overwriting as desired
            if (ReadOrAppend && File.Exists(DatabaseFile))
            {
	            try
				{
					// Try to validate the schema. If it is correct, we can use it as is.
					new SchemaValidator(config).Validate();
				    //config.SetProperty("adonet.batch_size", "100");
				}
	            catch (HibernateException)
	            {
					// Validation failed; we need to update the schema.
					// If this fails, we need to report an error.
					// If we are supposed to append, then only 'update' the schema
					// This will also create if it does not exist
					new SchemaUpdate(config).Execute(false, true);
	            }
            }
            else
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

	/// <summary>
	/// Custom naming convention for foreign keys
	/// </summary>
	public class CustomForeignKeyConvention : ForeignKeyConvention
	{
		/// <summary>
		/// Overloaded function to retrieve foreign key names
		/// </summary>
		/// <param name="property"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		protected override string GetKeyName(Member property, Type type)
		{
			if (property == null)
			{
				return type.Name + "Id";  // many-to-many, one-to-many, join
			}

			return property.Name + "Id"; // many-to-one
		}
	}
}
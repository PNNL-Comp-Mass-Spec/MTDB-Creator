using System.Data.SQLite;
using MTDBCreator.Data;
using System.IO;
using System.Collections.Generic;

namespace MTDBCreator.IO
{
    /// <summary>
    /// Class that creates a mass tag database in SQLite format.
    /// 
    /// WSU - Don't use this class.  Write your own that properly maps the data model to the database via an ORM.
    /// </summary>
    public class MultiAlignSqliteMtdbWriter: IMassTagDatabaseWriter
    {
        /// <summary>
        /// Command for inserting mass tags 
        /// </summary>
        private const string CONST_INSERT_COMMAND           = "INSERT INTO {0} VALUES ({1})";
        private const string CONST_PROTEIN_TABLE            = "T_Mass_Tag_To_Protein_Name_Map";
        private const string CONST_MASS_TAG_TABLE           = "T_Mass_Tags_plus_Conformers";
        private const string CONST_PROTEIN_COLUMNS          = "Mass_Tag_ID, Ref_ID, Reference, Protein_ID";
        private const string CONST_MASS_TAG_COLUMNS = "Mass_Tag_ID, Peptide, Net_Value_to_Use, High_Normalized_Score, High_Discriminant_Score, StD_GANET, Monoisotopic_Mass, Min_MSGF_SpecProb, Peptide_Obs_Count_Passing_Filter, Mod_Count, Mod_Description, High_Peptide_Prophet_Probability, Cleavage_State, Drift_Time_Avg, Conformer_Charge, Conformer_ID";

        /// <summary>
        /// Builds a SQL statement for inserting data into the database.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private string BuildTableInsertCommand(string table, string values)
        {
            return string.Format(CONST_INSERT_COMMAND, table, values);
        }

        /// <summary>
        /// Writes the database to the path provided.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="path"></param>
        public void WriteDatabase(MassTagDatabase database, string path)
        {
            // Remove the path if it already exists.
            if (File.Exists(path))            
                File.Delete(path);
            
            string connectionString = string.Format("Data Source = {0}", path);
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("CREATE TABLE {0} ({1})", CONST_MASS_TAG_TABLE, CONST_MASS_TAG_COLUMNS);
                    command.ExecuteNonQuery();

                    command.CommandText = string.Format("CREATE TABLE {0} ({1})", CONST_PROTEIN_TABLE, CONST_PROTEIN_COLUMNS);
                    command.ExecuteNonQuery();
                }


                string massTagValueFormatString = "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}";



                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    // Write the mass tags
                    foreach (ConsensusTarget averageTarget in database.ConsensusTargets)
                    {
                        using (SQLiteCommand command = connection.CreateCommand())
                        {

                            string values = string.Format(massTagValueFormatString,
                                                                            averageTarget.Id,                                      // 0 
                                                                            string.Format("'{0}'", averageTarget.Sequence),        // 1
                                                                            averageTarget.GaNetAverage,                            // 2
                                                                            averageTarget.HighNormalizedScore,                     // 3
                                                                            0,
                                                                            averageTarget.GaNetStdev,                              // 4
                                                                            averageTarget.MonoisotopicMass,                        // 5
                                                                            0,                                                     // 6
                                                                            averageTarget.Targets.Count,                           // 7
                                                                            averageTarget.ModificationCount,                       // 8
                                                                            string.Format("'{0}'", averageTarget.ModificationDescription),                 // 9
                                                                            averageTarget.HighPeptideProphetProbability,           // 10
                                                                            0,                                                      // 11
                                                                            averageTarget.DriftTime,                               // 12
                                                                            averageTarget.ConformerCharge,                         // 13
                                                                            averageTarget.ConformerId);                            // 14
                                                                            
                            string commandText  = BuildTableInsertCommand(  CONST_MASS_TAG_TABLE,                                     
                                                                            values);
                            command.CommandText = commandText;
                            command.ExecuteNonQuery();
                        }
                    }

                    // Write the proteins - each protein may by mapped by multiple mass tags - or consensus targets
                    foreach (Protein protein in database.Proteins)
                    {
                        List<ConsensusTarget> targets = protein.GetMappedTargets();
                        foreach(ConsensusTarget target in targets)
                        {
                            using (SQLiteCommand command = connection.CreateCommand())
                            {
                                string values = string.Format("{0}, {1}, '{2}', {3}", target.Id,                                                                                 
                                                                                 protein.Id,
                                                                                 protein.Reference,
                                                                                 protein.Id);

                                string commandText  = BuildTableInsertCommand(  CONST_PROTEIN_TABLE,                                     
                                                                                values);
                                command.CommandText = commandText;
                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit();
                }
                connection.Close();
            }
        }        
    }
}      
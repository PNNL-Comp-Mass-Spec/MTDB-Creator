using System.Data.SqlClient;
using System.IO;
using System;
using System.Collections.Generic;
using MTDBCreator.DmsExporter.Data;
using PRISM;

namespace MTDBCreator.DmsExporter.IO
{
    public class DmsLookupUtility
    {
        #region "Constants"

        public const string DEFAULT_SEPARATOR = "\t";

        private const string DmsConnectionString = "Data Source=pogo;Initial Catalog=MTS_Master;Integrated Security=SSPI;";

        private const string MainConnectionString = "Data Source=pogo;Initial Catalog=MT_Main;Integrated Security=SSPI";

        private const int MaxRetries = 3;

        #endregion

        private readonly List<int> m_MTIds = new List<int>();

        #region SqlStrings

        private string MassTagAccessDbQuery(AmtPeptideOptions stats)
        {
            return " SELECT Mass_Tag_ID, Peptide, Monoisotopic_Mass, Is_Confirmed, Multiple_Proteins, " +
                          " Created, Last_Affected, Number_Of_Peptides, Peptide_Obs_Count_Passing_Filter, " +
                          " High_Normalized_Score, High_Discriminant_Score, High_Peptide_Prophet_Probability, " +
                          " Min_Log_EValue, Mod_Count, Mod_Description, PMT_Quality_Score, " +
                          " Cleavage_State_Max, Min_MSGF_SpecProb" +
                   " FROM T_Mass_Tags " +
                   " WHERE PMT_Quality_Score >= " + stats.PmtQualityScore +
                   " ORDER BY Mass_Tag_ID ";
        }

        private string MassTagChargeDbQuery(AmtPeptideOptions stats)
        {
            return " SELECT Mass_Tag_ID, Peptide, Charge_State, Scan_Number, " +
                   "        DelM_PPM, GANET_Obs, MH" +
                   " FROM T_Peptides " +
                   " WHERE Mass_Tag_ID IN (SELECT Mass_Tag_ID FROM T_Mass_Tags WHERE PMT_Quality_Score >= " + stats.PmtQualityScore + " ) " +
                   " ORDER BY Mass_Tag_ID ";
        }

        private string ModAccessDbQuery()
        {
            var query = " SELECT Mass_Correction_Tag, Monoisotopic_Mass_Correction, Empirical_Formula " +
                        " FROM V_DMS_Mass_Correction_Factors ";
            return query;
        }

        private string MassTagNetAccessDbQuery(AmtPeptideOptions stats)
        {
            return "SELECT MT.Mass_Tag_ID, MTN.Min_GANET, MTN.Max_GANET, " +
                         " MTN.Avg_GANET, MTN.Cnt_GANET, " +
                         " ISNULL(MTN.StD_GANET, 0) AS StD_GANET, " +
                         " ISNULL(MTN.StdError_GANET, 0) AS StdError_GANET, " +
                         " MTN.PNET " +
                   " FROM T_Mass_Tags MT " +
                        " INNER JOIN T_Mass_Tags_NET MTN " +
                          " ON MT.Mass_tag_ID = MTN.Mass_Tag_ID " +
                   " WHERE MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " AND " +
                         " NOT AVG_GANET Is Null " +
                   " ORDER BY Mass_Tag_ID ";
        }

        // ReSharper disable StringLiteralTypo

        private string ProteinsAccessDbQuery(AmtPeptideOptions stats)
        {

            return "SELECT Prot.Ref_ID, Prot.Reference, Prot.Description, " +
                         " Prot.Protein_Residue_Count, Prot.Monoisotopic_Mass, " +
                         " Prot.Protein_Collection_ID, Prot.Last_Affected, " +
                         " '' AS Protein_Sequence, Prot.Protein_DB_ID, " +
                         " Prot.External_Reference_ID, Prot.External_Protein_ID " +
                    " FROM T_Mass_Tags MT INNER JOIN " +
                         " T_Mass_Tag_to_Protein_Map MTPM ON " +
                         " MT.Mass_Tag_ID = MTPM.Mass_Tag_ID INNER JOIN " +
                         " T_Proteins Prot ON MTPM.Ref_ID = Prot.Ref_ID " +
                    " WHERE MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " AND " +
                          " NOT Protein_Collection_ID Is Null AND " +
                          " NOT Prot.Reference LIKE 'reversed[_]%' AND " +
                          " NOT Prot.Reference LIKE 'rev[_]%' AND " +
                          " NOT Prot.Reference LIKE 'xxx[_]%' " +
                    " GROUP BY Prot.Ref_ID, Prot.Reference, Prot.Description, " +
                             " Prot.Protein_Residue_Count, Prot.Monoisotopic_Mass, " +
                             " Prot.Protein_Collection_ID, Prot.Last_Affected, " +
                             " Prot.Protein_DB_ID, " +
                             " Prot.External_Reference_ID, Prot.External_Protein_ID " +
                    " ORDER BY Prot.Ref_ID ";
        }

        private string ProteinsNullCollectionAccessDbQuery(AmtPeptideOptions stats)
        {
            return "SELECT Prot.Ref_ID, Prot.Reference, Prot.Reference AS Description, " +
                         " 0 AS Protein_Residue_Count, 0 AS Monoisotopic_Mass, " +
                         " 0 AS Protein_Collection_ID, Prot.Last_Affected, " +
                         " '' AS Protein_Sequence, 0 AS Protein_DB_ID, " +
                         " 0 AS External_Reference_ID, 0 ASExternal_Protein_ID " +
                    " FROM T_Mass_Tags MT INNER JOIN " +
                         " T_Mass_Tag_to_Protein_Map MTPM ON " +
                         " MT.Mass_Tag_ID = MTPM.Mass_Tag_ID INNER JOIN " +
                         " T_Proteins Prot ON MTPM.Ref_ID = Prot.Ref_ID " +
                    " WHERE MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " AND " +
                          " Protein_Collection_ID Is Null AND " +
                          " NOT Prot.Reference LIKE 'reversed[_]%' AND " +
                          " NOT Prot.Reference LIKE 'rev[_]%' AND " +
                          " NOT Prot.Reference LIKE 'xxx[_]%' " +
                    " GROUP BY Prot.Ref_ID, Prot.Reference, Prot.Description, " +
                             " Prot.Protein_Residue_Count, Prot.Monoisotopic_Mass, " +
                             " Prot.Protein_Collection_ID, Prot.Last_Affected, " +
                             " Prot.Protein_DB_ID, " +
                             " Prot.External_Reference_ID, Prot.External_Protein_ID " +
                    " ORDER BY Prot.Ref_ID ";
        }

        private string MassTagToProteinMapAccessDbQuery(AmtPeptideOptions stats)
        {

            return "SELECT MTPM.Mass_Tag_ID, MTPM.Mass_Tag_Name, " +
                         " MTPM.Ref_ID, MTPM.Cleavage_State, " +
                         " MTPM.Fragment_Number, MTPM.Fragment_Span, " +
                         " MTPM.Residue_Start, MTPM.Residue_End, " +
                         " MTPM.Repeat_Count, MTPM.Terminus_State, " +
                         " MTPM.Missed_Cleavage_Count " +
                   " FROM T_Mass_Tags MT " +
                        " INNER JOIN T_Mass_Tag_to_Protein_Map MTPM " +
                          " ON MT.Mass_Tag_ID = MTPM.Mass_Tag_ID " +
                        " INNER JOIN T_Proteins Prot " +
                          " ON MTPM.Ref_ID = Prot.Ref_ID " +
                   " WHERE MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " AND " +
                         " NOT Mass_Tag_Name IS NULL AND " +
                         " NOT Prot.Reference LIKE 'reversed[_]%' AND " +
                         " NOT Prot.Reference LIKE 'rev[_]%' AND " +
                         " NOT Prot.Reference LIKE 'xxx[_]%' " +
                   " ORDER BY MT.Mass_Tag_ID, MTPM.Ref_ID ";
        }

        private string MassTagNullToProteinMapAccessDbQuery(AmtPeptideOptions stats)
        {
            return "SELECT MTPM.Mass_Tag_ID, '' AS Mass_Tag_Name, " +
                         " MTPM.Ref_ID, MTPM.Cleavage_State, " +
                         " 0 AS Fragment_Number, 0 AS Fragment_Span, " +
                         " 0 AS Residue_Start, 0 AS Residue_End, " +
                         " 0 AS Repeat_Count, MTPM.Terminus_State, " +
                         " 0 AS Missed_Cleavage_Count " +
                   " FROM T_Mass_Tags MT " +
                        " INNER JOIN T_Mass_Tag_to_Protein_Map MTPM " +
                          " ON MT.Mass_Tag_ID = MTPM.Mass_Tag_ID " +
                        " INNER JOIN T_Proteins Prot " +
                          " ON MTPM.Ref_ID = Prot.Ref_ID " +
                   " WHERE MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " AND " +
                         " Mass_Tag_Name IS NULL AND " +
                         " NOT Prot.Reference LIKE 'reversed[_]%' AND " +
                         " NOT Prot.Reference LIKE 'rev[_]%' AND " +
                         " NOT Prot.Reference LIKE 'xxx[_]%' " +
                   " ORDER BY MT.Mass_Tag_ID, MTPM.Ref_ID ";
        }

        // ReSharper restore StringLiteralTypo

        private string AnalysisDescriptionAccessDbQuery()
        {
            return "SELECT Job," +
                         " Dataset, " +
                         " Dataset_ID, " +
                         " Dataset_Created_DMS, " +
                         " Dataset_Acq_Time_Start, " +
                         " Dataset_Acq_Time_End, " +
                         " Dataset_Acq_Length, " +
                         " Dataset_Scan_Count, " +
                         " Experiment, " +
                         " Campaign, " +
                         " Experiment_Organism, " +
                         " Instrument_Class, " +
                         " Instrument, " +
                         " Analysis_Tool, " +
                         " Parameter_File_Name, " +
                         " Settings_File_Name, " +
                         " Organism_DB_Name, " +
                         " Protein_Collection_List, " +
                         " Protein_Options_List, " +
                         " Completed, " +
                         " ResultType, " +
                         " Separation_Sys_Type, " +
                         " RowCount_Loaded, " +
                         " ScanTime_NET_Slope, " +
                         " ScanTime_NET_Intercept, " +
                         " ScanTime_NET_RSquared, " +
                         " ScanTime_NET_Fit, " +
                         " Regression_Order, " +
                         " Regression_Filtered_Data_Count, " +
                         " Regression_Equation " +
                   " FROM T_Analysis_Description " +
                   " ORDER BY Job";
        }

        private string FilterSetOverviewAccessDbQuery()
        {
            return " SELECT Filter_Type, Filter_Set_ID, Extra_Info, Filter_Set_Name, Filter_Set_Description " +
                   " FROM V_Filter_Set_Overview_Ex";
        }

        #endregion

        public string Separator { get; set; } = DEFAULT_SEPARATOR;

        private readonly string m_connectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        public DmsLookupUtility()
            : this(DmsConnectionString)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public DmsLookupUtility(string connectionString)
        {
            m_connectionString = connectionString;
        }

        /// <summary>
        /// Writes the information from the database to a set of temporary
        /// files which are deleted when the database is fully written. This
        /// workflow is necessary for the batch writing to sql or access file
        /// </summary>
        /// <param name="outFilePath"></param>
        /// <param name="selectedDb"></param>
        /// <param name="selectedStats"></param>
        /// <returns>True if success, otherwise false</returns>
        public bool ExportToText(string outFilePath, AmtInfo selectedDb, AmtPeptideOptions selectedStats)
        {
            var outFile = new FileInfo(outFilePath);

            var directoryPath = outFile.DirectoryName;
            if (directoryPath == null)
            {
                ConsoleMsgUtils.ShowWarning("Unable to determine the parent directory of " + outFilePath);
                return false;
            }

            var connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;",
                                                    selectedDb.Server,
                                                    selectedDb.Name);

            try
            {
                var retryCount = MaxRetries;
                if (retryCount < 1)
                    retryCount = 1;

                while (retryCount > 0)
                {
                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            var massTagSql = MassTagAccessDbQuery(selectedStats);
                            DbValueUtilities.CurrentQueryInfo = "T_Mass_Tags";

                            var cmd = new SqlCommand(massTagSql, connection);
                            var reader = cmd.ExecuteReader();

                            var modDescriptions = new List<string>();
                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempMassTags.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Mass_Tag_ID",
                                    "Peptide",
                                    "Monoisotopic_Mass",
                                    "Is_Confirmed",
                                    "Multiple_Proteins",
                                    "Created",
                                    "Last_Affected",
                                    "Number_Of_Peptides",
                                    "Peptide_Obs_Count_Passing_Filter",
                                    "High_Normalized_Score",
                                    "High_Discriminant_Score",
                                    "High_Peptide_Prophet_Probability",
                                    "Min_Log_EValue",
                                    "Mod_Count",
                                    "Mod_Description",
                                    "PMT_Quality_Score",
                                    "Cleavage_State_Max",
                                    "Min_MSGF_SpecProb"
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    m_MTIds.Add(DbValueUtilities.GetDbFieldValue(reader, 0, 0));

                                    var mods = DbValueUtilities.GetDbFieldValue(reader, 14);
                                    var modsList = mods.Split(',');
                                    foreach (var mod in modsList)
                                    {
                                        var modPieces = mod.Split(':');
                                        if (modPieces[0] != "" && !modDescriptions.Contains(modPieces[0]))
                                            modDescriptions.Add(modPieces[0]);
                                    }

                                    var dataValues = new List<string> {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),                     // Mass_Tag_ID
                                        DbValueUtilities.GetDbFieldValue(reader, 1),                        // Peptide
                                        DbValueUtilities.DbValueToStringFixedDecimals(reader, 2, 0.0, 5),   // Monoisotopic_Mass
                                        DbValueUtilities.DbValueToString(reader, 3, (byte)0),               // Is_Confirmed
                                        DbValueUtilities.DbValueToString(reader, 4, (short)0),                     // Multiple_Proteins
                                        DbValueUtilities.DbDateToString(reader, 5),                         // Created
                                        DbValueUtilities.DbDateToString(reader, 6),                         // Last_Affected
                                        DbValueUtilities.DbValueToString(reader, 7, 0),                     // Number_Of_Peptides
                                        DbValueUtilities.DbValueToString(reader, 8, 0),                     // Peptide_Obs_Count_Passing_Filter
                                        DbValueUtilities.DbValueToString(reader, 9, (float)0),              // High_Normalized_Score
                                        DbValueUtilities.DbValueToString(reader, 10, (float)0),             // High_Discriminant_Score
                                        DbValueUtilities.DbValueToString(reader, 11, (float)0),             // High_Peptide_Prophet_Probability
                                        DbValueUtilities.DbValueToString(reader, 12, (float)0),             // Min_Log_EValue
                                        DbValueUtilities.DbValueToString(reader, 13, 0),                    // Mod_Count
                                        "\"" + mods + "\"",                                                 // Mod_Description (always surround with double quotes, due to logic in RetrieveDataFromMassTagsFile)
                                        DbValueUtilities.DbValueToString(reader, 15, (decimal)0),           // PMT_Quality_Score
                                        DbValueUtilities.DbValueToString(reader, 16, (byte)0),              // Cleavage_State_Max
                                        DbValueUtilities.DbValueToString(reader, 17, (float)0),             // Min_MSGF_SpecProb
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));
                                }
                                reader.Close();
                            }

                            var chargeSql = MassTagChargeDbQuery(selectedStats);
                            DbValueUtilities.CurrentQueryInfo = "T_Peptides";

                            cmd = new SqlCommand(chargeSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempPeptides.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Mass_Tag_ID",
                                    "Peptide",
                                    "ChargeState",
                                    "Scan_Number",
                                    "DelM_PPM",
                                    "GANET_Obs",
                                    "MH"
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    var amtID = DbValueUtilities.GetDbFieldValue(reader, 0, 0);

                                    if (m_MTIds.Contains(amtID))
                                    {
                                        var dataValues = new List<string> {
                                            amtID.ToString(),                                           // Mass_Tag_ID
                                            DbValueUtilities.GetDbFieldValue(reader, 1),                // Peptide
                                            DbValueUtilities.DbValueToString(reader, 2, (short)0),      // ChargeState
                                            DbValueUtilities.DbValueToString(reader, 3, 0),             // Scan_Number
                                            DbValueUtilities.DbValueToString(reader, 4, (float)0),        // DelM_PPM
                                            DbValueUtilities.DbValueToString(reader, 5, (float)0),        // GANET_Obs
                                            DbValueUtilities.DbValueToStringFixedDecimals(reader, 6, 0.0, 4)    // MH
                                        };

                                        writer.WriteLine(string.Join(Separator, dataValues));
                                    }
                                }
                                reader.Close();
                            }

                            var massTagModsSql = ModAccessDbQuery();
                            DbValueUtilities.CurrentQueryInfo = "V_DMS_Mass_Correction_Factors";

                            var mainDb = new SqlConnection(MainConnectionString);
                            mainDb.Open();
                            cmd = new SqlCommand(massTagModsSql, mainDb);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempModInfo.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Mod_tag",
                                    "Mod_Mass",
                                    "Mod_formula"
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    var dataValues = new List<string> {
                                        DbValueUtilities.GetDbFieldValue(reader, 0),                        // Mass_Correction_Tag
                                        DbValueUtilities.DbValueToStringFixedDecimals(reader, 1, 0.0, 5),   // Monoisotopic_Mass_Correction
                                        DbValueUtilities.GetDbFieldValue(reader, 2),                        // Empirical_Formula
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }
                            mainDb.Close();

                            var massTagNetSql = MassTagNetAccessDbQuery(selectedStats);
                            DbValueUtilities.CurrentQueryInfo = "T_Mass_Tags_NET";

                            cmd = new SqlCommand(massTagNetSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempMassTagsNet.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Mass_Tag_ID",
                                    "Min_GANET",
                                    "Max_GANET",
                                    "Avg_GANET",
                                    "Cnt_GANET",
                                    "StD_GANET",
                                    "StdError_GANET",
                                    "PNET",
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    var dataValues = new List<string> {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),         // Mass_Tag_ID
                                        DbValueUtilities.DbValueToString(reader, 1, (float)0),  // Min_GANET
                                        DbValueUtilities.DbValueToString(reader, 2, (float)0),  // Max_GANET
                                        DbValueUtilities.DbValueToString(reader, 3, (float)0),  // Avg_GANET
                                        DbValueUtilities.DbValueToString(reader, 4, 0),         // Cnt_GANET
                                        DbValueUtilities.DbValueToString(reader, 5, (float)0),  // StD_GANET
                                        DbValueUtilities.DbValueToString(reader, 6, (float)0),  // StdError_GANET
                                        DbValueUtilities.DbValueToString(reader, 7, (float)0),  // PNET
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }

                            var proteinsSql = ProteinsAccessDbQuery(selectedStats);
                            var proteinsNullCollectionSql = ProteinsNullCollectionAccessDbQuery(selectedStats);
                            DbValueUtilities.CurrentQueryInfo = "T_Proteins (part 1)";

                            cmd = new SqlCommand(proteinsSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempProteins.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Ref_ID",
                                    "Reference",
                                    "Description",
                                    "Protein_Residue_Count",
                                    "Monoisotopic_Mass",
                                    "Protein_Collection_ID",
                                    "Last_Affected",
                                    "Protein_Sequence",
                                    "Protein_DB_ID",
                                    "External_Reference_ID",
                                    "External_Protein_ID",
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    var dataValues = new List<string>
                                    {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),                     // Ref_ID
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 1), Separator), // Reference
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 2), Separator), // Description
                                        DbValueUtilities.DbValueToString(reader, 3, 0),                     // Protein_Residue_Count
                                        DbValueUtilities.DbValueToStringFixedDecimals(reader, 4, 0.0, 5),   // Monoisotopic_Mass
                                        DbValueUtilities.DbValueToString(reader, 5, 0),                     // Protein_Collection_ID
                                        DbValueUtilities.DbDateToString(reader, 6),                         // Last_Affected
                                        DbValueUtilities.GetDbFieldValue(reader, 7),                        // Protein_Sequence
                                        DbValueUtilities.DbValueToString(reader, 8, 0),                     // Protein_DB_ID
                                        DbValueUtilities.DbValueToString(reader, 9, 0),                     // External_Reference_ID
                                        DbValueUtilities.DbValueToString(reader, 10, 0)                     // External_Protein_ID
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }

                            // Append proteins where Protein_Collection_ID is null
                            DbValueUtilities.CurrentQueryInfo = "T_Proteins (part 2)";

                            cmd = new SqlCommand(proteinsNullCollectionSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempProteins.txt"), true))
                            {
                                while (reader.Read())
                                {
                                    var dataValues = new List<string>
                                    {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),                     // Ref_ID
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 1), Separator), // Reference
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 2), Separator), // Description
                                        DbValueUtilities.DbValueToString(reader, 3, 0, 4),                  // Protein_Residue_Count
                                        DbValueUtilities.DbValueToStringFixedDecimals(reader, 4, 0.0, 5),   // Monoisotopic_Mass
                                        DbValueUtilities.DbValueToString(reader, 5, 0),                     // Protein_Collection_ID
                                        DbValueUtilities.DbDateToString(reader, 6),                         // Last_Affected
                                        DbValueUtilities.GetDbFieldValue(reader, 7),                        // Protein_Sequence
                                        DbValueUtilities.DbValueToString(reader, 8, 0),                     // Protein_DB_ID
                                        DbValueUtilities.DbValueToString(reader, 9, 0),                     // External_Reference_ID
                                        DbValueUtilities.DbValueToString(reader, 10, 0)                     // External_Protein_ID
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }

                            var massTagToProt = MassTagToProteinMapAccessDbQuery(selectedStats);
                            var massTagNullToProt = MassTagNullToProteinMapAccessDbQuery(selectedStats);
                            DbValueUtilities.CurrentQueryInfo = "T_Mass_Tag_to_Protein_Map (part 1)";

                            cmd = new SqlCommand(massTagToProt, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempMassTagToProteins.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Mass_Tag_ID",
                                    "Mass_Tag_Name",
                                    "Ref_ID",
                                    "Cleavage_State",
                                    "Fragment_Number",
                                    "Fragment_Span",
                                    "Residue_Start",
                                    "Residue_End",
                                    "Repeat_Count",
                                    "Terminus_State",
                                    "Missed_Cleavage_Count",
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    var dataValues = new List<string>
                                    {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),         // Mass_Tag_ID
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 1), Separator), // Mass_Tag_Name
                                        DbValueUtilities.DbValueToString(reader, 2, 0),         // Ref_ID
                                        DbValueUtilities.DbValueToString(reader, 3, (byte)0),   // Cleavage_State
                                        DbValueUtilities.DbValueToString(reader, 4, (short)0),  // Fragment_Number
                                        DbValueUtilities.DbValueToString(reader, 5, (short)0),  // Fragment_Span
                                        DbValueUtilities.DbValueToString(reader, 6, 0),         // Residue_Start
                                        DbValueUtilities.DbValueToString(reader, 7, 0),         // Residue_End
                                        DbValueUtilities.DbValueToString(reader, 8, (short)0),  // Repeat_Count
                                        DbValueUtilities.DbValueToString(reader, 9, (byte)0),   // Terminus_State
                                        DbValueUtilities.DbValueToString(reader, 10, (short)0)  // Missed_Cleavage_Count
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }

                            DbValueUtilities.CurrentQueryInfo = "T_Mass_Tag_to_Protein_Map (part 2)";

                            cmd = new SqlCommand(massTagNullToProt, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempMassTagToProteins.txt"), true))
                            {
                                while (reader.Read())
                                {
                                    var dataValues = new List<string>
                                    {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),         // Mass_Tag_ID
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 1), Separator), // Mass_Tag_Name
                                        DbValueUtilities.DbValueToString(reader, 2, 0),         // Ref_ID
                                        DbValueUtilities.DbValueToString(reader, 3, (byte)0),   // Cleavage_State
                                        DbValueUtilities.DbValueToString(reader, 4, (short)0),  // Fragment_Number
                                        DbValueUtilities.DbValueToString(reader, 5, (short)0),  // Fragment_Span
                                        DbValueUtilities.DbValueToString(reader, 6, 0),         // Residue_Start
                                        DbValueUtilities.DbValueToString(reader, 7, 0),         // Residue_End
                                        DbValueUtilities.DbValueToString(reader, 8, (short)0),  // Repeat_Count
                                        DbValueUtilities.DbValueToString(reader, 9, (byte)0),   // Terminus_State
                                        DbValueUtilities.DbValueToString(reader, 10, (short)0)  // Missed_Cleavage_Count
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }

                            var analysisDescription = AnalysisDescriptionAccessDbQuery();
                            DbValueUtilities.CurrentQueryInfo = "T_Analysis_Description";

                            cmd = new SqlCommand(analysisDescription, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempAnalysisDescription.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Job",
                                    "Dataset",
                                    "Dataset_ID",
                                    "Dataset_Created_DMS",
                                    "Dataset_Acq_Time_Start",
                                    "Dataset_Acq_Time_End",
                                    "Dataset_Acq_Length",
                                    "Dataset_Scan_Count",
                                    "Experiment",
                                    "Campaign",
                                    "Experiment_Organism",
                                    "Instrument_Class",
                                    "Instrument",
                                    "Analysis_Tool",
                                    "Parameter_File_Name",
                                    "Settings_File_Name",
                                    "Organism_DB_Name",
                                    "Protein_Collection_List",
                                    "Protein_Options_List",
                                    "Completed",
                                    "ResultType",
                                    "Separation_Sys_Type",
                                    "RowCount_Loaded",
                                    "ScanTime_Net_Slope",
                                    "ScanTime_Net_Intercept",
                                    "ScanTime_Net_RSquared",
                                    "ScanTime_Net_Fit",
                                    "Regression_Order",
                                    "Regression_Filtered_Data_Count",
                                    "Regression_Equation",
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {

                                    var dataValues = new List<string>
                                    {
                                        DbValueUtilities.DbValueToString(reader, 0, 0),         // Job
                                        DbValueUtilities.GetDbFieldValue(reader, 1),            // Dataset
                                        DbValueUtilities.DbValueToString(reader, 2, 0),         // Dataset_ID
                                        DbValueUtilities.DbDateToString(reader, 3),             // Dataset_Created_DMS
                                        DbValueUtilities.DbDateToString(reader, 4),             // Dataset_Acq_Time_Start
                                        DbValueUtilities.DbDateToString(reader, 5),             // Dataset_Acq_Time_End
                                        DbValueUtilities.DbValueToString(reader, 6, (decimal)0),// Dataset_Acq_Length
                                        DbValueUtilities.DbValueToString(reader, 7, 0),         // Dataset_Scan_Count
                                        DbValueUtilities.GetDbFieldValue(reader, 8),            // Experiment
                                        DbValueUtilities.GetDbFieldValue(reader, 9),            // Campaign
                                        DbValueUtilities.GetDbFieldValue(reader, 10),           // Experiment_Organism
                                        DbValueUtilities.GetDbFieldValue(reader, 11),           // Instrument_Class
                                        DbValueUtilities.GetDbFieldValue(reader, 12),           // Instrument
                                        DbValueUtilities.GetDbFieldValue(reader, 13),           // Analysis_Tool
                                        DbValueUtilities.GetDbFieldValue(reader, 14),           // Parameter_File_Name
                                        DbValueUtilities.GetDbFieldValue(reader, 15),           // Settings_File_Name
                                        DbValueUtilities.GetDbFieldValue(reader, 16),           // Organism_DB_Name
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 17), Separator),        // Protein_Collection_List
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 18), Separator),        // Protein_Options_List
                                        DbValueUtilities.DbDateToString(reader, 19),                    // Completed
                                        DbValueUtilities.GetDbFieldValue(reader, 20),                   // ResultType
                                        DbValueUtilities.GetDbFieldValue(reader, 21),                   // Separation_Sys_Type
                                        DbValueUtilities.DbValueToString(reader, 22, 0),                // RowCount_Loaded
                                        DbValueUtilities.DbValueToString(reader, 23, (float)0),         // ScanTime_Net_Slope
                                        DbValueUtilities.DbValueToString(reader, 24, (float)0),         // ScanTime_Net_Intercept
                                        DbValueUtilities.DbValueToString(reader, 25, (float)0),         // ScanTime_Net_RSquared
                                        DbValueUtilities.DbValueToString(reader, 26, (float)0),         // ScanTime_Net_Fit
                                        DbValueUtilities.DbValueToString(reader, 27, (byte)0),          // Regression_Order
                                        DbValueUtilities.DbValueToString(reader, 28, 0),                // Regression_Filtered_Data_Count
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 29), Separator),        // Regression_Equation
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));
                                }
                                reader.Close();
                            }

                            var filterSet = FilterSetOverviewAccessDbQuery();
                            DbValueUtilities.CurrentQueryInfo = "V_Filter_Set_Overview_Ex";

                            cmd = new SqlCommand(filterSet, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(Path.Combine(directoryPath, "tempFilterSet.txt")))
                            {
                                var headerNames = new List<string> {
                                    "Filter_Type",
                                    "Filter_Set_ID",
                                    "Extra_Info",
                                    "Filter_Set_Name",
                                    "Filter_Set_Description",
                                };

                                writer.WriteLine(string.Join(Separator, headerNames));

                                while (reader.Read())
                                {
                                    var dataValues = new List<string>
                                    {
                                        DbValueUtilities.GetDbFieldValue(reader, 0),                                                    // Filter_Type
                                        DbValueUtilities.DbValueToString(reader, 1, 0),                                                 // Filter_Set_ID
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 2), Separator),   // Extra_Info
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 3), Separator),   // Filter_Set_Name
                                        DbValueUtilities.PossiblyQuoteString(DbValueUtilities.GetDbFieldValue(reader, 4), Separator)    // Filter_Set_Description
                                    };

                                    writer.WriteLine(string.Join(Separator, dataValues));

                                }
                                reader.Close();
                            }

                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        retryCount -= 1;
                        var msg = "Exception querying database in Export: " + ex.Message;
                        msg += ", RetryCount = " + retryCount;

                        Console.WriteLine(msg);

                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Exception connecting to database in Export: " + ex.Message + "; ConnectionString: " + m_connectionString;
                Console.WriteLine(msg);
                return false;
            }

            DbValueUtilities.CurrentQueryInfo = "";
            return false;
        }

        public Dictionary<string, AmtInfo> GetDatabases()
        {
            var databaseNameFilter = string.Empty;
            return GetDatabases(databaseNameFilter);
        }

        public Dictionary<string, AmtInfo> GetDatabases(string datasetNameFilter)
        {
            var mtdbDictionary = new Dictionary<string, AmtInfo>();

            try
            {
                var sql = " SELECT MT_DB_Name, Description, Organism, Campaign, State, State_ID, Server_Name" +
                          " FROM V_Active_MT_DBs " +
                          " WHERE State_ID < 15";
                DbValueUtilities.CurrentQueryInfo = "V_Active_MT_DBs";

                var retryCount = MaxRetries;
                if (retryCount < 1)
                    retryCount = 1;

                while (retryCount > 0)
                {
                    try
                    {
                        using (var connection = new SqlConnection(m_connectionString))
                        {
                            connection.Open();

                            var cmd = new SqlCommand(sql, connection);
                            var reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                var amtInfo = new AmtInfo
                                {
                                    Name = DbValueUtilities.GetDbFieldValue(reader, 0),
                                    Description = DbValueUtilities.GetDbFieldValue(reader, 1),
                                    Organism = DbValueUtilities.GetDbFieldValue(reader, 2),
                                    Campaign = DbValueUtilities.GetDbFieldValue(reader, 3),
                                    State = DbValueUtilities.GetDbFieldValue(reader, 4),
                                    StateId = DbValueUtilities.GetDbFieldValue(reader, 5, 0),
                                    Server = DbValueUtilities.GetDbFieldValue(reader, 6)
                                };

                                mtdbDictionary.Add(amtInfo.Name, amtInfo);

                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        retryCount -= 1;
                        var msg = "Exception querying database in GetDatasets: " + ex.Message;
                        msg += ", RetryCount = " + retryCount;

                        Console.WriteLine(msg);

                        // Delay 0.5 sec then try again
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Exception connecting to database in GetDatasets: " + ex.Message + "; ConnectionString: " + m_connectionString;
                Console.WriteLine(msg);
            }

            DbValueUtilities.CurrentQueryInfo = "";
            return mtdbDictionary;

        }

        public Dictionary<double, AmtPeptideOptions> GetStats(AmtInfo amt)
        {
            var statsDictionary = new Dictionary<double, AmtPeptideOptions>();

            var connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;",
                                                    amt.Server,
                                                    amt.Name);

            try
            {
                var sql =
                    " SELECT PMT_Quality_Score, MT_Count_Passing_QS, Filter_Set_ID, Filter_Set_Name, Filter_Set_Description " +
                    " FROM V_PMT_Quality_Score_Report ";
                DbValueUtilities.CurrentQueryInfo = "V_PMT_Quality_Score_Report";

                var retryCount = MaxRetries;
                if (retryCount < 1)
                    retryCount = 1;

                while (retryCount > 0)
                {
                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            var cmd = new SqlCommand(sql, connection);
                            var reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                var pmtQS = DbValueUtilities.GetDbFieldValue(reader, 0, (decimal)0);    // PMT_Quality_Score

                                var peptideOptions = new AmtPeptideOptions
                                {
                                    PmtQualityScore = pmtQS,
                                    MtCountPassing = DbValueUtilities.GetDbFieldValue(reader, 1, 0),    // MT_Count_Passing_QS
                                    FilterSetId = DbValueUtilities.GetDbFieldValue(reader, 2, 0),       // Filter_Set_ID
                                    FilterSetName = DbValueUtilities.GetDbFieldValue(reader, 3),        // Filter_Set_Name
                                    FilterSetDescription = DbValueUtilities.GetDbFieldValue(reader, 4)  // Filter_Set_Description
                                };

                                var pmtQSDouble = Convert.ToDouble(peptideOptions.PmtQualityScore);
                                if (!statsDictionary.ContainsKey(pmtQSDouble))
                                {
                                    statsDictionary.Add(pmtQSDouble, peptideOptions);
                                }

                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        retryCount -= 1;
                        var msg = "Exception querying database in GetStats: " + ex.Message;
                        msg += ", RetryCount = " + retryCount;

                        Console.WriteLine(msg);

                        // Delay 0.5 sec then try again
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = "Exception connecting to database in GetStats: " + ex.Message + "; ConnectionString: " + m_connectionString;
                Console.WriteLine(msg);
            }

            DbValueUtilities.CurrentQueryInfo = "";
            return statsDictionary;

        }

    }
}

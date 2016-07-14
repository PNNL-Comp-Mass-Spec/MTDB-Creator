using System.Data.SqlClient;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using MTDBCreator.DmsExporter.Data;

namespace MTDBCreator.DmsExporter.IO
{
    public class DmsLookupUtility
    {
        #region "Constants"

        private const string DmsConnectionString = "Data Source=pogo;Initial Catalog=MTS_Master;Integrated Security=SSPI;";

        private const string MainConnectionString = "Data Source=pogo;Initial Catalog=MT_Main;Integrated Security=SSPI";

        private const int MaxRetries = 3;

        #endregion

        private List<int> m_MTIds = new List<int>();

        private char m_separator = '\t';

        #region SqlStrings

        private string MassTagAccessDbQuery(AmtPeptideOptions stats)
        {
            return " SELECT Mass_Tag_ID, Peptide, Monoisotopic_Mass, Is_Confirmed, Multiple_Proteins, " +
                          " Created, Last_Affected, Number_Of_Peptides, Peptide_Obs_Count_Passing_Filter, " +
                          " High_Normalized_Score, High_Discriminant_Score, High_Peptide_Prophet_Probability, " +
                          " Min_Log_EValue, Mod_Count, Mod_Description, PMT_Quality_Score, " +
                          " Cleavage_State_Max, Min_MSGF_SpecProb" +
                          " FROM T_Mass_Tags MT " +
                          " WHERE (MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " ) " +
                          " ORDER BY Mass_Tag_ID ";
        }

        private string MassTagChargeDbQuery()
        {
            return " SELECT * " +
                   " FROM T_Peptides " +
                   " WHERE Mass_Tag_ID BETWEEN " + m_MTIds.Min() + " AND " + m_MTIds.Max() + " " +
                   " ORDER BY Mass_Tag_ID ";
        }

        private string ModAccessDbQuery()
        {
            string query = " SELECT * " +
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
                    " WHERE (MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " ) AND " +
                    " NOT AVG_GANET Is Null " +
                    " ORDER BY Mass_Tag_ID ";
        }


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
                    " WHERE (MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " ) AND " +
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
                    " WHERE (MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " ) AND " +
                    " Protein_Collection_ID Is Null  AND " +
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
                    " WHERE (MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " ) AND " +
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
                    " WHERE (MT.PMT_Quality_Score >= " + stats.PmtQualityScore + " ) AND " +
                    " Mass_Tag_Name IS NULL  AND " +
                    " NOT Prot.Reference LIKE 'reversed[_]%' AND " +
                    " NOT Prot.Reference LIKE 'rev[_]%' AND " +
                    " NOT Prot.Reference LIKE 'xxx[_]%' " +
                    " ORDER BY MT.Mass_Tag_ID, MTPM.Ref_ID ";
        }

        private string AnalysisDescriptionAccessDbQuery()
        {
            return "SELECT Job," +
                   " Dataset, " +
                   " Dataset_ID, " +
                   " CONVERT(varchar(64), Dataset_Created_DMS, 101) + ' ' +  " +
                   " CONVERT(varchar(64), Dataset_Created_DMS, 108) AS Dataset_Created_DMS, " +
                   " CONVERT(varchar(64), Dataset_Acq_Time_Start, 101) + ' ' +  " +
                   " CONVERT(varchar(64), Dataset_Acq_Time_Start, 108) AS Dataset_Acq_Time_Start, " +
                   " CONVERT(varchar(64), Dataset_Acq_Time_End, 101) + ' ' +  " +
                   " CONVERT(varchar(64), Dataset_Acq_Time_End, 108) AS Dataset_Acq_Time_End, " +
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
                   " CONVERT(varchar(64), Completed, 101) + ' ' +  " +
                   " CONVERT(varchar(64), Completed, 108) AS Completed, " +
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
            return "SELECT * FROM V_Filter_Set_Overview_Ex";
        }

        #endregion

        public char Separator {
            get { return m_separator; }
            set { m_separator = value; }
        }

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
        /// workflow is necesary for the batch writing to sql or access file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="selectedDb"></param>
        /// <param name="selectedStats"></param>
        public void ExportToText(string path, AmtInfo selectedDb, AmtPeptideOptions selectedStats)
        {
            var pieces = path.Split('\\');
            string directory = "";
            foreach (var piece in pieces)
            {
                if (piece.Contains("."))
                {
                    continue;
                }
                directory += piece;
                directory += "\\";
            }

            var connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;",
                                                    selectedDb.Server,
                                                    selectedDb.Name);

            try
            {
                int retryCount = MaxRetries;
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

                            var cmd = new SqlCommand(massTagSql, connection);
                            var reader = cmd.ExecuteReader();

                            var modDescriptions = new List<string>();
                            using (var writer = new StreamWriter(directory + "tempMassTags.txt"))
                            {
                                var header = string.Format("{0}{18}{1}{18}{2}{18}{3}{18}{4}{18}{5}{18}{6}{18}{7}{18}{8}{18}{9}{18}{10}{18}{11}{18}{12}{18}{13}{18}{14}{18}{15}{18}{16}{18}{17}",
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
                                                            "Min_MSGF_SpecProb",
                                                            m_separator
                                                            );
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    m_MTIds.Add(Convert.ToInt32(reader.GetValue(0)));

                                    string mods = "";
                                    if (!Convert.IsDBNull(reader.GetValue(14)))
                                        mods = Convert.ToString(reader.GetValue(14));
                                    var modsList = mods.Split(',');
                                    foreach (var mod in modsList)
                                    {
                                        var modPieces = mod.Split(':');
                                        if (modPieces[0] != "" && !modDescriptions.Contains(modPieces[0]))
                                            modDescriptions.Add(modPieces[0]);
                                    }
                                    var line = string.Format("{0}{18}{1}{18}{2}{18}{3}{18}{4}{18}{5}{18}{6}{18}{7}{18}{8}{18}{9}{18}{10}{18}\"{11}\"{18}{12}{18}{13}{18}\"{14}\"{18}{15}{18}{16}{18}{17}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : "",
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : "",
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : "",
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetValue(10) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(11)) ? reader.GetValue(11) : "0",
                                                                !Convert.IsDBNull(reader.GetValue(12)) ? reader.GetValue(12) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(13)) ? reader.GetValue(13) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(14)) ? reader.GetValue(14) : "",
                                                                !Convert.IsDBNull(reader.GetValue(15)) ? reader.GetValue(15) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(16)) ? reader.GetValue(16) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(17)) ? reader.GetValue(17) : 0,
                                                                m_separator);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }
                            var chargeSql = MassTagChargeDbQuery();
                            cmd = new SqlCommand(chargeSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempPeptides.txt"))
                            {
                                var header = string.Format("{0}{7}{1}{7}{2}{7}{3}{7}{4}{7}{5}{7}{6}",
                                                            "Mass_Tag_ID",
                                                            "Peptide",
                                                            "ChargeState",
                                                            "Scan_Number",
                                                            "DelM_PPM",
                                                            "GANET_Obs",
                                                            "MH",
                                                            m_separator);
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    if (m_MTIds.Contains(Convert.ToInt32(reader.GetValue(8))))
                                    {
                                        var line = string.Format("{0}{7}{1}{7}{2}{7}{3}{7}{4}{7}{5}{7}{6}",
                                            !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                            !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : "",
                                            !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                            !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                            !Convert.IsDBNull(reader.GetValue(16)) ? reader.GetValue(16) : 0,
                                            !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                            !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                            m_separator);
                                        writer.WriteLine(line);
                                    }
                                }
                                reader.Close();
                            }

                            var massTagModsSql = ModAccessDbQuery();
                            var mainDb = new SqlConnection(MainConnectionString);
                            mainDb.Open();
                            cmd = new SqlCommand(massTagModsSql, mainDb);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempModInfo.txt"))
                            {
                                var header = string.Format("{0}{3}{1}{3}{2}",
                                                            "Mod_tag",
                                                            "Mod_Mass",
                                                            "Mod_formula",
                                                            m_separator);
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{3}{1}{3}{2}",
                                        !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : 0,
                                        !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                        !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                        m_separator);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }
                            mainDb.Close();

                            var massTagNetSql = MassTagNetAccessDbQuery(selectedStats);
                            cmd = new SqlCommand(massTagNetSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempMassTagsNet.txt"))
                            {
                                var header = string.Format("{0}{8}{1}{8}{2}{8}{3}{8}{4}{8}{5}{8}{6}{8}{7}",
                                                            "Mass_Tag_ID",
                                                            "Min_GANET",
                                                            "Max_GANET",
                                                            "Avg_GANET",
                                                            "Cnt_GANET",
                                                            "StD_GANET",
                                                            "StdError_GANET",
                                                            "PNET",
                                                            m_separator
                                                            );
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{8}{1}{8}{2}{8}{3}{8}{4}{8}{5}{8}{6}{8}{7}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : 0,
                                                                m_separator);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                            var proteinsSql = ProteinsAccessDbQuery(selectedStats);
                            var proteinsNullCollectionSql = ProteinsNullCollectionAccessDbQuery(selectedStats);

                            cmd = new SqlCommand(proteinsSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempProteins.txt"))
                            {
                                var header = string.Format("{0}{11}\"{1}\"{11}\"{2}\"{11}{3}{11}{4}{11}{5}{11}{6}{11}{7}{11}{8}{11}{9}{11}{10}",
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
                                                            m_separator
                                                            );
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{11}\"{1}\"{11}\"{2}\"{11}{3}{11}{4}{11}{5}{11}{6}{11}{7}{11}{8}{11}{9}{11}{10}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : "",
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : "",
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : "",
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : "",
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : "",
                                                                !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetValue(10) : 0,
                                                                m_separator);
                                    writer.WriteLine(line);

                                }
                                reader.Close();
                            }

                            cmd = new SqlCommand(proteinsNullCollectionSql, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempProteins.txt", true))
                            {
                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{11}\"{1}\"{11}\"{2}\"{11}{3}{11}{4}{11}{5}{11}{6}{11}{7}{11}{8}{11}{9}{11}{10}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : "",
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : "",
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : "",
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : "",
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : "",
                                                                !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetValue(10) : 0,
                                                                m_separator);
                                    writer.WriteLine(line);

                                }
                                reader.Close();
                            }

                            var massTagToProt = MassTagToProteinMapAccessDbQuery(selectedStats);
                            var massTagNullToProt = MassTagNullToProteinMapAccessDbQuery(selectedStats);

                            cmd = new SqlCommand(massTagToProt, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempMassTagToProteins.txt"))
                            {
                                var header = string.Format("{0}{11}{1}{11}{2}{11}{3}{11}{4}{11}{5}{11}{6}{11}{7}{11}{8}{11}{9}{11}{10}",
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
                                                            m_separator
                                                            );
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{11}\"{1}\"{11}{2}{11}{3}{11}{4}{11}{5}{11}{6}{11}{7}{11}{8}{11}{9}{11}{10}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : "",
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : "",
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetInt16(10) : 0,
                                                                m_separator);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                            cmd = new SqlCommand(massTagNullToProt, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempMassTagToProteins.txt", true))
                            {
                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{11}\"{1}\"{11}{2}{11}{3}{11}{4}{11}{5}{11}{6}{11}{7}{11}{8}{11}{9}{11}{10}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : "",
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : "",
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetInt16(10) : 0,
                                                                m_separator);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                            var analysisDescription = AnalysisDescriptionAccessDbQuery();

                            cmd = new SqlCommand(analysisDescription, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempAnalysisDescription.txt"))
                            {
                                var fieldNames = string.Format("{0}{30}{1}{30}{2}{30}{3}{30}{4}{30}{5}{30}{6}{30}{7}{30}{8}{30}{9}{30}{10}{30}{11}{30}{12}{30}{13}{30}{14}{30}{15}{30}{16}{30}{17}{30}{18}{30}{19}{30}{20}{30}{21}{30}{22}{30}{23}{30}{24}{30}{25}{30}{26}{30}{27}{30}{28}{30}{29}",
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
                                                            m_separator);
                                writer.WriteLine(fieldNames);
                                while (reader.Read())
                                {
                                    var line =
                                        string.Format(
                                            "{0}{30}{1}{30}{2}{30}{3}{30}{4}{30}{5}{30}{6}{30}{7}{30}{8}{30}{9}{30}{10}{30}{11}{30}{12}{30}{13}{30}{14}{30}{15}{30}{16}{30}\"{17}\"{30}\"{18}\"{30}{19}{30}{20}{30}{21}{30}{22}{30}{23}{30}{24}{30}{25}{30}{26}{30}{27}{30}{28}{30}\"{29}\"",
                                            !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : 0,
                                            !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : "",
                                            !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                            !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : "",
                                            !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : "",
                                            !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : "",
                                            !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : 0,
                                            !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : 0,
                                            !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : "",
                                            !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : "",
                                            !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetValue(10) : "",
                                            !Convert.IsDBNull(reader.GetValue(11)) ? reader.GetValue(11) : "",
                                            !Convert.IsDBNull(reader.GetValue(12)) ? reader.GetValue(12) : "",
                                            !Convert.IsDBNull(reader.GetValue(13)) ? reader.GetValue(13) : "",
                                            !Convert.IsDBNull(reader.GetValue(14)) ? reader.GetValue(14) : "",
                                            !Convert.IsDBNull(reader.GetValue(15)) ? reader.GetValue(15) : "",
                                            !Convert.IsDBNull(reader.GetValue(16)) ? reader.GetValue(16) : "",
                                            !Convert.IsDBNull(reader.GetValue(17)) ? reader.GetValue(17) : "",
                                            !Convert.IsDBNull(reader.GetValue(18)) ? reader.GetValue(18) : "",
                                            !Convert.IsDBNull(reader.GetValue(19)) ? reader.GetValue(19) : "",
                                            !Convert.IsDBNull(reader.GetValue(20)) ? reader.GetValue(20) : "",
                                            !Convert.IsDBNull(reader.GetValue(21)) ? reader.GetValue(21) : "",
                                            !Convert.IsDBNull(reader.GetValue(22)) ? reader.GetValue(22) : 0,
                                            !Convert.IsDBNull(reader.GetValue(23)) ? reader.GetValue(23) : 0,
                                            !Convert.IsDBNull(reader.GetValue(24)) ? reader.GetValue(24) : 0,
                                            !Convert.IsDBNull(reader.GetValue(25)) ? reader.GetValue(25) : 0,
                                            !Convert.IsDBNull(reader.GetValue(26)) ? reader.GetValue(26) : 0,
                                            !Convert.IsDBNull(reader.GetValue(27)) ? reader.GetValue(27) : 0,
                                            !Convert.IsDBNull(reader.GetValue(28)) ? reader.GetValue(28) : 0,
                                            !Convert.IsDBNull(reader.GetValue(29)) ? reader.GetValue(29) : "",
                                            m_separator
                                            );
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                            var filterSet = FilterSetOverviewAccessDbQuery();

                            cmd = new SqlCommand(filterSet, connection);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempFilterSet.txt"))
                            {
                                var header = string.Format("{0}{5}{1}{5}{2}{5}{3}{5}{4}",
                                                            "Filter_Type",
                                                            "Filter_Set_ID",
                                                            "Extra_Info",
                                                            "Filter_Set_Name",
                                                            "Filter_Set_Description",
                                                            m_separator
                                                            );
                                writer.WriteLine(header);

                                while (reader.Read())
                                {
                                    var line = string.Format("{0}{5}{1}{5}{2}{5}\"{3}\"{5}\"{4}\"",
                                                            !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : 0,
                                                            !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : 0,
                                                            !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : "",
                                                            !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : "",
                                                            !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : "",
                                                            m_separator);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        retryCount -= 1;
                        string msg = "Exception querying database in Export: " + ex.Message;
                        msg += ", RetryCount = " + retryCount;

                        Console.WriteLine(msg);

                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception connecting to database in Export: " + ex.Message + "; ConnectionString: " + m_connectionString;
                Console.WriteLine(msg);
            }
        }

        public Dictionary<string, AmtInfo> GetDatabases()
        {
            string databaseNameFilter = string.Empty;
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

                int retryCount = MaxRetries;
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
                                    Name = GetDBString(reader, 0),
                                    Description = GetDBString(reader, 1),
                                    Organism = GetDBString(reader, 2),
                                    Campaign = GetDBString(reader, 3),
                                    State = GetDBString(reader, 4),
                                    StateId = reader.GetInt32(5),
                                    Server = GetDBString(reader, 6)
                                };

                                mtdbDictionary.Add(amtInfo.Name, amtInfo);

                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        retryCount -= 1;
                        string msg = "Exception querying database in GetDatasets: " + ex.Message;
                        msg += ", RetryCount = " + retryCount;

                        Console.WriteLine(msg);

                        //Delay for 3 second before trying again
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception connecting to database in GetDatasets: " + ex.Message + "; ConnectionString: " + m_connectionString;
                Console.WriteLine(msg);
            }

            return mtdbDictionary;

        }

        public Dictionary<double, AmtPeptideOptions> GetStats(AmtInfo amt)//string datasetNameFilter)
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

                int retryCount = MaxRetries;
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
                                var peptideOptions = new AmtPeptideOptions();

                                peptideOptions.PmtQualityScore = reader.GetDecimal(0);
                                peptideOptions.MtCountPassing = reader.GetInt32(1);
                                peptideOptions.FilterSetId = (!Convert.IsDBNull(reader.GetValue(2)) ? reader.GetInt32(2) : 0);
                                peptideOptions.FilterSetName = GetDBString(reader, 3);
                                peptideOptions.FilterSetDescription = GetDBString(reader, 4);

                                statsDictionary.Add(Convert.ToDouble(peptideOptions.PmtQualityScore), peptideOptions);

                            }
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        retryCount -= 1;
                        string msg = "Exception querying database in GetStats: " + ex.Message;
                        msg += ", RetryCount = " + retryCount;

                        Console.WriteLine(msg);

                        //Delay for 3 second before trying again
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Exception connecting to database in GetStats: " + ex.Message + "; ConnectionString: " + m_connectionString;
                Console.WriteLine(msg);
            }

            return statsDictionary;

        }

        private string GetDBString(SqlDataReader reader, int columnIndex)
        {
            if (!Convert.IsDBNull(reader.GetValue(columnIndex)))
            {
                return reader.GetString(columnIndex);
            }

            return string.Empty;
        }

    }
}

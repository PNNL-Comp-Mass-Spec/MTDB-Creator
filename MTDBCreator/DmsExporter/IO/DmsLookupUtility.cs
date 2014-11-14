﻿using System.Data.SqlClient;
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

        private const string TemplateConnectionString = "Data Source=pogo;Initial Catalog=MT_Template;Integrated Security=SSPI";

        private const int MaxRetries = 3;

        #endregion

        private List<int> m_MTIds = new List<int>();

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

            //var massTags = "";
            //foreach (var massTagId in massTagIds)
            //{
            //    massTags += massTagId.ToString() + ", ";
            //}

            //massTags = massTags.Substring(0, massTags.Length - 2);

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
                        using (var cnDB = new SqlConnection(connectionString))
                        {
                            cnDB.Open();

                            var massTagSql = MassTagAccessDbQuery(selectedStats);

                            var row = 0;

                            var cmd = new SqlCommand(massTagSql, cnDB);
                            var reader = cmd.ExecuteReader();

                            //Write the mass tags to a temporary file
                            var modDescriptions = new List<string>();
                            var MTList = new List<int>();
                            using (var writer = new StreamWriter(directory + "tempMassTags.txt"))
                            {
                                var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
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
                                    var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},\"{11}\",{12},{13},\"{14}\",{15},{16},{17}",
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
                                                                !Convert.IsDBNull(reader.GetValue(17)) ? reader.GetValue(17) : 0);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                            var chargeSql = MassTagChargeDbQuery();
                            //var templateDB = new SqlConnection(TemplateConnectionString);
                            //templateDB.Open();
                            cmd = new SqlCommand(chargeSql, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempCharges.txt"))
                            {
                                var header = "Mass_Tag_ID,ChargeState";
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    if (m_MTIds.Contains(Convert.ToInt32(reader.GetValue(8))))
                                    {
                                        var line = string.Format("{0},{1}",
                                            !Convert.IsDBNull(reader.GetValue(8)) ? reader.GetValue(8) : 0,
                                            !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0);
                                        writer.WriteLine(line);
                                    }
                                    else
                                    {
                                        //Console.Write("Bwahahaha!" + reader.GetValue(8));
                                    }
                                }
                                reader.Close();
                            }

                            //Console.WriteLine(modDescriptions);
                            // TODO: Put the thing here
                            var massTagModsSql = ModAccessDbQuery();
                            //Console.WriteLine(massTagModsSql)
                            var mainDb = new SqlConnection(MainConnectionString);
                            mainDb.Open();
                            cmd = new SqlCommand(massTagModsSql, mainDb);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempModInfo.txt"))
                            {
                                var header = string.Format("Mod_tag, Mod_description, Mod_Mass, Mod_formula");
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0},{1},{2},{3}",
                                        !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : 0, // Tag here (NEEDED)
                                        !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0, // 
                                        !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0, // Mass is this one (NEEDED)
                                        !Convert.IsDBNull(reader.GetValue(9)) ? reader.GetValue(9) : 0); // Formula is this one (Maybe needed?)
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }
                            mainDb.Close();

                            var massTagNetSql = MassTagNetAccessDbQuery(selectedStats);
                            cmd = new SqlCommand(massTagNetSql, cnDB);
                            reader = cmd.ExecuteReader();

                            //Write the MassTagsNet to a temporary file

                            using (var writer = new StreamWriter(directory + "tempMassTagsNet.txt"))
                            {
                                var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                                            "Mass_Tag_ID",
                                                            "Min_GANET",
                                                            "Max_GANET",
                                                            "Avg_GANET",
                                                            "Cnt_GANET",
                                                            "StD_GANET",
                                                            "StdError_GANET",
                                                            "PNET"
                                                            );
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                                                !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(5)) ? reader.GetValue(5) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(6)) ? reader.GetValue(6) : 0,
                                                                !Convert.IsDBNull(reader.GetValue(7)) ? reader.GetValue(7) : 0);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }


                            var proteinsSql = ProteinsAccessDbQuery(selectedStats);
                            var proteinsNullCollectionSql = ProteinsNullCollectionAccessDbQuery(selectedStats);

                            cmd = new SqlCommand(proteinsSql, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempProteins.txt"))
                            {
                                var header = string.Format("{0},\"{1}\",\"{2}\",{3},{4},{5},{6},{7},{8},{9},{10}",
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
                                                            "External_Protein_ID"
                                                            );
                                writer.WriteLine(header);
                                while (reader.Read())
                                {
                                    var line = string.Format("{0},\"{1}\",\"{2}\",{3},{4},{5},{6},{7},{8},{9},{10}",
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
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetValue(10) : 0);
                                    writer.WriteLine(line);

                                }
                                reader.Close();
                            }
                            cmd = new SqlCommand(proteinsNullCollectionSql, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempProteins.txt", true))
                            {
                                while (reader.Read())
                                {
                                    var line = string.Format("{0},\"{1}\",\"{2}\",{3},{4},{5},{6},{7},{8},{9},{10}",
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
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetValue(10) : 0);
                                    writer.WriteLine(line);

                                }
                                reader.Close();
                            }


                            var massTagToProt = MassTagToProteinMapAccessDbQuery(selectedStats);
                            var massTagNullToProt = MassTagNullToProteinMapAccessDbQuery(selectedStats);

                            cmd = new SqlCommand(massTagToProt, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempMassTagToProteins.txt"))
                            {
                                var header = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
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
                                                            "Missed_Cleavage_Count"
                                                            );
                                writer.WriteLine(header);
                                var i = 0;
                                while (reader.Read())
                                {
                                    var line = string.Format("{0},\"{1}\",{2},{3},{4},{5},{6},{7},{8},{9},{10}",
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
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetInt16(10) : 0);
                                    writer.WriteLine(line);
                                    i++;
                                }
                                reader.Close();
                            }

                            cmd = new SqlCommand(massTagNullToProt, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempMassTagToProteins.txt", true))
                            {
                                while (reader.Read())
                                {
                                    var line = string.Format("{0},\"{1}\",{2},{3},{4},{5},{6},{7},{8},{9},{10}",
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
                                                                !Convert.IsDBNull(reader.GetValue(10)) ? reader.GetInt16(10) : 0);
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }


                            var analysisDescription = AnalysisDescriptionAccessDbQuery();

                            cmd = new SqlCommand(analysisDescription, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempAnalysisDescription.txt"))
                            {
                                var fieldNames = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29}",
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
                                                            "Regression_Equation");
                                writer.WriteLine(fieldNames);
                                row = 0;
                                while (reader.Read())
                                {
                                    if (row == 429)
                                    {
                                        Console.WriteLine("Breaks");
                                    }
                                    row++;
                                    var line =
                                        string.Format(
                                            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},\"{17}\",\"{18}\",{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},\"{29}\"",
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
                                            !Convert.IsDBNull(reader.GetValue(29)) ? reader.GetValue(29) : ""
                                            );
                                    writer.WriteLine(line);
                                }
                                reader.Close();
                            }

                            var filterSet = FilterSetOverviewAccessDbQuery();

                            cmd = new SqlCommand(filterSet, cnDB);
                            reader = cmd.ExecuteReader();

                            using (var writer = new StreamWriter(directory + "tempFilterSet.txt"))
                            {
                                var header = string.Format("{0},{1},{2},{3},{4}",
                                                            "Filter_Type",
                                                            "Filter_Set_ID",
                                                            "Extra_Info",
                                                            "Filter_Set_Name",
                                                            "Filter_Set_Description"
                                                            );
                                writer.WriteLine(header);

                                while (reader.Read())
                                {
                                    var line = string.Format("{0},{1},{2},\"{3}\",\"{4}\"",
                                                            !Convert.IsDBNull(reader.GetValue(0)) ? reader.GetValue(0) : 0,
                                                            !Convert.IsDBNull(reader.GetValue(1)) ? reader.GetValue(1) : 0,
                                                            !Convert.IsDBNull(reader.GetValue(2)) ? reader.GetValue(2) : "",
                                                            !Convert.IsDBNull(reader.GetValue(3)) ? reader.GetValue(3) : "",
                                                            !Convert.IsDBNull(reader.GetValue(4)) ? reader.GetValue(4) : "");
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

                        //Delay for 3 second before trying again
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
                        using (var cnDB = new SqlConnection(m_connectionString))
                        {
                            cnDB.Open();

                            var cmd = new SqlCommand(sql, cnDB);
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
                        using (var cnDB = new SqlConnection(connectionString))
                        {
                            cnDB.Open();

                            var cmd = new SqlCommand(sql, cnDB);
                            var reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                var peptideOptions = new AmtPeptideOptions();

                                peptideOptions.PmtQualityScore = reader.GetDecimal(0); //reader.GetDouble(0);
                                peptideOptions.MtCountPassing = reader.GetInt32(1); //reader.GetInt32(1);
                                peptideOptions.FilterSetId = (!Convert.IsDBNull(reader.GetValue(2)) ? reader.GetInt32(2) : 0);//GetDBString(reader, 2); //reader.GetInt32(2);
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
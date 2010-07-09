using System;
using System.Data;
using System.Data.OleDb;
using System.Collections; 

using Microsoft.Office.Interop.Access;
using MassTagDatabaseStoredQueryCreator;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsMTDB.
	/// </summary>
	public class clsMTDB: ProcessorBase, IDisposable 
	{

        #region Static Column Names
        private static string m_TPeptidesColName_Peptide_Id = "Peptide_Id";
        private static string m_TPeptidesColName_Analysis_Id = "Analysis_Id";
        private static string m_TPeptidesColName_Scan_Number = "Scan_Number";
        private static string m_TPeptidesColName_Number_Of_Scans = "Number_Of_Scans";
        private static string m_TPeptidesColName_Charge_State = "Charge_State";
        private static string m_TPeptidesColName_MH = "MH";
        private static string m_TPeptidesColName_Multiple_Proteins = "Multiple_Proteins";
        private static string m_TPeptidesColName_Peptide = "Peptide";
        private static string m_TPeptidesColName_Mass_Tag_ID = "Mass_Tag_ID";
        private static string m_TPeptidesColName_GANET_Obs = "GANET_Obs";
        private static string m_TPeptidesColName_Scan_Time_Peak_Apex = "Scan_Time_Peak_Apex";
        private static string m_TPeptidesColName_Peak_Area = "Peak_Area";
        private static string m_TPeptidesColName_Peak_SN_Ratio = "Peak_SN_Ratio";

        private static string m_TMassTagsColName_Mass_Tag_ID = "Mass_Tag_ID";
        private static string m_TMassTagsColName_Peptide = "Peptide";
        private static string m_TMassTagsColName_Monoisotopic_Mass = "Monoisotopic_Mass";
        private static string m_TMassTagsColName_Multiple_Proteins = "Multiple_Proteins";
        private static string m_TMassTagsColName_Created = "Created";
        private static string m_TMassTagsColName_Last_Affected = "Last_Affected";
        private static string m_TMassTagsColName_Number_Of_Peptides = "Number_Of_Peptides";
        private static string m_TMassTagsColName_Peptide_Obs_Count_Passing_Filter = "Peptide_Obs_Count_Passing_Filter";
        private static string m_TMassTagsColName_High_Normalized_Score = "High_Normalized_Score";
        private static string m_TMassTagsColName_High_Peptide_Prophet_Probability = "High_Peptide_Prophet_Probability";
        private static string m_TMassTagsColName_Min_Log_EValue = "Min_Log_EValue";
        private static string m_TMassTagsColName_Mod_Count = "Mod_Count";
        private static string m_TMassTagsColName_Mod_Description = "Mod_Description";
        private static string m_TMassTagsColName_PMT_Quality_Score = "PMT_Quality_Score";        
        private static string m_TMassTagsColName_External_Protein_ID            = "External_Protein_ID";
        private static string m_TMassTagsColName_External_Reference_ID          = "External_Reference_ID";
        private static string m_TMassTagsColName_PepProphet_FScore_Max_CS1      = "PepProphet_FScore_Max_CS1";
        private static string m_TMassTagsColName_PepProphet_FScore_Max_CS2      = "PepProphet_FScore_Max_CS2";
        private static string m_TMassTagsColName_PepProphet_FScore_Max_CS3      = "PepProphet_FScore_Max_CS3";
        private static string m_TMassTagsColName_PepProphet_Probability_Max_CS1 = "PepProphet_Probability_Max_CS1";
        private static string m_TMassTagsColName_PepProphet_Probability_Max_CS2 = "PepProphet_Probability_Max_CS2";
        private static string m_TMassTagsColName_PepProphet_Probability_Max_CS3 = "PepProphet_Probability_Max_CS3";
        private static string m_TMassTagsColName_Is_confirmed                   = "Is_Confirmed";
        private static string m_TMassTagsColName_High_Dicriminant_Score         = "High_Discriminant_Score";
        
        

        private static string m_TProteinsColName_Ref_ID = "Ref_ID";
        private static string m_TProteinsColName_Reference = "Reference";
        private static string m_TProteinsColName_Description = "Description";
        private static string m_TProteinsColName_Protein_Sequence = "Protein_Sequence";
        private static string m_TProteinsColName_Protein_Residue_Count = "Protein_Residue_Count";
        private static string m_TProteinsColName_Monoisotopic_Mass = "Monoisotopic_Mass";
        private static string m_TProteinsColName_Protein_Collection_ID = "Protein_Collection_ID";
        private static string m_TProteinsColName_Last_Affected = "Last_Affected";
        private static string m_TProteinsColName_External_Reference_ID = "External_Reference_ID";
        private static string m_TProteinsColName_Protein_DB_ID = "Protein_DB_ID";
        private static string m_TProteinsColName_External_Protein_ID = "External_Protein_ID";

        private static string m_TMassTagsToProteinMapColName_Mass_Tag_ID = "Mass_Tag_ID";
        private static string m_TMassTagsToProteinMapColName_Mass_Tag_Name = "Mass_Tag_Name";
        private static string m_TMassTagsToProteinMapColName_Ref_ID = "Ref_ID";
        private static string m_TMassTagsToProteinMapColName_Cleavage_State = "Cleavage_State";
        private static string m_TMassTagsToProteinMapColName_Fragment_Number = "Fragment_Number";
        private static string m_TMassTagsToProteinMapColName_Fragment_Span = "Fragment_Span";
        private static string m_TMassTagsToProteinMapColName_Residue_Start = "Residue_Start";
        private static string m_TMassTagsToProteinMapColName_Residue_End = "Residue_End";
        private static string m_TMassTagsToProteinMapColName_Repeat_Count = "Repeat_Count";
        private static string m_TMassTagsToProteinMapColName_Terminus_State = "Terminus_State";
        private static string m_TMassTagsToProteinMapColName_Missed_Cleavage_Count = "Missed_Cleavage_Count";


        private static string m_TScoreXTandemColName_Peptide_ID = "Peptide_ID";
        private static string m_TScoreXTandemColName_Hyperscore = "Hyperscore";
        private static string m_TScoreXTandemColName_Log_EValue = "Log_EValue";
        private static string m_TScoreXTandemColName_DeltaCn2 = "DeltaCn2";
        private static string m_TScoreXTandemColName_Y_Score = "Y_Score";
        private static string m_TScoreXTandemColName_Y_Ions = "Y_Ions";
        private static string m_TScoreXTandemColName_B_Score = "B_Score";
        private static string m_TScoreXTandemColName_B_Ions = "B_Ions";
        private static string m_TScoreXTandemColName_DelM = "DelM";
        private static string m_TScoreXTandemColName_Intensity = "Intensity";
        private static string m_TScoreXTandemColName_Normalized_Score = "Normalized_Score";

        private static string m_TScoreSequestColName_Peptide_ID = "Peptide_ID";
        private static string m_TScoreSequestColName_XCorr = "XCorr";
        private static string m_TScoreSequestColName_DeltaCn = "DeltaCn";
        private static string m_TScoreSequestColName_DeltaCn2 = "DeltaCn2";
        private static string m_TScoreSequestColName_Sp = "Sp";
        private static string m_TScoreSequestColName_RankSp = "RankSp";
        private static string m_TScoreSequestColName_RankXc = "RankXc";
        private static string m_TScoreSequestColName_DelM = "DelM";
        private static string m_TScoreSequestColName_XcRatio = "XcRatio";

        private static string m_TMassTagsNETColName_Mass_Tag_ID = "Mass_Tag_ID";
        private static string m_TMassTagsNETColName_Min_GANET = "Min_GANET";
        private static string m_TMassTagsNETColName_Max_GANET = "Max_GANET";
        private static string m_TMassTagsNETColName_Avg_GANET = "Avg_GANET";
        private static string m_TMassTagsNETColName_Cnt_GANET = "Cnt_GANET";
        private static string m_TMassTagsNETColName_StD_GANET = "StD_GANET";
        private static string m_TMassTagsNETColName_StdError_GANET = "StdError_GANET";
        private static string m_TMassTagsNETColName_PNET = "PNET";


        private static string m_TAnalysisDescriptionColName_Job = "Job";
        private static string m_TAnalysisDescriptionColName_Dataset = "Dataset";
        private static string m_TAnalysisDescriptionColName_Dataset_ID = "Dataset_ID";
        private static string m_TAnalysisDescriptionColName_Dataset_Created_DMS = "Dataset_Created_DMS";
        private static string m_TAnalysisDescriptionColName_Dataset_Acq_Time_Start = "Dataset_Acq_Time_Start";
        private static string m_TAnalysisDescriptionColName_Dataset_Acq_Time_End = "Dataset_Acq_Time_End";
        private static string m_TAnalysisDescriptionColName_Dataset_Scan_Count = "Dataset_Scan_Count";
        private static string m_TAnalysisDescriptionColName_Experiment = "Experiment";
        private static string m_TAnalysisDescriptionColName_Campaign = "Campaign";
        private static string m_TAnalysisDescriptionColName_Organism = "Organism";
        private static string m_TAnalysisDescriptionColName_Instrument_Class = "Instrument_Class";
        private static string m_TAnalysisDescriptionColName_Instrument = "Instrument";
        private static string m_TAnalysisDescriptionColName_Analysis_Tool = "Analysis_Tool";
        private static string m_TAnalysisDescriptionColName_Parameter_File_Name = "Parameter_File_Name";
        private static string m_TAnalysisDescriptionColName_Settings_File_Name = "Settings_File_Name";
        private static string m_TAnalysisDescriptionColName_Organism_DB_Name = "Organism_DB_Name";
        private static string m_TAnalysisDescriptionColName_Protein_Collection_List = "Protein_Collection_List";
        private static string m_TAnalysisDescriptionColName_Protein_Options_List = "Protein_Options_List";
        private static string m_TAnalysisDescriptionColName_Completed = "Completed";
        private static string m_TAnalysisDescriptionColName_ResultType = "ResultType";
        private static string m_TAnalysisDescriptionColName_Separation_Sys_Type = "Separation_Sys_Type";
        private static string m_TAnalysisDescriptionColName_ScanTime_NET_Slope = "ScanTime_NET_Slope";
        private static string m_TAnalysisDescriptionColName_ScanTime_NET_Intercept = "ScanTime_NET_Intercept";
        private static string m_TAnalysisDescriptionColName_ScanTime_NET_RSquared = "ScanTime_NET_RSquared";
        private static string m_TAnalysisDescriptionColName_ScanTime_NET_Fit = "ScanTime_NET_Fit";


        // T_Mass_Tag_Peptide_Prophet_Stats data structures:
        // Mass_Tag_ID, ObsCount_CS1, ObsCount_CS2, ObsCount_CS3, 
        // PepProphet_FScore_Max_CS1, PepProphet_FScore_Max_CS2, PepProphet_FScore_Max_CS3,
        // PepProphet_Probability_Max_CS1, PepProphet_Probability_Max_CS2, PepProphet_Probability_Max_CS3
        // PepProphet_Probability_Avg_CS1, PepProphet_Probability_Avg_CS2, PepProphet_Probability_Avg_CS3
        private static string m_TMassTagPeptideProphetStatsColName_Mass_Tag_ID = "Mass_Tag_ID";
        private static string m_TMassTagPeptideProphetStatsColName_ObsCount_CS1 = "ObsCount_CS1";
        private static string m_TMassTagPeptideProphetStatsColName_ObsCount_CS2 = "ObsCount_CS2";
        private static string m_TMassTagPeptideProphetStatsColName_ObsCount_CS3 = "ObsCount_CS3";
        // maxs will not be printed for now. 
        private static string m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS1 = "PepProphet_Probability_Avg_CS1";
        private static string m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS2 = "PepProphet_Probability_Avg_CS2";
        private static string m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS3 = "PepProphet_Probability_Avg_CS3";
        private static string m_TMassTagPeptideProphetStatsColName_PepProphet_FScore_Avg_CS1 = "PepProphet_FScore_Avg_CS1";
        private static string m_TMassTagPeptideProphetStatsColName_PepProphet_FScore_Avg_CS2 = "PepProphet_FScore_Avg_CS2";
        private static string m_TMassTagPeptideProphetStatsColName_PepProphet_FScore_Avg_CS3 = "PepProphet_FScore_Avg_CS3";
        private static string m_TAnalysisDescriptionTable = "T_Analysis_Description";
        private static string m_TPeptidesTable = "T_Peptides";
        private static string m_TMassTagsTable = "T_Mass_Tags";
        private static string m_TMassTagsNETTable = "T_Mass_Tags_NET";
        private static string m_TMassTagsToProteinMapTable = "T_Mass_Tag_to_Protein_Map";
        private static string m_TProteinsTable = "T_Proteins";
        private static string m_TScoreXTandemTable = "T_Score_XTandem";
        private static string m_TScoreSequestTable = "T_Score_Sequest";
        private static string m_TMassTagPeptideProphetStatsTable = "T_Mass_Tag_Peptide_Prophet_Stats"; 
        #endregion

		public const float MISSING_F_SCORE = -100 ; 
		private enum ACTION { IDLE=0, ALIGNING, ERROR } ;

		private ACTION menmAction ; 
		/// <summary>
        /// hashtable that goes from sequence to index. 
		/// </summary> 
		private Hashtable mhashMassTags ; 
		/// <summary>
        /// hashtable that goes from protein name to protein index 
		/// </summary>
		private Hashtable mhashProteins ; 
		/// <summary>
        /// hashtable that goes from Protein Reference Id_Peptide 
		/// </summary> 
		private Hashtable mhashMassTagsToProteinMap ;
		private ArrayList m_massTags ; 
		private ArrayList marrProteins ; 
		private Hashtable mhashCurrentJobUniqIdToMassTagId ; 
		private ArrayList m_massTagsInJobs ; 
		private ArrayList marrAnalyses ; 

		private Hashtable mhashProteinsPassingConstraints = new Hashtable() ; 		
		private bool mblnSequestFilesExist  = false ; 
		private bool mblnXTandemFilesExist  = false;
        private string mstrDelim            = "," ; 
		private bool mblnTPeptideHeaderWritten = false ; 
		private bool mblnTScoreSequestFileNameHeaderWritten = false ; 
		private bool mblnTScoreXTandemFileNameHeaderWritten = false ; 
		private string m_TAnalysisDescriptionFileName ; 
		private string m_TPeptideFileName ;
		private string m_TMassTagsFileName ;
		private string m_TMassTagsNETFileName ;
		private string m_TMassTagPeptideProphetStatsFileName ;
		private string m_TMassTagsToProteinMapFileName ;
		private string m_TProteinsFileName ;
		private string m_TScoreXTandemFileName ;
		private string m_TScoreSequestFileName ;
		private string mstrAccessDBPath = "" ; 
		private int mintNumPeptides = 0 ;


		private NETPredictionBasic.iPeptideElutionTime mobjPredictionKrokhin; 
		private NETPrediction.iPeptideElutionTime      mobjPredictionKangas; 

        private Regressor.clsRegressor mobjRegressor ; 
		private clsOptions mobjOptions ; 
		
		public clsMTDB(clsOptions options)
		{
			menmAction          = ACTION.IDLE ; 			
			mhashMassTags       = new Hashtable() ; 
			mhashProteins       = new Hashtable() ;
			mobjRegressor       = new Regressor.clsRegressor() ; 
			m_massTags        = new ArrayList() ; 
			marrProteins        = new ArrayList() ;
			m_massTagsInJobs  = new ArrayList() ; 
			marrAnalyses        = new ArrayList() ; 
			Options             = options ; 
            mhashMassTagsToProteinMap        = new Hashtable();
            mhashCurrentJobUniqIdToMassTagId = new Hashtable(); 


//			string executablePath = System.Windows.Forms.Application.ExecutablePath ;
//			string executableFolder = executablePath.Substring(0, 
//				executablePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar)) ; 

			//System.Reflection.Assembly netPredictionAssembly ;

			mobjPredictionKrokhin = new NETPredictionBasic.ElutionTimePredictionKrokhin() ; 
			mobjPredictionKangas  = new NETPrediction.ElutionTimePredictionKangas() ; 

			DateTime now = DateTime.Now ; 

			string ext = ".txt" ;
			if (mstrDelim == ",")
				ext = ".csv" ; 
			m_TAnalysisDescriptionFileName    =  m_TAnalysisDescriptionTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext ; 
			m_TPeptideFileName                =  m_TPeptidesTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext ; 
			m_TMassTagsFileName               =  m_TMassTagsTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds)  + ext ; 
			m_TMassTagsNETFileName            =  m_TMassTagsNETTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			
            // filenames are too long. Access only allows 64 characters!!GRRR!!.
			//m_TMassTagPeptideProphetStatsFileName =  m_TMassTagPeptideProphetStatsTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			m_TMassTagPeptideProphetStatsFileName =  "TMTPP_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			
            m_TMassTagsToProteinMapFileName   =  m_TMassTagsToProteinMapTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			m_TProteinsFileName               =  m_TProteinsTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext ; 
			m_TScoreXTandemFileName           =  m_TScoreXTandemTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			m_TScoreSequestFileName           =  m_TScoreSequestTable +  "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 

			m_TAnalysisDescriptionFileName        = m_TAnalysisDescriptionFileName.Replace('/', '_') ;
			m_TPeptideFileName                    = m_TPeptideFileName.Replace('/','_'); 
			m_TMassTagsFileName                   = m_TMassTagsFileName.Replace('/','_'); 
			m_TMassTagsNETFileName                = m_TMassTagsNETFileName.Replace('/','_'); 
			m_TMassTagPeptideProphetStatsFileName = m_TMassTagPeptideProphetStatsFileName.Replace('/','_'); 
			m_TMassTagsToProteinMapFileName       = m_TMassTagsToProteinMapFileName.Replace('/','_'); 
			m_TProteinsFileName                   = m_TProteinsFileName.Replace('/','_'); 
			m_TScoreXTandemFileName               = m_TScoreXTandemFileName.Replace('/','_'); 
			m_TScoreSequestFileName               = m_TScoreSequestFileName.Replace('/','_'); 

			m_TAnalysisDescriptionFileName    = m_TAnalysisDescriptionFileName.Replace(':', '_') ;
			m_TPeptideFileName                = m_TPeptideFileName.Replace(':','_'); 
			m_TMassTagsFileName               = m_TMassTagsFileName.Replace(':','_'); 
			m_TMassTagsNETFileName            = m_TMassTagsNETFileName.Replace(':','_'); 
			m_TMassTagPeptideProphetStatsFileName = m_TMassTagPeptideProphetStatsFileName.Replace(':','_'); 
			m_TMassTagsToProteinMapFileName       = m_TMassTagsToProteinMapFileName.Replace(':','_'); 
			m_TProteinsFileName                   = m_TProteinsFileName.Replace(':','_'); 
			m_TScoreXTandemFileName               = m_TScoreXTandemFileName.Replace(':','_'); 
			m_TScoreSequestFileName               = m_TScoreSequestFileName.Replace(':','_'); 

			m_TAnalysisDescriptionFileName        = m_TAnalysisDescriptionFileName.Replace(' ', '_') ;
			m_TPeptideFileName                    = m_TPeptideFileName.Replace(' ','_'); 
			m_TMassTagsFileName                   = m_TMassTagsFileName.Replace(' ','_'); 
			m_TMassTagsNETFileName                = m_TMassTagsNETFileName.Replace(' ','_'); 
			m_TMassTagPeptideProphetStatsFileName = m_TMassTagPeptideProphetStatsFileName.Replace(' ','_'); 
			m_TMassTagsToProteinMapFileName       = m_TMassTagsToProteinMapFileName.Replace(' ','_'); 
			m_TProteinsFileName                   = m_TProteinsFileName.Replace(' ','_'); 
			m_TScoreXTandemFileName               = m_TScoreXTandemFileName.Replace(' ','_'); 
			m_TScoreSequestFileName               = m_TScoreSequestFileName.Replace(' ','_'); 

			string path = System.IO.Path.GetTempPath() ; 
			m_TAnalysisDescriptionFileName        = path + m_TAnalysisDescriptionFileName  ;
			m_TPeptideFileName                    = path + m_TPeptideFileName  ; 
			m_TMassTagsFileName                   = path + m_TMassTagsFileName  ; 
			m_TMassTagsNETFileName                = path + m_TMassTagsNETFileName  ; 
			m_TMassTagPeptideProphetStatsFileName = path + m_TMassTagPeptideProphetStatsFileName ; 
			m_TMassTagsToProteinMapFileName       = path + m_TMassTagsToProteinMapFileName  ; 
			m_TProteinsFileName                   = path + m_TProteinsFileName  ; 
			m_TScoreXTandemFileName               = path + m_TScoreXTandemFileName  ; 
			m_TScoreSequestFileName               = path + m_TScoreSequestFileName  ;
        }

        #region Properties

        public string AccessDBPath
        {
            set
            {
                mstrAccessDBPath = value;
            }
        }
        public clsOptions Options
        {
            get
            {
                return mobjOptions;
            }
            set
            {
                mobjOptions = value;
                mobjRegressor.RegressionOrder = mobjOptions.RegressionOrder;
            }
        }

        #endregion

        #region methods
        private void DeleteFile(string strPath)
		{
			try
			{
				if (System.IO.File.Exists(strPath))
					System.IO.File.Delete(strPath) ; 
			}
			catch (Exception ex)
			{
                ErrorMessage("Could not delete the file " + strPath + " " + ex.Message);
			}
		}
		
		
		private void LoadCurrentMassTags()
		{
			if (mstrAccessDBPath == null)
				return ; 
			try
			{
				System.IO.FileInfo fInfo = new System.IO.FileInfo(mstrAccessDBPath) ; 
				if (!fInfo.Exists)
					return ;
			}
			catch (Exception ex)
			{
                ErrorMessage("Could not load the mass tags stored in the access path provided. " + ex.Message);
			}
		}
        /// <summary>
        /// Writes the stored results to file.
        /// </summary>
        /// <param name="writeToAccessDB">When true, writes to an Access MTDB.  When False, writes to temporary text files so the user can create a MTDB manually.</param>
		public void LoadResultsIntoDB(bool writeToAccessDB, string databasePath)
		{

			WriteMassTagsToFile() ; 
			WriteProteinsToFile() ; 
			WriteMassTagsToProteinMapToFile() ; 
			WriteMassTagsNETToFile() ; 
			if (mblnSequestFilesExist)
				WriteMassTagsPeptideProphetStatsToFile() ; 
			WriteAnalysisDescriptionToFile() ; 

			if (!writeToAccessDB) 
			{
                string message = string.Format("You must now manually import these files into your Access DB: {0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}",
				                                m_TPeptideFileName,
                                                m_TMassTagsFileName,
                                                m_TMassTagsNETFileName,
                                                m_TMassTagsToProteinMapFileName,
                                                m_TProteinsFileName);

				if (mblnXTandemFilesExist)
					message += "\r\n" + m_TScoreXTandemFileName;

				if (mblnSequestFilesExist)
				    message += "\r\n" + m_TScoreSequestFileName + "\r\n" + m_TMassTagPeptideProphetStatsFileName;

				StatusMessage(message) ; 				
			}
			else 
			{
                StatusMessage("Loading results into Microsoft Access DB") ; 
			    ApplicationClass access = new ApplicationClass() ;
                DAO.Database  database   = null;
                DAO._DBEngine engine     = null;
                bool error              = false;
			    
			    try
			    {
				    access.NewCurrentDatabase(databasePath);
                    
				    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TPeptidesTable,                m_TPeptideFileName,               true, null, null) ; 
				    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TMassTagsTable,                m_TMassTagsFileName,              true, null, null) ; 
				    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TMassTagsNETTable,             m_TMassTagsNETFileName,           true, null, null) ; 
				    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TMassTagsToProteinMapTable,    m_TMassTagsToProteinMapFileName,  true, null, null) ; 
				    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TProteinsTable,                m_TProteinsFileName,              true, null, null) ;

                    if (mblnXTandemFilesExist)
                    {
                        access.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", m_TScoreXTandemTable, m_TScoreXTandemFileName, true, null, null);
                    }
				    if (mblnSequestFilesExist)
				    {
					    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TScoreSequestTable,                m_TScoreSequestFileName,               true, null, null) ; 
					    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TMassTagPeptideProphetStatsTable,  m_TMassTagPeptideProphetStatsFileName, true, null, null) ; 
				    }    			
				    access.DoCmd.TransferText(AcTextTransferType.acImportDelim, "", m_TAnalysisDescriptionTable, m_TAnalysisDescriptionFileName, true, null, null) ; 

                    
				    DAO.Field    field;
				    DAO.Relation relation;
                    database    = access.CurrentDb();
                    engine      = access.DBEngine;
                    
                    // I dont know if we need relationships?

                    
				    if (mblnXTandemFilesExist)
				    {
					    relation          = database.CreateRelation("PeptideIDRelationship", m_TPeptidesTable, m_TScoreXTandemTable, DAO.RelationAttributeEnum.dbRelationDontEnforce) ; 
					    field             = relation.CreateField(m_TPeptidesColName_Peptide_Id, DAO.DataTypeEnum.dbInteger, 0) ; 
					    field.ForeignName = m_TScoreXTandemColName_Peptide_ID ; 
					    relation.Fields.Append(field) ;
                        database.Relations.Append(relation); 
				    }
				    if (mblnSequestFilesExist)
				    {
					    relation          = database.CreateRelation("PeptideIDRelationship", m_TPeptidesTable, m_TScoreSequestTable, DAO.RelationAttributeEnum.dbRelationDontEnforce) ; 
					    field             = relation.CreateField(m_TPeptidesColName_Peptide_Id,DAO.DataTypeEnum.dbInteger, 0) ; 
					    field.ForeignName = m_TScoreSequestColName_Peptide_ID ; 
					    relation.Fields.Append(field) ;
                        database.Relations.Append(relation); 
				    }

				    relation              = database.CreateRelation("MassTagdIDRelationshipTPepTmass", m_TMassTagsTable, m_TPeptidesTable, DAO.RelationAttributeEnum.dbRelationDontEnforce) ; 
				    field                 = relation.CreateField(m_TMassTagsColName_Mass_Tag_ID,DAO.DataTypeEnum.dbInteger, 0) ; 
				    field.ForeignName     = m_TPeptidesColName_Mass_Tag_ID ; 
				    relation.Fields.Append(field) ;
                    database.Relations.Append(relation); 

				    relation              = database.CreateRelation("MassTagdIDRelationshipTmassTmassNet", m_TMassTagsTable, m_TMassTagsNETTable, DAO.RelationAttributeEnum.dbRelationDontEnforce) ; 
				    field                 = relation.CreateField(m_TMassTagsColName_Mass_Tag_ID,DAO.DataTypeEnum.dbInteger, 0) ; 
				    field.ForeignName     = m_TMassTagsNETColName_Mass_Tag_ID; 
				    relation.Fields.Append(field) ;
                    database.Relations.Append(relation); 

				    relation              = database.CreateRelation("MassTagdIDRelationshipTmassTmasstagtoprotein", m_TMassTagsTable, m_TMassTagsToProteinMapTable, DAO.RelationAttributeEnum.dbRelationDontEnforce) ; 
				    field                 = relation.CreateField(m_TMassTagsColName_Mass_Tag_ID,DAO.DataTypeEnum.dbInteger, 0) ; 
				    field.ForeignName     = m_TMassTagsToProteinMapColName_Mass_Tag_ID ; 
				    relation.Fields.Append(field) ;
                    database.Relations.Append(relation); 

				    relation              = database.CreateRelation("ProteinIDRelationshipTproteinTmasstagtoprotein", m_TProteinsTable,m_TMassTagsToProteinMapTable,  DAO.RelationAttributeEnum.dbRelationDontEnforce) ; 
				    field                 = relation.CreateField(m_TProteinsColName_Ref_ID,DAO.DataTypeEnum.dbInteger, 0) ; 
				    field.ForeignName     = m_TMassTagsToProteinMapColName_Ref_ID; 
				    relation.Fields.Append(field) ;
                    database.Relations.Append(relation);

                    relation              = database.CreateRelation("AnalysisIDRelationship", m_TAnalysisDescriptionTable, m_TPeptidesTable, DAO.RelationAttributeEnum.dbRelationDontEnforce); 
				    field                 = relation.CreateField(m_TAnalysisDescriptionColName_Job, DAO.DataTypeEnum.dbInteger, 0) ; 
				    field.ForeignName     = m_TPeptidesColName_Analysis_Id ; 
				    relation.Fields.Append(field) ; 
				    database.Relations.Append(relation) ; 
                     
			    }
                catch(Exception ex)
                {
                    error = true;
                    ErrorMessage("Failed to save the MTDB in access format. " + ex.Message);
                }
			    finally
                {
                    // Release the underlying engine COM object that would otherwise leave a reference to the database and lock the db file.
                    // http://support.microsoft.com/kb/317113
                    if (database != null)
                    {
                        database.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(database);                        
                    }
                    // Release the underlying engine COM object that would otherwise leave a reference to the database and lock the db file.
                    // http://support.microsoft.com/kb/317113
                    if (engine != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(engine);                        
                    }
                    // Close access if necessary?
                    if (access != null)
                    {
                        try
                        {
                            access.CloseCurrentDatabase();

                            // Release the underlying engine COM object that would otherwise leave a reference to the database and lock the db file.
                            // http://support.microsoft.com/kb/317113
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(access);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage(ex.Message);
                            // Do nothing.  It may fail to close if the database failed to be created.
                        }
                    }

                    database = null;
                    engine   = null;
				    access   = null; 
			    }

                if (!error)
                {
                    CreateStoredQueries(databasePath);
                }
			}
        }
        /// <summary>
        /// Adds stored queries to the access database provided
        /// </summary>
        /// <param name="databasePath"></param>
        public void CreateStoredQueries(string databasePath)
        {
            if (!System.IO.File.Exists(databasePath))
            {
                ErrorMessage("Could not find the database path provided");
                return;
            }

            StatusMessage("Updating mass tag database with stored queries for use with peak matching tools.");
            // The queries are stored in files in a resources file.  Here we access them and then
            // start the process of modifying an existing MTDB.
            string massTagMatchCount                =  global::MTDBCreator.StoredQueries.GetMassTagMatchCount;
            string massTagsPassingFiltersWork       =  global::MTDBCreator.StoredQueries.GetMassTagsPassingFiltersWork;
            string massTagsPlusPeptideProphetStats  =  global::MTDBCreator.StoredQueries.GetMassTagsPlusPepProphetStats;
            string massTagToProteinNameMap          =  global::MTDBCreator.StoredQueries.GetMassTagToProteinNameMap;
            string viewMassTagToProteinNamemap      =  global::MTDBCreator.StoredQueries.V_IFC_Mass_Tag_to_Protein_Name_Map;

            clsMassTagDBPeptideStoredQueryCreator creator = new clsMassTagDBPeptideStoredQueryCreator(AccessType.Access2007, databasePath);
            creator.CreateGetMassTagToProteinNameMap(massTagToProteinNameMap, viewMassTagToProteinNamemap);
            try
            {
                creator.CreateGetMassTagsPlusPepProphetStats(massTagsPlusPeptideProphetStats, massTagsPassingFiltersWork);
            }
            catch
            {
                //TODO: May not work if sequest files do not exist....
            }
            creator.CreateGetMassTagsMatchCount(massTagMatchCount);
        }
        #endregion

        #region Writing Methods
        private void WritePeptidesToFile(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numResults = results.marrXTandemResults.Length ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TPeptideFileName, true)) 
			{
				if (!mblnTPeptideHeaderWritten)
				{
					mblnTPeptideHeaderWritten = true ; 
					// Headers first.
					sw.Write(m_TPeptidesColName_Peptide_Id);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Analysis_Id);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Scan_Number);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Number_Of_Scans);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Charge_State);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_MH);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Multiple_Proteins);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Peptide);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Mass_Tag_ID);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_GANET_Obs);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Scan_Time_Peak_Apex);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Peak_Area);
					sw.Write(mstrDelim) ; 
					sw.WriteLine(m_TPeptidesColName_Peak_SN_Ratio);
				}
				
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					int percentDone = (resultNum*100)/numResults ; 
					PercentComplete(percentDone) ; 
					clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 

					if (!mobjOptions.IsToBeExported(xtResult))
						continue ; 

					clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 
					int massTagId = (int) mhashCurrentJobUniqIdToMassTagId[resultToSeq.mint_unique_seq_id] ; 

					sw.Write(mintNumPeptides+resultNum+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mintDMSJobNum+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mint_scan) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mshort_charge) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_peptide_mh) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mshort_multi_protein_count) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mstr_peptide_sequence) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTagId+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_observed_net) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mint_scan) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(Math.Exp(xtResult.mdbl_log_intensity)) ; 
					sw.Write(mstrDelim) ; 
					// should be writing in correct apex.
					sw.WriteLine(3) ; 
				}
				sw.Close() ; 
			}

		}
		private void WritePeptidesToFile(clsSequestAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numResults = results.marrSequestResults.Length ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TPeptideFileName, true)) 
			{
				if (!mblnTPeptideHeaderWritten)
				{
					mblnTPeptideHeaderWritten = true ; 
					// Headers first.
					sw.Write(m_TPeptidesColName_Peptide_Id);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Analysis_Id);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Scan_Number);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Number_Of_Scans);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Charge_State);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_MH);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Multiple_Proteins);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Peptide);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Mass_Tag_ID);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_GANET_Obs);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Scan_Time_Peak_Apex);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TPeptidesColName_Peak_Area);
					sw.Write(mstrDelim) ; 
					sw.WriteLine(m_TPeptidesColName_Peak_SN_Ratio);
				}
				
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					int percentDone = (resultNum*100)/numResults ; 
					PercentComplete(percentDone) ; 
					clsSequestResults seqResult = results.marrSequestResults[resultNum] ; 

					if (!mobjOptions.IsToBeExported(seqResult)) 
						continue ; 

					clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 
					int massTagId = (int) mhashCurrentJobUniqIdToMassTagId[resultToSeq.mint_unique_seq_id] ; 

					sw.Write(mintNumPeptides+resultNum+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mintDMSJobNum+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mint_ScanNum) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mshort_ChargeState) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_MH) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mshort_MultiProtein) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mstr_Peptide) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTagId+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_observed_net) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mint_ScanNum) ; 
					sw.Write(mstrDelim) ; 
					// undefined intensity
					sw.Write(100) ; 
					sw.Write(mstrDelim) ; 
					// should be writing in correct apex.
					sw.WriteLine(3) ; 
				}
				sw.Close() ; 
			}

		}
		private void WriteXTandemScoresToFile(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{

			int numResults = results.marrXTandemResults.Length ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TScoreXTandemFileName, true)) 
			{
				if (!mblnTScoreXTandemFileNameHeaderWritten)
				{
					mblnTScoreXTandemFileNameHeaderWritten = true ; 
					// Headers first.
					sw.Write(m_TScoreXTandemColName_Peptide_ID);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_Hyperscore);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_Log_EValue);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_DeltaCn2);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_Y_Score);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_Y_Ions);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_B_Score);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_B_Ions);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_DelM);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreXTandemColName_Intensity);
					sw.Write(mstrDelim) ; 
					sw.WriteLine(m_TScoreXTandemColName_Normalized_Score);
				}
				
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					int percentDone = (resultNum * 100)/ numResults ; 
					PercentComplete(percentDone) ; 
					clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 

					if (!mobjOptions.IsToBeExported(xtResult))
						continue ; 

					sw.Write(mintNumPeptides+resultNum+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_peptide_hyperscore) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_log_peptide_e_value) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_deltaCN2) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_y_score) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mshort_num_y_ions) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_b_score) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mshort_num_b_ions) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_delta_mass) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(xtResult.mdbl_log_intensity) ; 
					sw.Write(mstrDelim) ; 

					double highNorm = 0 ; 

					if (xtResult.mshort_charge == 1)
						highNorm = 0.082 * xtResult.mdbl_peptide_hyperscore ; 
					else if (xtResult.mshort_charge == 2)
						highNorm = 0.085 * xtResult.mdbl_peptide_hyperscore ; 
					else
						highNorm = 0.0872 * xtResult.mdbl_peptide_hyperscore ; 					
					sw.WriteLine(highNorm) ; 
				}
				sw.Close() ; 
			}

		}
		private void WriteSequestScoresToFile(clsSequestAnalysisReader results, clsAnalysisDescription analysis)
		{

			int numResults = results.marrSequestResults.Length ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TScoreSequestFileName, true)) 
			{
				if (!mblnTScoreSequestFileNameHeaderWritten)
				{
					mblnTScoreSequestFileNameHeaderWritten = true ; 
					// Headers first.
					sw.Write(m_TScoreSequestColName_Peptide_ID);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_XCorr);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_DeltaCn);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_DeltaCn2);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_Sp);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_RankSp);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_RankXc);
					sw.Write(mstrDelim) ; 
					sw.Write(m_TScoreSequestColName_DelM);
					sw.Write(mstrDelim) ; 
					sw.WriteLine(m_TScoreSequestColName_XcRatio);
				}
				
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					int percentDone = (resultNum * 100)/ numResults ; 
					PercentComplete(percentDone) ; 
					clsSequestResults seqResult = results.marrSequestResults[resultNum] ; 

					if (!mobjOptions.IsToBeExported(seqResult))
						continue ; 

					sw.Write(mintNumPeptides+resultNum+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_XCorr) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_DelCn) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_DelCn2) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_Sp) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mshort_RankSp) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mshort_RankXc) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(seqResult.mdbl_DelM) ; 
					sw.Write(mstrDelim) ; 
					sw.WriteLine(seqResult.mdbl_XcRatio) ; 
				}
				sw.Close() ; 
			}

		}
		private void WriteMassTagsToFile()
		{
			int numMassTags = m_massTags.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TMassTagsFileName)) 
			{
				// Headers first.
				sw.Write( m_TMassTagsColName_Mass_Tag_ID ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Peptide ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Monoisotopic_Mass ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Multiple_Proteins ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Created ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Last_Affected ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Number_Of_Peptides ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Peptide_Obs_Count_Passing_Filter ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_High_Normalized_Score ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_High_Peptide_Prophet_Probability ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Min_Log_EValue ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsColName_Mod_Count ); 		
				sw.Write(mstrDelim) ;
                sw.Write(m_TMassTagsColName_Mod_Description);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PMT_Quality_Score);

                // Added these columns so they will work with MultiAlign.
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_External_Protein_ID);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_External_Reference_ID);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PepProphet_FScore_Max_CS1);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PepProphet_FScore_Max_CS2);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PepProphet_FScore_Max_CS3);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PepProphet_Probability_Max_CS1);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PepProphet_Probability_Max_CS2);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_PepProphet_Probability_Max_CS3);
                //sw.Write(mstrDelim);        
                //sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS1);
                //sw.Write(mstrDelim);
                //sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS2);
                //sw.Write(mstrDelim);        
                //sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS3);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagsColName_Is_confirmed);
                sw.Write(mstrDelim);
                sw.WriteLine(m_TMassTagsColName_High_Dicriminant_Score);

				
				for (int massTagNum = 0 ; massTagNum < numMassTags ; massTagNum++)
				{
					int percentDone = (massTagNum * 100)/ numMassTags ; 
					PercentComplete(percentDone) ; 

					clsMassTag massTag = (clsMassTag) m_massTags[massTagNum] ; 
					sw.Write(massTag.mint_mass_tag_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mstr_clean_peptide) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_monoisotopic_mass) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mshort_multiple_proteins) ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; // created
					sw.Write(mstrDelim) ; 
					sw.Write("") ; // last affected 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mint_number_of_peptides) ; 
					sw.Write(mstrDelim) ; 
					// peptide_obs_count_passing_filter: a value thats not reall set yet.  
					sw.Write(massTag.mint_peptide_obs_count_passing_filter) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_high_normalized_score) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_high_peptide_prophet_probability) ; // a value that is not set yet.
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_min_log_evalue) ; // a value that is not set for sequest only mass tags.
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mshort_mod_count) ; 
					sw.Write(mstrDelim) ; 

					if (massTag.mstr_mod_description.IndexOf(",") != -1)
						sw.Write("\""+ massTag.mstr_mod_description+ "\"") ;
					else
						sw.Write(massTag.mstr_mod_description) ; 

					sw.Write(mstrDelim) ;                    
                    sw.Write(mstrDelim);                    
                    sw.Write(mstrDelim);                    
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    //sw.Write(mstrDelim);
                    //sw.Write(mstrDelim);
                    //sw.Write(mstrDelim);
                    sw.WriteLine(mstrDelim);                    
				}
				sw.Close() ; 
			}

		}		
		private void WriteMassTagsNETToFile()
		{
			int numMassTags = m_massTags.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TMassTagsNETFileName)) 
			{
				// Headers first.
				sw.Write( m_TMassTagsNETColName_Mass_Tag_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsNETColName_Min_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsNETColName_Max_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsNETColName_Avg_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsNETColName_Cnt_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsNETColName_StD_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsNETColName_StdError_GANET); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( m_TMassTagsNETColName_PNET); 
				
				for (int massTagNum = 0 ; massTagNum < numMassTags ; massTagNum++)
				{
					int percentDone = (massTagNum * 100)/ numMassTags ; 
					PercentComplete(percentDone) ; 

					clsMassTag massTag = (clsMassTag) m_massTags[massTagNum] ; 

//					if (
//						(massTag.mdbl_min_log_evalue == clsMassTag.DEFAULT_MIN_LOG_EVAL && 
//						massTag.mdbl_high_normalized_score < mdbl_min_xcorr)
//						|| (massTag.mdbl_min_log_evalue != clsMassTag.DEFAULT_MIN_LOG_EVAL && 
//						massTag.mdbl_min_log_evalue > mdbl_max_log_eval)
//						)
//						continue ; 
					
					sw.Write(massTag.mint_mass_tag_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_min_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_max_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_avg_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mshort_cnt_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_std_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_stderr_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.WriteLine(massTag.mdbl_predicted_net) ; 
				}
				sw.Close() ; 
			}

		}	
		private void WriteMassTagsPeptideProphetStatsToFile()
		{
			int numMassTags = m_massTags.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TMassTagPeptideProphetStatsFileName)) 
			{
				// Headers first.
				sw.Write(m_TMassTagPeptideProphetStatsColName_Mass_Tag_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write(m_TMassTagPeptideProphetStatsColName_ObsCount_CS1); 
				sw.Write(mstrDelim) ; 
				sw.Write(m_TMassTagPeptideProphetStatsColName_ObsCount_CS2); 
				sw.Write(mstrDelim) ; 
				sw.Write(m_TMassTagPeptideProphetStatsColName_ObsCount_CS3); 
				sw.Write(mstrDelim) ; 
				sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_FScore_Avg_CS1); 
				sw.Write(mstrDelim) ; 
				sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_FScore_Avg_CS2); 
				sw.Write(mstrDelim) ;
                sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_FScore_Avg_CS3);
                sw.Write(mstrDelim);        
                sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS1);
                sw.Write(mstrDelim);
                sw.Write(m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS2);
                sw.Write(mstrDelim);        
                sw.WriteLine(m_TMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS3);
				
				for (int massTagNum = 0 ; massTagNum < numMassTags ; massTagNum++)
				{
					int percentDone = (massTagNum * 100)/ numMassTags ; 
					PercentComplete(percentDone) ; 

					clsMassTag massTag = (clsMassTag) m_massTags[massTagNum] ; 

					float fcs1 = MISSING_F_SCORE ; 
					float fcs2 = MISSING_F_SCORE ; 
					float fcs3 = MISSING_F_SCORE ; 

					if (massTag.marr_FScore_CS_Sum[0] != 0)
					{
						fcs1 = massTag.marr_FScore_CS_Sum[0]/massTag.marr_FScore_CS_Count[0] ; 
					}
					if (massTag.marr_FScore_CS_Sum[1] != 0)
					{
						fcs2 = massTag.marr_FScore_CS_Sum[1]/massTag.marr_FScore_CS_Count[1] ; 
					}
					if (massTag.marr_FScore_CS_Sum[2] != 0)
					{
						fcs3 = massTag.marr_FScore_CS_Sum[2]/massTag.marr_FScore_CS_Count[2] ; 
					}

					sw.Write(massTag.mint_mass_tag_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.marr_FScore_CS_Count[0]) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.marr_FScore_CS_Count[1]) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.marr_FScore_CS_Count[2]) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(fcs1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(fcs2) ; 
					sw.Write(mstrDelim) ;
                    sw.Write(fcs3);
                    sw.Write(mstrDelim);
                    sw.Write(mstrDelim);
                    sw.WriteLine(mstrDelim); 
				}
				sw.Close() ; 
			}

		}		
		private void WriteProteinsToFile()
		{
			int numProteins = marrProteins.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TProteinsFileName)) 
			{
				// Headers first.
				sw.Write( m_TProteinsColName_Ref_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TProteinsColName_Reference); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TProteinsColName_Description ); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TProteinsColName_Protein_Sequence); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TProteinsColName_Protein_Residue_Count); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TProteinsColName_Monoisotopic_Mass); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TProteinsColName_Protein_Collection_ID); 
				sw.Write(mstrDelim) ;
                sw.Write(m_TProteinsColName_Last_Affected);
                sw.Write(mstrDelim);
                sw.Write(m_TProteinsColName_External_Reference_ID);
                sw.Write(mstrDelim); 
                sw.Write(m_TProteinsColName_Protein_DB_ID);
                sw.Write(mstrDelim); 
                sw.WriteLine(m_TProteinsColName_External_Protein_ID);
				
				for (int proteinNum = 0 ; proteinNum < numProteins ; proteinNum++)
				{
					int percentDone = (proteinNum * 100)/ numProteins ; 
					PercentComplete(percentDone) ; 
					clsProtein protein = (clsProtein) marrProteins[proteinNum] ; 

					if (!mhashProteinsPassingConstraints.ContainsKey(protein.mint_ref_id))
						continue ; 

					sw.Write(protein.mint_ref_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(protein.mstr_reference) ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.WriteLine("") ;

                    sw.Write(mstrDelim);
                    sw.WriteLine("");
                    sw.Write(mstrDelim);
                    sw.WriteLine("");
                    sw.Write(mstrDelim);
                    sw.WriteLine(""); 
				}
				sw.Close() ; 
			}
		}
		private void WriteMassTagsToProteinMapToFile()
		{
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TMassTagsToProteinMapFileName)) 
			{
				// Headers first.
				sw.Write( m_TMassTagsToProteinMapColName_Mass_Tag_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Mass_Tag_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Ref_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Cleavage_State); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Fragment_Number); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Fragment_Span); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Residue_Start); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Residue_End); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Repeat_Count); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TMassTagsToProteinMapColName_Terminus_State); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( m_TMassTagsToProteinMapColName_Missed_Cleavage_Count); 

				int mapSize = mhashMassTagsToProteinMap.Count ; 
				int numDone = 0 ; 
				foreach (clsMassTagToProteinMap massTagToProteinMap in mhashMassTagsToProteinMap.Values)
				{
					int percentDone = (numDone * 100)/ mapSize ; 
					PercentComplete(percentDone) ; 

					numDone++ ; 
					clsMassTag massTag = (clsMassTag) m_massTags[massTagToProteinMap.mint_mass_tag_id] ; 
					sw.Write(massTagToProteinMap.mint_mass_tag_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTagToProteinMap.mint_ref_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTagToProteinMap.mshort_cleavage_state) ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTagToProteinMap.mshort_terminus_state) ; 
					sw.Write(mstrDelim) ; 
					sw.WriteLine(massTagToProteinMap.mshort_missed_cleavage_count) ; 
				}
				sw.Close() ; 
			}
		}
		private void WriteAnalysisDescriptionToFile()
		{
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(m_TAnalysisDescriptionFileName)) 
			{				
				// Headers first.
				sw.Write( m_TAnalysisDescriptionColName_Job); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Dataset); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Dataset_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Dataset_Created_DMS); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Dataset_Acq_Time_Start); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Dataset_Acq_Time_End); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Dataset_Scan_Count); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Experiment); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Campaign); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Organism); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Instrument_Class); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Instrument); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Analysis_Tool); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Parameter_File_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Settings_File_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Organism_DB_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Protein_Collection_List); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Protein_Options_List); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Completed); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_ResultType); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_Separation_Sys_Type); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_ScanTime_NET_Slope); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_ScanTime_NET_Intercept); 
				sw.Write(mstrDelim) ; 
				sw.Write( m_TAnalysisDescriptionColName_ScanTime_NET_RSquared); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( m_TAnalysisDescriptionColName_ScanTime_NET_Fit); 


				int numAnalyses = marrAnalyses.Count ; 
				for (int analysisNum = 0 ; analysisNum < numAnalyses ; analysisNum++)
				{
					int percentDone = (analysisNum * 100)/ numAnalyses ; 
					PercentComplete(percentDone) ;

					clsAnalysisDescription analysis = (clsAnalysisDescription) marrAnalyses[analysisNum] ; 
					sw.Write(analysis.mintDMSJobNum) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrDataset) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mintDatasetId+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mtimeCreated) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mtimeAcqStart) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mtimeAcqFinish) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mint_num_scans) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrExperiment) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrCampaign) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrOrganism) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrInstrument) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrInstrument) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrAnalysisTool) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrParamFile) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrSettingsFile) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrOrganismDB) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrProteinCollectionList) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mstrProteinOptionsList) ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write("") ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mdbl_scan_net_slope) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mdbl_scan_net_intercept) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(analysis.mdbl_scan_net_rsquared) ; 
					sw.Write(mstrDelim) ; 
					sw.WriteLine(analysis.mdbl_scan_net_fit) ; 
				}
				sw.Close() ; 
			}
		}
		#endregion

		#region "Helpers"
		public static void LinearRegression(float [] X, float [] Y, ref double slope, ref double intercept, ref double rsquare)
		{
			float SumY, SumX, SumXY, SumXX, SumYY ; 
			SumY = 0 ; 
			SumX = 0 ; 
			SumXY = 0 ;
			SumXX = 0 ;
			SumYY = 0 ;
			int num_pts = X.Length ; 
			for (int index = 0 ; index < num_pts ; index++)
			{
				SumX = SumX + X[index] ; 
				SumY = SumY + Y[index] ; 
				SumXX = SumXX + X[index] * X[index] ; 
				SumXY = SumXY + X[index] * Y[index] ; 
				SumYY = SumYY + Y[index] * Y[index] ; 
			}
			double denom = (num_pts * SumXX - SumX * SumX)  ; 
			if (denom == 0)
			{
				// overflow, etc. throw exception. 
				throw new System.Exception("Linear Regression not possible, all x values are the same") ; 
			}
			slope = (num_pts * SumXY - SumX * SumY) / denom ; 
			intercept = (SumY - slope * SumX) / num_pts ;

			double temp = (num_pts * SumXY - SumX * SumY) / Math.Sqrt((num_pts*SumXX - SumX * SumX)*(num_pts*SumYY - SumY * SumY)) ; 
			rsquare = temp * temp ; 
		}
		#endregion

		#region "XTandem aligning functions" 
		public void AlignXTandemDatasetToAverageNETs(   clsXTandemAnalysisReader results, 
			                                            ref float [] peptideScans,
                                                        ref float []peptidePredictedNET,
                                                        ref double slope, 
			                                            ref double intercept,
                                                        ref double rsquared)
		{
			try
			{
				menmAction = ACTION.IDLE ; 
				int numResults = results.marrXTandemResults.Length ; 
				// contains first scan number each observed peptide was seen in.
				Hashtable peptideTable = new Hashtable(results.marrSeqInfo.Length) ; 

				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 
					clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

					int massTagIndex = (int) mhashMassTags[xtResult.mstr_clean_peptide + seqInfo.mstr_mod_description] ; 
					clsMassTag massTag = (clsMassTag) m_massTags[massTagIndex] ; 
					if (massTag.mshort_cnt_ganet < mobjOptions.MinObservationsForExport 
						|| !mobjOptions.IsToBeExported(xtResult)
						|| xtResult.mdbl_log_peptide_e_value > mobjOptions.MaxLogEValForXTandemAlignment)
						continue ; 

					if (peptideTable.ContainsKey(massTagIndex))
					{
						if ((int) peptideTable[massTagIndex] < xtResult.mint_scan)
							peptideTable[massTagIndex] = xtResult.mint_scan ; 
					}
					else
					{
						peptideTable[massTagIndex] = xtResult.mint_scan ; 
					}
				}

				peptideScans = new float [peptideTable.Count] ; 
				peptidePredictedNET = new float [peptideTable.Count] ; 
				int numElementsSoFar = 0 ; 
				// now for each peptide calculate theoretical NET value. 
				foreach (int massTagIndex in peptideTable.Keys)
				{
					clsMassTag massTag = (clsMassTag) m_massTags[massTagIndex] ; 
					peptideScans[numElementsSoFar] = Convert.ToSingle((int) peptideTable[massTagIndex]) ; 
					peptidePredictedNET[numElementsSoFar] = Convert.ToSingle(massTag.mdbl_avg_ganet); 
					numElementsSoFar++ ; 
				}
				StatusMessage("Performing Alignment") ; 
				PercentComplete(0) ; 
				// regression is done on a different thread. Lets put a status. 
				System.Threading.ThreadStart monitorThreadStart = new System.Threading.ThreadStart(this.MonitorAlignment) ; 
				System.Threading.Thread monitorThread = new System.Threading.Thread(monitorThreadStart) ; 

				mobjRegressor.SetPoints(ref peptideScans, ref peptidePredictedNET);
				monitorThread.Start() ; 
				mobjRegressor.PerformRegression((Regressor.RegressionType)mobjOptions.RegressionType) ; 
				slope = mobjRegressor.Slope ; 
				intercept = mobjRegressor.Intercept ; 
				rsquared = mobjRegressor.RSquared ; 
			}
			catch (Exception ex)
			{
				// Let the user know what went wrong.
				ErrorMessage("Error performing alignment: " + ex.Message ) ;
				Console.WriteLine("Error performing alignment: " + ex.Message + ex.StackTrace);

				menmAction = ACTION.ERROR ; 
			}
			finally
			{
				if (menmAction != ACTION.ERROR)
					menmAction = ACTION.IDLE ; 
			}
		}

		public void AlignXTandemDatasetToTheoreticalNETs(clsXTandemAnalysisReader results, 
			ref float [] peptideScans, ref float []peptidePredictedNET, ref double slope, 
			ref double intercept, ref int numScans, ref double rsquared, bool useKrohkin)
		{
			try
			{
				int numResults = results.marrXTandemResults.Length ; 
				// contains first scan number each observed peptide was seen in.
				Hashtable peptideTable = new Hashtable(results.marrSeqInfo.Length) ; 

				numScans = 0 ; 
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 
					clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

					// in the alignment we will only use the unmodified peptides
					if (seqInfo.mshort_mod_count > mobjOptions.MaxModificationsForAlignment 
						|| xtResult.mdbl_log_peptide_e_value > mobjOptions.MaxLogEValForXTandemAlignment)
						continue ; 
					if (peptideTable.ContainsKey(xtResult.mstr_clean_peptide))
					{
						if ((int) peptideTable[xtResult.mstr_clean_peptide] < xtResult.mint_scan)
							peptideTable[xtResult.mstr_clean_peptide] = xtResult.mint_scan ; 
					}
					else
					{
						peptideTable[xtResult.mstr_clean_peptide] = xtResult.mint_scan ; 
					}
					if (xtResult.mint_scan > numScans)
					{
						numScans = xtResult.mint_scan ; 
					}
				}

				peptideScans = new float [peptideTable.Count] ; 
				peptidePredictedNET = new float [peptideTable.Count] ; 
				int numElementsSoFar = 0 ; 
				// now for each peptide calculate theoretical NET value. 
				foreach (string cleanPeptide in peptideTable.Keys)
				{
					peptideScans[numElementsSoFar] = Convert.ToSingle((int) peptideTable[cleanPeptide]) ;
                    if (useKrohkin)
                    {
                        peptidePredictedNET[numElementsSoFar] = mobjPredictionKrokhin.GetElutionTime(cleanPeptide);
                    }
                    else
                    {
                        peptidePredictedNET[numElementsSoFar] = mobjPredictionKangas.GetElutionTime(cleanPeptide);
                    }

					//Console.WriteLine(Convert.ToString(peptideScans[numElementsSoFar])+ "," + Convert.ToString(peptidePredictedNET[numElementsSoFar])) ; 
					numElementsSoFar++ ; 
				}
				try
				{
					StatusMessage("Performing Alignment") ; 
					PercentComplete(0) ; 
					// regression is done on a different thread. Lets put a status. 
					System.Threading.ThreadStart monitorThreadStart = new System.Threading.ThreadStart(this.MonitorAlignment) ; 
					System.Threading.Thread monitorThread = new System.Threading.Thread(monitorThreadStart) ; 

					mobjRegressor.SetPoints(ref peptideScans, ref peptidePredictedNET); 
					monitorThread.Start() ; 
					mobjRegressor.PerformRegression( (Regressor.RegressionType) mobjOptions.RegressionType) ;
					slope = mobjRegressor.Slope ; 
					intercept = mobjRegressor.Intercept ; 
					rsquared = mobjRegressor.RSquared ; 
					//LinearRegression(peptideScans, peptidePredictedNET, ref slope, ref intercept, ref rsquared) ; 
				}
				catch (Exception ex)
				{
					slope = 0 ; 
					intercept = 0 ; 
					rsquared = 0 ;
                    ErrorMessage("Could not align X!Tandem results. " + ex.Message);
				}
			}
			catch (Exception ex)
			{
				// Let the user know what went wrong.
				ErrorMessage("Error performing alignment: " + ex.Message ) ;
				Console.WriteLine("Error performing alignment: " + ex.Message + ex.StackTrace);
				
				menmAction = ACTION.ERROR ; 
			}
			finally
			{
				if (menmAction != ACTION.ERROR)
					menmAction = ACTION.IDLE ; 
			}
		}
        
        private void MonitorAlignment()
		{
			while(menmAction == ACTION.ALIGNING)
			{
				if (mobjRegressor != null)
				{
					int percent_complete = mobjRegressor.PercentComplete ; 
					Console.WriteLine("Regression Completion = " + percent_complete) ; 
					PercentComplete(percent_complete) ; 
				}
				System.Threading.Thread.Sleep(200); 
			}
		}

		public void ApplyScanTransformation(clsXTandemAnalysisReader results)
		{
			int numResults = results.marrXTandemResults.Length ; 
			for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
			{
				results.marrXTandemResults[resultNum].mdbl_observed_net = 
					GetTransformedNET(Convert.ToSingle(results.marrXTandemResults[resultNum].mint_scan)) ; 
			}
		}

		public float GetTransformedNET(float scan)
		{
			if (mobjRegressor != null)
				return mobjRegressor.GetNETFromScan(scan) ; 
			return 0 ; 
		}

		#endregion 

		#region "Sequest aligning functions" 
		public void AlignSequestDatasetToAverageNETs(clsSequestAnalysisReader results,
			ref float [] peptideScans, ref float []peptidePredictedNET, ref double slope, 
			ref double intercept, ref double rsquared ) 
		{
			try
			{
				int numResults = results.marrSequestResults.Length ; 
				// contains first scan number each observed peptide was seen in.
				Hashtable peptideTable = new Hashtable(results.marrSeqInfo.Length) ; 

				short minMTObservations = mobjOptions.MinObservationsForExport ; 

				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					clsSequestResults seqResult = results.marrSequestResults[resultNum] ; 
					clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

					int massTagIndex = (int) mhashMassTags[seqResult.mstr_clean_peptide + seqInfo.mstr_mod_description] ; 
					clsMassTag massTag = (clsMassTag) m_massTags[massTagIndex] ; 
					if (massTag.mshort_cnt_ganet < minMTObservations)
						continue ; 
					if (!mobjOptions.IsToBeExported(seqResult))
						continue ; 
					if (peptideTable.ContainsKey(massTagIndex))
					{
						if ((int) peptideTable[massTagIndex] < seqResult.mint_ScanNum)
							peptideTable[massTagIndex] = seqResult.mint_ScanNum ; 
					}
					else
					{
						peptideTable[massTagIndex] = seqResult.mint_ScanNum ; 
					}
				}

				peptideScans = new float [peptideTable.Count] ; 
				peptidePredictedNET = new float [peptideTable.Count] ; 
				int numElementsSoFar = 0 ; 
				// now for each peptide calculate theoretical NET value. 
				foreach (int massTagIndex in peptideTable.Keys)
				{
					clsMassTag massTag = (clsMassTag) m_massTags[massTagIndex] ; 
					peptideScans[numElementsSoFar] = Convert.ToSingle((int) peptideTable[massTagIndex]) ; 
					peptidePredictedNET[numElementsSoFar] = Convert.ToSingle(massTag.mdbl_avg_ganet); 
					numElementsSoFar++ ; 
				}
				StatusMessage("Performing Alignment") ; 
				PercentComplete(0) ; 
				// regression is done on a different thread. Lets put a status. 
				System.Threading.ThreadStart monitorThreadStart = new System.Threading.ThreadStart(this.MonitorAlignment) ; 
				System.Threading.Thread monitorThread = new System.Threading.Thread(monitorThreadStart) ; 

				mobjRegressor.SetPoints(ref peptideScans, ref peptidePredictedNET); 
				monitorThread.Start() ;
                mobjRegressor.PerformRegression((Regressor.RegressionType)mobjOptions.RegressionType); 
				slope = mobjRegressor.Slope ; 
				intercept = mobjRegressor.Intercept ; 
				rsquared = mobjRegressor.RSquared ; 
			}
			catch (Exception ex)
			{
				// Let the user know what went wrong.
				ErrorMessage("Error performing alignment: " + ex.Message ) ;
				Console.WriteLine("Error performing alignment: " + ex.Message + ex.StackTrace);

				menmAction = ACTION.ERROR ; 
			}
			finally
			{
				if (menmAction != ACTION.ERROR)
					menmAction = ACTION.IDLE ; 
			}
		}

		public void AlignSequestDatasetToTheoreticalNETs(   clsSequestAnalysisReader results, 
			                                                ref float []    peptideScans, 
                                                            ref float []    peptidePredictedNET, 
                                                            ref double      slope, 
			                                                ref double      intercept, 
                                                            ref int         numScans, 
                                                            ref double      rsquared,
                                                            bool useKrokhin)
		{
			try
			{
				menmAction      = ACTION.ALIGNING ; 
				int numResults  = results.marrSequestResults.Length ; 
				// contains first scan number each observed peptide was seen in.
				Hashtable peptideTable = new Hashtable(results.marrSeqInfo.Length) ; 

				numScans = 0 ; 

				short  max_mod_count_for_alignment  = mobjOptions.MaxModificationsForAlignment ; 
				double minXCorrForAlignment         = mobjOptions.MinXCorrForAlignment ; 

				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					clsSequestResults seqResult     = results.marrSequestResults[resultNum] ; 
					clsResultsToSeqMap resultToSeq  = results.marrResultsToSeqMap[resultNum] ; 

					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

					// in the alignment we will only use the unmodified peptides
					if (seqInfo.mshort_mod_count > max_mod_count_for_alignment)
						continue ;

					if (seqResult.mdbl_XCorr < minXCorrForAlignment || seqResult.mshort_RankXc != 1 )
						continue ; 

					if (peptideTable.ContainsKey(seqResult.mstr_clean_peptide))
					{
                        if ((int)peptideTable[seqResult.mstr_clean_peptide] < seqResult.mint_ScanNum)
                        {
                            peptideTable[seqResult.mstr_clean_peptide] = seqResult.mint_ScanNum;
                        }
					}
					else
					{
						peptideTable[seqResult.mstr_clean_peptide] = seqResult.mint_ScanNum ; 
					}
					if (seqResult.mint_ScanNum > numScans)
					{
						numScans = seqResult.mint_ScanNum ; 
					}
				}

				peptideScans         = new float [peptideTable.Count] ; 
				peptidePredictedNET  = new float [peptideTable.Count] ; 
				int numElementsSoFar = 0 ; 
				// now for each peptide calculate theoretical NET value. 
				foreach (string cleanPeptide in peptideTable.Keys)
				{
					peptideScans[numElementsSoFar] = Convert.ToSingle((int) peptideTable[cleanPeptide]) ;
                    if (useKrokhin)
                    {
                        peptidePredictedNET[numElementsSoFar] = mobjPredictionKrokhin.GetElutionTime(cleanPeptide);
                    }
                    else
                    {
                        peptidePredictedNET[numElementsSoFar] = mobjPredictionKangas.GetElutionTime(cleanPeptide);
                    }
					numElementsSoFar++ ; 
				}
				try
				{
					StatusMessage("Performing Alignment") ; 
					PercentComplete(0) ; 
					// regression is done on a different thread. Lets put a status. 
					System.Threading.ThreadStart monitorThreadStart = new System.Threading.ThreadStart(this.MonitorAlignment) ; 
					System.Threading.Thread monitorThread = new System.Threading.Thread(monitorThreadStart) ; 
					mobjRegressor.SetPoints(ref peptideScans, ref peptidePredictedNET); 
					monitorThread.Start() ;
                    mobjRegressor.PerformRegression((Regressor.RegressionType)mobjOptions.RegressionType);
					slope = mobjRegressor.Slope ; 
					intercept = mobjRegressor.Intercept ; 
					rsquared = mobjRegressor.RSquared ; 
					if (double.IsNaN(intercept))
					{
						System.Windows.Forms.MessageBox.Show("Intercept returned is undefined. Try changing the thresholds") ; 
					}
					if (double.IsNaN(slope))
					{
						System.Windows.Forms.MessageBox.Show("Slope returned is undefined. Try changing the thresholds") ; 
					}
				}
				catch (Exception ex)
				{
					// Let the user know what went wrong.
					ErrorMessage("Error in alignment: " + ex.Message ) ;					

					slope = 0 ; 
					intercept = 0 ; 
					rsquared = 0 ; 
				}
			}
			catch (Exception ex)
			{
                ErrorMessage("Could not align SEQUEST data. " + ex.Message);
				menmAction = ACTION.IDLE ; 
			}
			finally
			{
				if (menmAction != ACTION.ERROR)
					menmAction = ACTION.IDLE ; 
			}
		}

		public void ApplyScanTransformation(clsSequestAnalysisReader results)
		{
			int numResults = results.marrSequestResults.Length ; 
			for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
			{
				results.marrSequestResults[resultNum].mdbl_observed_net = 
					GetTransformedNET(Convert.ToSingle(results.marrSequestResults[resultNum].mint_ScanNum)) ; 
			}
		}

		#endregion 

		#region "Adding functions" 
		private void AddProteinsAndMassTagMap(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numSeqToProteinMap = results.marrSeqToProteinMap.Length ; 
			for (int mapNum = 0 ; mapNum < numSeqToProteinMap ; mapNum++)
			{
				clsSeqToProteinMap seqToProtein = results.marrSeqToProteinMap[mapNum] ; 
				// make sure this mass tag was already added.
				if (!mhashCurrentJobUniqIdToMassTagId.ContainsKey(seqToProtein.mint_unique_seq_id))
					continue ;
				clsProtein protein = new clsProtein() ; 
				if(!mhashProteins.Contains(seqToProtein.mstr_protein_name))
				{
					protein.mint_ref_id = marrProteins.Count ; 
					protein.mstr_reference = seqToProtein.mstr_protein_name ;
					marrProteins.Add(protein) ; 
					mhashProteins[seqToProtein.mstr_protein_name] = protein.mint_ref_id ; 
				}
				else
				{
					int protein_index = (int ) mhashProteins[seqToProtein.mstr_protein_name] ; 
					protein = (clsProtein) marrProteins[protein_index]  ; 
				}

				int massTagId = (int) mhashCurrentJobUniqIdToMassTagId[seqToProtein.mint_unique_seq_id] ; 
				string massTagIndex2ProteinIndex = Convert.ToString(seqToProtein.mstr_protein_name) 
					+ "_MT_ID_" + Convert.ToString(massTagId) ;
				if (!mhashMassTagsToProteinMap.Contains(massTagIndex2ProteinIndex))
				{
					clsMassTagToProteinMap massTagToProteinMap = new clsMassTagToProteinMap() ; 
					massTagToProteinMap.mint_mass_tag_id = massTagId ; 
					massTagToProteinMap.mint_ref_id = protein.mint_ref_id ; 
					// whats the difference between cleavage state and cleavage count 
					massTagToProteinMap.mshort_cleavage_state = seqToProtein.mshort_cleavage_state ;
					massTagToProteinMap.mshort_missed_cleavage_count = seqToProtein.mshort_cleavage_state ; 
					massTagToProteinMap.mshort_terminus_state = seqToProtein.mshort_terminus_state ; 
					mhashMassTagsToProteinMap[massTagIndex2ProteinIndex] = massTagToProteinMap ; 
				}
			}
		}

		private void AddProteinsAndMassTagMap(clsSequestAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numSeqToProteinMap = results.marrSeqToProteinMap.Length ; 
			for (int mapNum = 0 ; mapNum < numSeqToProteinMap ; mapNum++)
			{
				clsSeqToProteinMap seqToProtein = results.marrSeqToProteinMap[mapNum] ; 
				// make sure this mass tag was already added.
				if (!mhashCurrentJobUniqIdToMassTagId.ContainsKey(seqToProtein.mint_unique_seq_id))
					continue ;
				clsProtein protein = new clsProtein() ; 
				try
				{
					if(!mhashProteins.Contains(seqToProtein.mstr_protein_name))
					{
						protein.mint_ref_id = marrProteins.Count ; 
						protein.mstr_reference = seqToProtein.mstr_protein_name ;
						marrProteins.Add(protein) ; 
						mhashProteins[seqToProtein.mstr_protein_name] = protein.mint_ref_id ; 
					}
					else
					{
						int protein_index = (int ) mhashProteins[seqToProtein.mstr_protein_name] ; 
						protein = (clsProtein) marrProteins[protein_index]  ; 
					}

					int massTagId = (int) mhashCurrentJobUniqIdToMassTagId[seqToProtein.mint_unique_seq_id] ; 
					string massTagIndex2ProteinIndex = Convert.ToString(seqToProtein.mstr_protein_name) 
						+ "_MT_ID_" + Convert.ToString(massTagId) ;
					if (!mhashMassTagsToProteinMap.Contains(massTagIndex2ProteinIndex))
					{
						clsMassTagToProteinMap massTagToProteinMap = new clsMassTagToProteinMap() ; 
						massTagToProteinMap.mint_mass_tag_id = massTagId ; 
						massTagToProteinMap.mint_ref_id = protein.mint_ref_id ; 
						// whats the difference between cleavage state and cleavage count 
						massTagToProteinMap.mshort_cleavage_state = seqToProtein.mshort_cleavage_state ;
						massTagToProteinMap.mshort_missed_cleavage_count = seqToProtein.mshort_cleavage_state ; 
						massTagToProteinMap.mshort_terminus_state = seqToProtein.mshort_terminus_state ; 
						mhashMassTagsToProteinMap[massTagIndex2ProteinIndex] = massTagToProteinMap ; 
					}
				}
				catch (Exception ex)
				{
					// Let the user know what went wrong.
					ErrorMessage("Error adding mass tag to protein mapping: " + ex.Message ) ;
					Console.WriteLine("Error adding mass tag to protein mapping: " + ex.Message + ex.StackTrace);

					Console.WriteLine(ex.StackTrace + ex.Message) ; 
				}
			}
		}

		
		private void AddMassTags(clsSequestAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numResults = results.marrSequestResults.Length ; 
			// contains first scan number each observed peptide was seen in.
			Hashtable massTag2NET = new Hashtable(results.marrSeqInfo.Length) ; 

			for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
			{
				clsSequestResults seqResult = results.marrSequestResults[resultNum] ; 
				clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
				clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

				if (!mobjOptions.IsToBeExported(seqResult))
					continue ; 

				int percentDone = (resultNum*100)/numResults ; 
				PercentComplete(percentDone) ; 

				string peptideWithMod = seqResult.mstr_clean_peptide + seqInfo.mstr_mod_description ; 

				int massTagIndex = -1 ;

				double highNorm = seqResult.mdbl_XCorr ; 


				if (mhashMassTags.ContainsKey(peptideWithMod))
				{
					massTagIndex = (int) mhashMassTags[peptideWithMod] ; 
					if (((clsMassTag)m_massTags[massTagIndex]).mdbl_high_normalized_score < highNorm)
					{
						((clsMassTag)m_massTags[massTagIndex]).mdbl_high_normalized_score = highNorm ; 
					}
					if (!massTag2NET.ContainsKey(massTagIndex))
					{
						((clsMassTag)m_massTags[massTagIndex]).mint_number_of_peptides++ ;
						mhashCurrentJobUniqIdToMassTagId[seqInfo.mint_unique_seq_id] = massTagIndex ; 
					}

					short charge = seqResult.mshort_ChargeState ; 
					if (charge > clsSequestResults.MAX_CHARGE_FOR_FSCORE)
					{
						charge = clsSequestResults.MAX_CHARGE_FOR_FSCORE ; 
					}
					((clsMassTag)m_massTags[massTagIndex]).marr_FScore_CS_Count[charge-1]++ ; 
					((clsMassTag)m_massTags[massTagIndex]).marr_FScore_CS_Sum[charge-1] += Convert.ToSingle(seqResult.mdbl_FScore) ;
				}
				else
				{
					massTagIndex = m_massTags.Count ;
					clsMassTag massTag = new clsMassTag() ; 
					massTag.mdbl_monoisotopic_mass = seqInfo.mdbl_mono_mass ; 
					massTag.mint_mass_tag_id = massTagIndex ; 
					massTag.mdbl_high_normalized_score = highNorm ; 
					massTag.mint_number_of_peptides = 1 ;
					massTag.mint_peptide_obs_count_passing_filter = 0 ;
					massTag.mshort_mod_count = seqInfo.mshort_mod_count ; 
					massTag.mshort_multiple_proteins = seqResult.mshort_MultiProtein ; 
					massTag.mshort_PMT_Quality_Score = 0 ; 
					massTag.mstr_mod_description = seqInfo.mstr_mod_description ; 
					massTag.mstr_peptide = peptideWithMod ; 
					massTag.mstr_clean_peptide = seqResult.mstr_clean_peptide ;

                    if (mobjOptions.UseKrokhinNET)
                    {
                        massTag.mdbl_predicted_net = mobjPredictionKrokhin.GetElutionTime(seqResult.mstr_clean_peptide);
                    }
                    else
                    {
                        massTag.mdbl_predicted_net = mobjPredictionKangas.GetElutionTime(seqResult.mstr_clean_peptide);
                    }
					mhashMassTags[peptideWithMod] = massTagIndex ; 

					short charge = seqResult.mshort_ChargeState ; 
					if (charge > clsSequestResults.MAX_CHARGE_FOR_FSCORE)
					{
						charge = clsSequestResults.MAX_CHARGE_FOR_FSCORE ; 
					}
					massTag.marr_FScore_CS_Count[charge-1]++ ; 
					massTag.marr_FScore_CS_Sum[charge-1] += Convert.ToSingle(seqResult.mdbl_FScore) ; 

					m_massTags.Add(massTag) ; 
					mhashCurrentJobUniqIdToMassTagId[seqInfo.mint_unique_seq_id] = massTagIndex ; 
				}

				if (!massTag2NET.ContainsKey(massTagIndex))
				{
					massTag2NET[massTagIndex] = seqResult.mint_ScanNum ; 
				}
				else
				{
					int scan = (int) massTag2NET[massTagIndex] ; 
					if (scan > seqResult.mint_ScanNum)
					{
						massTag2NET[massTagIndex] = seqResult.mint_ScanNum ; 
					}
				}
			}
			// calculate NET in current job.
			foreach (int massTagIndex in massTag2NET.Keys)
			{
				clsPair pair = new clsPair() ; 
				pair.First = massTagIndex ; 
				int scan = (int) massTag2NET[massTagIndex] ;
				pair.Second = mobjRegressor.GetNETFromScan(scan) ;
				m_massTagsInJobs.Add(pair) ;
			}
		}

		private void AddMassTags(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numResults = results.marrXTandemResults.Length ; 
			// contains first scan number each observed peptide was seen in.
			Hashtable massTag2NET = new Hashtable(results.marrSeqInfo.Length) ; 

			for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
			{
				clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 
				clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
				clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

				if (!mobjOptions.IsToBeExported(xtResult))
					continue ; 

				int percentDone = (resultNum*100)/numResults ; 
				PercentComplete(percentDone) ; 

				string peptideWithMod = xtResult.mstr_clean_peptide + seqInfo.mstr_mod_description ; 

				int massTagIndex = -1 ;

				double highNorm = 0 ; 

				if (xtResult.mshort_charge == 1)
					highNorm = 0.082 * xtResult.mdbl_peptide_hyperscore ; 
				else if (xtResult.mshort_charge == 2)
					highNorm = 0.085 * xtResult.mdbl_peptide_hyperscore ; 
				else
					highNorm = 0.0872 * xtResult.mdbl_peptide_hyperscore ; 					

				if (mhashMassTags.ContainsKey(peptideWithMod))
				{
					massTagIndex = (int) mhashMassTags[peptideWithMod] ; 
					if (((clsMassTag)m_massTags[massTagIndex]).mdbl_high_normalized_score < highNorm)
					{
						((clsMassTag)m_massTags[massTagIndex]).mdbl_high_normalized_score = highNorm ; 
					}
					if (((clsMassTag)m_massTags[massTagIndex]).mdbl_min_log_evalue > xtResult.mdbl_log_peptide_e_value)
					{
						((clsMassTag)m_massTags[massTagIndex]).mdbl_min_log_evalue = xtResult.mdbl_log_peptide_e_value ; 
					}
					if (!massTag2NET.ContainsKey(massTagIndex))
					{
						((clsMassTag)m_massTags[massTagIndex]).mint_number_of_peptides++ ;
						mhashCurrentJobUniqIdToMassTagId[seqInfo.mint_unique_seq_id] = massTagIndex ; 
					}
				}
				else
				{
					massTagIndex = m_massTags.Count ;
					clsMassTag massTag = new clsMassTag() ; 
					massTag.mdbl_monoisotopic_mass = seqInfo.mdbl_mono_mass ; 
					massTag.mint_mass_tag_id = massTagIndex ; 
					massTag.mdbl_min_log_evalue = xtResult.mdbl_log_peptide_e_value ; 
					massTag.mdbl_high_normalized_score = highNorm ; 
					massTag.mint_number_of_peptides = 1 ;
					massTag.mint_peptide_obs_count_passing_filter = 0 ;
					massTag.mshort_mod_count = seqInfo.mshort_mod_count ; 
					massTag.mshort_multiple_proteins = xtResult.mshort_multi_protein_count ; 
					massTag.mshort_PMT_Quality_Score = 0 ; 
					massTag.mstr_mod_description = seqInfo.mstr_mod_description ; 
					massTag.mstr_peptide = peptideWithMod ; 
					massTag.mstr_clean_peptide = xtResult.mstr_clean_peptide ;
                    if (mobjOptions.UseKrokhinNET)
                    {
                        massTag.mdbl_predicted_net = mobjPredictionKrokhin.GetElutionTime(xtResult.mstr_clean_peptide);
                    }
                    else
                    {
                        massTag.mdbl_predicted_net = mobjPredictionKangas.GetElutionTime(xtResult.mstr_clean_peptide);
                    }
					mhashMassTags[peptideWithMod] = massTagIndex ; 
					m_massTags.Add(massTag) ; 
					mhashCurrentJobUniqIdToMassTagId[seqInfo.mint_unique_seq_id] = massTagIndex ; 
				}

				if (!massTag2NET.ContainsKey(massTagIndex))
				{
					massTag2NET[massTagIndex] = xtResult.mint_scan ; 
				}
				else
				{
					int scan = (int) massTag2NET[massTagIndex] ; 
					if (scan > xtResult.mint_scan)
					{
						massTag2NET[massTagIndex] = xtResult.mint_scan ; 
					}
				}
			}
			// calculate NET in current job.
			foreach (int massTagIndex in massTag2NET.Keys)
			{
				clsPair pair = new clsPair() ; 
				pair.First = massTagIndex ; 
				int scan = (int) massTag2NET[massTagIndex] ;
				pair.Second = mobjRegressor.GetNETFromScan(scan) ;
				m_massTagsInJobs.Add(pair) ;
			}
		}

	
		public void AddResults(clsXTandemAnalysisReader results, Regressor.RegressionType regressionType,
			clsAnalysisDescription analysis)
		{
			mblnXTandemFilesExist = true ; 
			marrAnalyses.Add(analysis) ; 
			mhashCurrentJobUniqIdToMassTagId.Clear() ; 
			StatusMessage("Aligning " + analysis.mstrDataset) ; 
			// Go through each line, check if mass tag, protein exists, if not, add them and get ids. 
			ApplyScanTransformation(results) ; 
			StatusMessage("Adding Mass Tags: ") ; 
			AddMassTags(results, analysis) ; 
			StatusMessage("Adding Proteins ") ; 
			AddProteinsAndMassTagMap(results, analysis) ; 
			StatusMessage("Writing peptides to temporary files ") ; 
			WritePeptidesToFile(results, analysis) ; 
			StatusMessage("Writing XTandem results to temporary files ") ; 
			WriteXTandemScoresToFile(results, analysis) ; 
			mintNumPeptides += results.marrXTandemResults.Length ; 
		}

		public void AddResults(clsSequestAnalysisReader results, Regressor.RegressionType regressionType,
			clsAnalysisDescription analysis)
		{
			mblnSequestFilesExist = true ; 
			marrAnalyses.Add(analysis) ; 
			mhashCurrentJobUniqIdToMassTagId.Clear() ; 
			StatusMessage("Aligning " + analysis.mstrDataset) ; 
			// Go through each line, check if mass tag, protein exists, if not, add them and get ids. 
			ApplyScanTransformation(results) ; 
			StatusMessage("Adding Mass Tags: ") ; 
			AddMassTags(results, analysis) ; 
			StatusMessage("Adding Proteins ") ; 
			AddProteinsAndMassTagMap(results, analysis) ; 
			StatusMessage("Writing peptides to temporary files ") ; 
			WritePeptidesToFile(results, analysis) ; 
			StatusMessage("Writing Sequest results to temporary files ") ; 
			WriteSequestScoresToFile(results, analysis) ; 
			mintNumPeptides += results.marrSequestResults.Length ; 
		}

		#endregion

		public class MassTagSorterClass : IComparer  
		{

			// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
			int IComparer.Compare( object x, object y )  
			{
				clsPair pairx = (clsPair) x ; 
				clsPair pairy = (clsPair) y ; 
				int id1 = (int) pairx.First ; 
				int id2 = (int) pairy.First ;
				return id1.CompareTo(id2) ; 
			}

		}

		#region "Summary Stats Region" 
		public void CalculateMassTagNETs()
		{
			m_massTagsInJobs.Sort(new MassTagSorterClass()) ; 
			// Go through each guy and compute average. 
			int startIndex = 0 ; 
			int stopIndex = 0 ; 
			int numPairs = m_massTagsInJobs.Count ; 
			while (startIndex < numPairs)
			{
				int massTagId = (int) ((clsPair)m_massTagsInJobs[startIndex]).First ; 
				while (stopIndex < numPairs)
				{
					int massTagIdThis = (int) ((clsPair)m_massTagsInJobs[stopIndex]).First ; 
					if (massTagIdThis != massTagId)
						break ; 
					stopIndex++ ; 
				}
				stopIndex-- ; 

				// now from startIndex to stopIndex there should be current mass tag. 
				int numObs = stopIndex - startIndex + 1; 
				double sumSquare = 0 ; 
				double sum = 0 ; 
				double min_ganet = double.MaxValue ; 
				double max_ganet = double.MinValue ; 
				for (int index = startIndex ; index <= stopIndex ; index++)
				{
					float val = (float) ((clsPair)m_massTagsInJobs[index]).Second  ; 
					if (val > max_ganet)
						max_ganet = val ; 
					if (val < min_ganet)
						min_ganet = val ; 
					sum += val ; 
					sumSquare += val * val ; 
				}
				double std = 0 ; 
				if (numObs > 1)
					std = Math.Sqrt((numObs * sumSquare - sum * sum) / (numObs * numObs-1)) ; 

				((clsMassTag)m_massTags[massTagId]).mshort_cnt_ganet = numObs ;
				((clsMassTag)m_massTags[massTagId]).mdbl_avg_ganet = sum/numObs ;
				((clsMassTag)m_massTags[massTagId]).mdbl_std_ganet = std ;
				((clsMassTag)m_massTags[massTagId]).mdbl_stderr_ganet = std / Math.Sqrt(numObs) ; 

				((clsMassTag)m_massTags[massTagId]).mdbl_min_ganet = min_ganet ;
				((clsMassTag)m_massTags[massTagId]).mdbl_max_ganet = max_ganet ;

				startIndex = stopIndex + 1 ; 
				stopIndex++ ; 
			}
		}
		public void CalculateProteinsPassingFilters()
		{
			mhashProteinsPassingConstraints.Clear() ; 
			StatusMessage("Calculating Proteins with at least one mass tag passing filter") ; 
			int mapSize = mhashMassTagsToProteinMap.Count ; 
			int numDone = 0 ; 
			foreach (clsMassTagToProteinMap massTagToProteinMap in mhashMassTagsToProteinMap.Values)
			{
				int percentDone = (numDone * 100)/ mapSize ; 
				PercentComplete(percentDone) ; 

				numDone++ ; 
				clsMassTag massTag = (clsMassTag) m_massTags[massTagToProteinMap.mint_mass_tag_id] ; 
//				if (
//					(massTag.mdbl_min_log_evalue == clsMassTag.DEFAULT_MIN_LOG_EVAL && 
//						massTag.mdbl_high_normalized_score < mdbl_min_xcorr)
//					|| (massTag.mdbl_min_log_evalue != clsMassTag.DEFAULT_MIN_LOG_EVAL && 
//						massTag.mdbl_min_log_evalue > mdbl_max_log_eval)
//					)
//					continue ; 
				if (mhashProteinsPassingConstraints.ContainsKey(massTagToProteinMap.mint_ref_id))
				{
					mhashProteinsPassingConstraints[massTagToProteinMap.mint_ref_id] = ((int) mhashProteinsPassingConstraints[massTagToProteinMap.mint_ref_id] +1); 
				}
				else
				{
					mhashProteinsPassingConstraints[massTagToProteinMap.mint_ref_id] = 1 ; 
				}
			}
		}
		#endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes of any allocated resources.
        /// </summary>
        public void Dispose()
        {
            DeleteFile(m_TAnalysisDescriptionFileName);
            DeleteFile(m_TPeptideFileName);
            DeleteFile(m_TMassTagsFileName);
            DeleteFile(m_TMassTagsNETFileName);
            DeleteFile(m_TMassTagPeptideProphetStatsFileName);
            DeleteFile(m_TMassTagsToProteinMapFileName);
            DeleteFile(m_TProteinsFileName);
            DeleteFile(m_TScoreXTandemFileName);
            DeleteFile(m_TScoreSequestFileName);		
        }
        #endregion
    }
}

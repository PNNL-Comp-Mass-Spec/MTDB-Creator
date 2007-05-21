using System;	
using System.Collections ; 
using System.Data.OleDb ; 
using System.Data ; 


namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsMTDB.
	/// </summary>
	public class clsMTDB
	{
		private int mintNumResultsSoFar ; 
		// hashtable that goes from sequence to index. 
		private Hashtable mhashMassTags ; 
		// hashtable that goes from protein name to protein index 
		private Hashtable mhashProteins ; 
		// hashtable that goes from Protein Reference Id_Peptide 
		private Hashtable mhashMassTagsToProteinMap ;
		// masstag Arraylist. 
		private ArrayList marrMassTags ; 
		// protein Arraylist.
		private ArrayList marrProteins ; 
		private Hashtable mhashCurrentJobUniqIdToMassTagId ; 
		private ArrayList marrMassTagsInJobs ; 
		private ArrayList marrAnalyses ; 
		public double mdbl_max_log_eval = 0 ; 
		private Hashtable mhashProteinsPassingConstraints = new Hashtable() ; 
		private string mstrDataName ; 

		private static string mstrTPeptidesColName_Peptide_Id = "Peptide_Id" ; 
		private static string mstrTPeptidesColName_Analysis_Id = "Analysis_Id" ; 
		private static string mstrTPeptidesColName_Scan_Number = "Scan_Number" ; 
		private static string mstrTPeptidesColName_Number_Of_Scans = "Number_Of_Scans" ; 
		private static string mstrTPeptidesColName_Charge_State = "Charge_State" ; 
		private static string mstrTPeptidesColName_MH = "MH" ; 
		private static string mstrTPeptidesColName_Multiple_Proteins = "Multiple_Proteins" ; 
		private static string mstrTPeptidesColName_Peptide = "Peptide" ; 
		private static string mstrTPeptidesColName_Mass_Tag_ID = "Mass_Tag_ID" ; 
		private static string mstrTPeptidesColName_GANET_Obs = "GANET_Obs" ; 
		private static string mstrTPeptidesColName_Scan_Time_Peak_Apex = "Scan_Time_Peak_Apex" ; 
		private static string mstrTPeptidesColName_Peak_Area = "Peak_Area" ; 
		private static string mstrTPeptidesColName_Peak_SN_Ratio = "Peak_SN_Ratio" ; 

		private static string mstrTMassTagsColName_Mass_Tag_ID = "Mass_Tag_ID" ; 
		private static string mstrTMassTagsColName_Peptide = "Peptide" ; 
		private static string mstrTMassTagsColName_Monoisotopic_Mass = "Monoisotopic_Mass" ; 
		private static string mstrTMassTagsColName_Multiple_Proteins = "Multiple_Proteins" ; 
		private static string mstrTMassTagsColName_Created = "Created" ; 
		private static string mstrTMassTagsColName_Last_Affected = "Last_Affected" ; 
		private static string mstrTMassTagsColName_Number_Of_Peptides = "Number_Of_Peptides" ; 
		private static string mstrTMassTagsColName_Peptide_Obs_Count_Passing_Filter = "Peptide_Obs_Count_Passing_Filter" ; 
		private static string mstrTMassTagsColName_High_Normalized_Score = "High_Normalized_Score" ; 
		private static string mstrTMassTagsColName_High_Peptide_Prophet_Probability = "High_Peptide_Prophet_Probability" ; 
		private static string mstrTMassTagsColName_Min_Log_EValue = "Min_Log_EValue" ; 
		private static string mstrTMassTagsColName_Mod_Count = "Mod_Count" ; 		
		private static string mstrTMassTagsColName_Mod_Description = "Mod_Description" ; 
		private static string mstrTMassTagsColName_PMT_Quality_Score = "PMT_Quality_Score" ; 

		private static string mstrTProteinsColName_Ref_ID = "Ref_ID" ; 
		private static string mstrTProteinsColName_Reference = "Reference" ; 
		private static string mstrTProteinsColName_Description = "Description" ; 
		private static string mstrTProteinsColName_Protein_Sequence = "Protein_Sequence" ; 
		private static string mstrTProteinsColName_Protein_Residue_Count = "Protein_Residue_Count" ; 
		private static string mstrTProteinsColName_Monoisotopic_Mass = "Monoisotopic_Mass" ; 
		private static string mstrTProteinsColName_Protein_Collection_ID = "Protein_Collection_ID" ; 
		private static string mstrTProteinsColName_Last_Affected = "Last_Affected" ; 

		private static string mstrTMassTagsToProteinMapColName_Mass_Tag_ID = "Mass_Tag_ID" ; 
		private static string mstrTMassTagsToProteinMapColName_Mass_Tag_Name = "Mass_Tag_Name" ; 
		private static string mstrTMassTagsToProteinMapColName_Ref_ID = "Ref_ID" ; 
		private static string mstrTMassTagsToProteinMapColName_Cleavage_State = "Cleavage_State" ; 
		private static string mstrTMassTagsToProteinMapColName_Fragment_Number = "Fragment_Number" ; 
		private static string mstrTMassTagsToProteinMapColName_Fragment_Span = "Fragment_Span" ; 
		private static string mstrTMassTagsToProteinMapColName_Residue_Start = "Residue_Start" ; 
		private static string mstrTMassTagsToProteinMapColName_Residue_End = "Residue_End" ; 
		private static string mstrTMassTagsToProteinMapColName_Repeat_Count = "Repeat_Count" ; 
		private static string mstrTMassTagsToProteinMapColName_Terminus_State = "Terminus_State" ; 
		private static string mstrTMassTagsToProteinMapColName_Missed_Cleavage_Count = "Missed_Cleavage_Count" ; 


		private static string mstrTScoreXTandemColName_Peptide_ID = "Peptide_ID" ; 
		private static string mstrTScoreXTandemColName_Hyperscore = "Hyperscore" ; 
		private static string mstrTScoreXTandemColName_Log_EValue = "Log_EValue" ; 
		private static string mstrTScoreXTandemColName_DeltaCn2 = "DeltaCn2" ; 
		private static string mstrTScoreXTandemColName_Y_Score = "Y_Score" ; 
		private static string mstrTScoreXTandemColName_Y_Ions = "Y_Ions" ; 
		private static string mstrTScoreXTandemColName_B_Score = "B_Score" ; 
		private static string mstrTScoreXTandemColName_B_Ions = "B_Ions" ; 
		private static string mstrTScoreXTandemColName_DelM = "DelM" ; 
		private static string mstrTScoreXTandemColName_Intensity = "Intensity" ; 
		private static string mstrTScoreXTandemColName_Normalized_Score = "Normalized_Score" ; 


		private static string mstrTMassTagsNETColName_Mass_Tag_ID = "Mass_Tag_ID" ; 
		private static string mstrTMassTagsNETColName_Min_GANET = "Min_GANET" ; 
		private static string mstrTMassTagsNETColName_Max_GANET = "Max_GANET" ; 
		private static string mstrTMassTagsNETColName_Avg_GANET = "Avg_GANET" ; 
		private static string mstrTMassTagsNETColName_Cnt_GANET = "Cnt_GANET" ; 
		private static string mstrTMassTagsNETColName_StD_GANET = "StD_GANET" ; 
		private static string mstrTMassTagsNETColName_StdError_GANET = "StdError_GANET" ; 
		private static string mstrTMassTagsNETColName_PNET = "PNET" ; 


		private static string mstrTAnalysisDescriptionColName_Job = "Job" ; 
		private static string mstrTAnalysisDescriptionColName_Dataset = "Dataset" ; 
		private static string mstrTAnalysisDescriptionColName_Dataset_ID = "Dataset_ID" ; 
		private static string mstrTAnalysisDescriptionColName_Dataset_Created_DMS = "Dataset_Created_DMS" ; 
		private static string mstrTAnalysisDescriptionColName_Dataset_Acq_Time_Start = "Dataset_Acq_Time_Start" ; 
		private static string mstrTAnalysisDescriptionColName_Dataset_Acq_Time_End = "Dataset_Acq_Time_End" ; 
		private static string mstrTAnalysisDescriptionColName_Dataset_Scan_Count = "Dataset_Scan_Count" ; 
		private static string mstrTAnalysisDescriptionColName_Experiment = "Experiment" ; 
		private static string mstrTAnalysisDescriptionColName_Campaign = "Campaign" ; 
		private static string mstrTAnalysisDescriptionColName_Organism = "Organism" ; 
		private static string mstrTAnalysisDescriptionColName_Instrument_Class = "Instrument_Class" ; 
		private static string mstrTAnalysisDescriptionColName_Instrument = "Instrument" ; 
		private static string mstrTAnalysisDescriptionColName_Analysis_Tool = "Analysis_Tool" ; 
		private static string mstrTAnalysisDescriptionColName_Parameter_File_Name = "Parameter_File_Name" ; 
		private static string mstrTAnalysisDescriptionColName_Settings_File_Name = "Settings_File_Name" ; 
		private static string mstrTAnalysisDescriptionColName_Organism_DB_Name = "Organism_DB_Name" ; 
		private static string mstrTAnalysisDescriptionColName_Protein_Collection_List = "Protein_Collection_List" ; 
		private static string mstrTAnalysisDescriptionColName_Protein_Options_List = "Protein_Options_List" ; 
		private static string mstrTAnalysisDescriptionColName_Completed = "Completed" ; 
		private static string mstrTAnalysisDescriptionColName_ResultType = "ResultType" ; 
		private static string mstrTAnalysisDescriptionColName_Separation_Sys_Type = "Separation_Sys_Type" ; 
		private static string mstrTAnalysisDescriptionColName_ScanTime_NET_Slope = "ScanTime_NET_Slope" ; 
		private static string mstrTAnalysisDescriptionColName_ScanTime_NET_Intercept = "ScanTime_NET_Intercept" ; 
		private static string mstrTAnalysisDescriptionColName_ScanTime_NET_RSquared = "ScanTime_NET_RSquared" ; 
		private static string mstrTAnalysisDescriptionColName_ScanTime_NET_Fit = "ScanTime_NET_Fit" ; 


		// T_Mass_Tag_Peptide_Prophet_Stats data structures:
		// Mass_Tag_ID, ObsCount_CS1, ObsCount_CS2, ObsCount_CS3, 
		// PepProphet_FScore_Max_CS1, PepProphet_FScore_Max_CS2, PepProphet_FScore_Max_CS3,
		// PepProphet_Probability_Max_CS1, PepProphet_Probability_Max_CS2, PepProphet_Probability_Max_CS3
		// PepProphet_Probability_Avg_CS1, PepProphet_Probability_Avg_CS2, PepProphet_Probability_Avg_CS3
		private static string mstrTMassTagPeptideProphetStatsColName_Mass_Tag_ID = "Mass_Tag_ID" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_ObsCount_CS1 = "ObsCount_CS1" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_ObsCount_CS2 = "ObsCount_CS2" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_ObsCount_CS3 = "ObsCount_CS3" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_FScore_Max_CS1 = "PepProphet_FScore_Max_CS1" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_FScore_Max_CS2 = "PepProphet_FScore_Max_CS2" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_FScore_Max_CS3 = "PepProphet_FScore_Max_CS3" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_Probability_Max_CS1 = "PepProphet_Probability_Max_CS1" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_Probability_Max_CS2 = "PepProphet_Probability_Max_CS2" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_Probability_Max_CS3 = "PepProphet_Probability_Max_CS3" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS1 = "PepProphet_Probability_Avg_CS1" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS2 = "PepProphet_Probability_Avg_CS2" ; 
		private static string mstrTMassTagPeptideProphetStatsColName_PepProphet_Probability_Avg_CS3 = "PepProphet_Probability_Avg_CS3" ; 


		private string mstrDelim = "," ; 


		private bool mblnTPeptideHeaderWritten = false ; 
		private bool mblnTScoreXTandemFileNameHeaderWritten = false ; 
		private string mstrTAnalysisDescriptionFileName ; 
		private string mstrTPeptideFileName ;
		private string mstrTMassTagsFileName ;
		private string mstrTMassTagsNETFileName ;
		private string mstrTMassTagsToProteinMapFileName ;
		private string mstrTProteinsFileName ;
		private string mstrTScoreXTandemFileName ;

		private static string mstrTAnalysisDescriptionTable = "T_Analysis_Description" ; 
		private static string mstrTPeptidesTable = "T_Peptides" ; 
		private static string mstrTMassTagsTable = "T_Mass_Tags" ; 
		private static string mstrTMassTagsNETTable = "T_Mass_Tags_NET" ; 
		private static string mstrTMassTagsToProteinMapTable = "T_Mass_Tags_to_Protein_Map" ; 
		private static string mstrTProteinsTable = "T_Proteins" ; 
		private static string mstrTScoreXTandemTable = "T_Score_XTandem" ; 
		private static string mstrTMassTagPeptideProphetStats = "T_Mass_Tag_Peptide_Prophet_Stats" ; 

		private string mstrAccessDBPath = "" ; 
		private int mintNumPeptides = 0 ;

		private clsNETPredictionKrokhin mobjKrokhinNET ; 
		private Regressor.clsRegressor mobjRegressor ; 


		private frmStatus.dlgSetPercentComplete mevntPercentComplete ; 
		private frmStatus.dlgSetStatusMessage mevntStatusMessage ; 

		// for each dataset, store 
		public clsMTDB(frmStatus statusForm)
		{
			//
			// TODO: Add constructor logic here
			//
			mintNumResultsSoFar = 0 ; 
			mhashMassTags = new Hashtable() ; 
			mhashProteins = new Hashtable() ; 
			mhashMassTagsToProteinMap = new Hashtable() ; 
			mobjKrokhinNET = new clsNETPredictionKrokhin() ; 
			mobjRegressor = new Regressor.clsRegressor() ; 
			marrMassTags = new ArrayList() ; 
			marrProteins = new ArrayList() ;
			mhashCurrentJobUniqIdToMassTagId = new Hashtable() ; 
			marrMassTagsInJobs = new ArrayList() ; 
			marrAnalyses = new ArrayList() ; 

			mevntPercentComplete = new MTDBCreator.frmStatus.dlgSetPercentComplete(statusForm.SetPrecentComplete) ; 
			mevntStatusMessage = new MTDBCreator.frmStatus.dlgSetStatusMessage(statusForm.SetStatusMessage) ; 
			DateTime now = DateTime.Now ; 

			string ext = ".txt" ;
			if (mstrDelim == ",")
				ext = ".csv" ; 
			mstrTAnalysisDescriptionFileName =  mstrTAnalysisDescriptionTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext ; 
			mstrTPeptideFileName =  mstrTPeptidesTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext ; 
			mstrTMassTagsFileName =  mstrTMassTagsTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds)  + ext ; 
			mstrTMassTagsNETFileName =  mstrTMassTagsNETTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			mstrTMassTagsToProteinMapFileName =  mstrTMassTagsToProteinMapTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 
			mstrTProteinsFileName =  mstrTProteinsTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext ; 
			mstrTScoreXTandemFileName =  mstrTScoreXTandemTable + "_" + now.Date + "_" + Convert.ToInt32(now.TimeOfDay.TotalMilliseconds) + ext; 

			mstrTAnalysisDescriptionFileName = mstrTAnalysisDescriptionFileName.Replace('/', '_') ;
			mstrTPeptideFileName = mstrTPeptideFileName.Replace('/','_'); 
			mstrTMassTagsFileName = mstrTMassTagsFileName.Replace('/','_'); 
			mstrTMassTagsNETFileName = mstrTMassTagsNETFileName.Replace('/','_'); 
			mstrTMassTagsToProteinMapFileName = mstrTMassTagsToProteinMapFileName.Replace('/','_'); 
			mstrTProteinsFileName = mstrTProteinsFileName.Replace('/','_'); 
			mstrTScoreXTandemFileName = mstrTScoreXTandemFileName.Replace('/','_'); 

			mstrTAnalysisDescriptionFileName = mstrTAnalysisDescriptionFileName.Replace(':', '_') ;
			mstrTPeptideFileName = mstrTPeptideFileName.Replace(':','_'); 
			mstrTMassTagsFileName = mstrTMassTagsFileName.Replace(':','_'); 
			mstrTMassTagsNETFileName = mstrTMassTagsNETFileName.Replace(':','_'); 
			mstrTMassTagsToProteinMapFileName = mstrTMassTagsToProteinMapFileName.Replace(':','_'); 
			mstrTProteinsFileName = mstrTProteinsFileName.Replace(':','_'); 
			mstrTScoreXTandemFileName = mstrTScoreXTandemFileName.Replace(':','_'); 

			mstrTAnalysisDescriptionFileName = mstrTAnalysisDescriptionFileName.Replace(' ', '_') ;
			mstrTPeptideFileName = mstrTPeptideFileName.Replace(' ','_'); 
			mstrTMassTagsFileName = mstrTMassTagsFileName.Replace(' ','_'); 
			mstrTMassTagsNETFileName = mstrTMassTagsNETFileName.Replace(' ','_'); 
			mstrTMassTagsToProteinMapFileName = mstrTMassTagsToProteinMapFileName.Replace(' ','_'); 
			mstrTProteinsFileName = mstrTProteinsFileName.Replace(' ','_'); 
			mstrTScoreXTandemFileName = mstrTScoreXTandemFileName.Replace(' ','_'); 

			string path = "c:\\" ; 
			mstrTAnalysisDescriptionFileName =path + mstrTAnalysisDescriptionFileName  ;
			mstrTPeptideFileName =path + mstrTPeptideFileName  ; 
			mstrTMassTagsFileName =path + mstrTMassTagsFileName  ; 
			mstrTMassTagsNETFileName =path + mstrTMassTagsNETFileName  ; 
			mstrTMassTagsToProteinMapFileName =path + mstrTMassTagsToProteinMapFileName  ; 
			mstrTProteinsFileName =path + mstrTProteinsFileName  ; 
			mstrTScoreXTandemFileName =path + mstrTScoreXTandemFileName  ; 

		}

		~clsMTDB()
		{
			try
			{
				System.IO.File.Delete(mstrTMassTagsFileName) ; 
			}
			catch (Exception ex)
			{
			}
			try
			{
				System.IO.File.Delete(mstrTMassTagsNETFileName) ; 
			}
			catch (Exception ex)
			{
			}
			try
			{
				System.IO.File.Delete(mstrTMassTagsToProteinMapFileName) ; 
			}
			catch (Exception ex)
			{
			}
			try
			{
				System.IO.File.Delete(mstrTPeptideFileName) ; 
			}
			catch (Exception ex)
			{
			}
			try
			{
				System.IO.File.Delete(mstrTScoreXTandemFileName) ; 
			}
			catch (Exception ex)
			{
			}
			try
			{
				System.IO.File.Delete(mstrTProteinsFileName) ; 
			}
			catch (Exception ex)
			{
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
			}
		}

		public void LoadResultsIntoDB()
		{
			WriteMassTagsToFile() ; 
			WriteProteinsToFile() ; 
			WriteMassTagsToProteinMapToFile() ; 
			WriteMassTagsNETToFile() ; 
			WriteAnalysisDescriptionToFile() ; 

			Microsoft.Office.Interop.Access.ApplicationClass oAccess = new Microsoft.Office.Interop.Access.ApplicationClass() ; 
			oAccess.NewCurrentDatabase(mstrAccessDBPath) ; 

			DateTime now = new DateTime() ; 
			
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTPeptidesTable, mstrTPeptideFileName, true,null, null) ; 
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTMassTagsTable, mstrTMassTagsFileName, true,null, null) ; 
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTMassTagsNETTable, mstrTMassTagsNETFileName, true,null, null) ; 
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTMassTagsToProteinMapTable, mstrTMassTagsToProteinMapFileName, true,null, null) ; 
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTProteinsTable, mstrTProteinsFileName, true,null, null) ; 
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTScoreXTandemTable, mstrTScoreXTandemFileName, true,null, null) ; 
			oAccess.DoCmd.TransferText(Microsoft.Office.Interop.Access.AcTextTransferType.acImportDelim, "", mstrTAnalysisDescriptionTable, mstrTAnalysisDescriptionFileName, true,null, null) ; 


			dao.Relation relation = oAccess.CurrentDb().CreateRelation("PeptideIDRelationship", mstrTPeptidesTable, mstrTScoreXTandemTable, dao.RelationAttributeEnum.dbRelationDontEnforce) ; 
			dao.Field fld = relation.CreateField(mstrTPeptidesColName_Peptide_Id,dao.DataTypeEnum.dbInteger, 0) ; 
			fld.ForeignName = mstrTScoreXTandemColName_Peptide_ID ; 
			relation.Fields.Append(fld) ; 
			oAccess.CurrentDb().Relations.Append(relation) ; 

			relation = oAccess.CurrentDb().CreateRelation("MassTagdIDRelationshipTPepTmass", mstrTMassTagsTable, mstrTPeptidesTable, dao.RelationAttributeEnum.dbRelationDontEnforce) ; 
			fld = relation.CreateField(mstrTMassTagsColName_Mass_Tag_ID,dao.DataTypeEnum.dbInteger, 0) ; 
			fld.ForeignName = mstrTPeptidesColName_Mass_Tag_ID ; 
			relation.Fields.Append(fld) ; 
			oAccess.CurrentDb().Relations.Append(relation) ; 

			relation = oAccess.CurrentDb().CreateRelation("MassTagdIDRelationshipTmassTmassNet", mstrTMassTagsTable, mstrTMassTagsNETTable, dao.RelationAttributeEnum.dbRelationDontEnforce) ; 
			fld = relation.CreateField(mstrTMassTagsColName_Mass_Tag_ID,dao.DataTypeEnum.dbInteger, 0) ; 
			fld.ForeignName = mstrTMassTagsNETColName_Mass_Tag_ID; 
			relation.Fields.Append(fld) ; 
			oAccess.CurrentDb().Relations.Append(relation) ; 

			relation = oAccess.CurrentDb().CreateRelation("MassTagdIDRelationshipTmassTmasstagtoprotein", mstrTMassTagsTable, mstrTMassTagsToProteinMapTable, dao.RelationAttributeEnum.dbRelationDontEnforce) ; 
			fld = relation.CreateField(mstrTMassTagsColName_Mass_Tag_ID,dao.DataTypeEnum.dbInteger, 0) ; 
			fld.ForeignName = mstrTMassTagsToProteinMapColName_Mass_Tag_ID ; 
			relation.Fields.Append(fld) ; 
			oAccess.CurrentDb().Relations.Append(relation) ; 

			relation = oAccess.CurrentDb().CreateRelation("ProteinIDRelationshipTproteinTmasstagtoprotein", mstrTProteinsTable,mstrTMassTagsToProteinMapTable,  dao.RelationAttributeEnum.dbRelationDontEnforce) ; 
			fld = relation.CreateField(mstrTProteinsColName_Ref_ID,dao.DataTypeEnum.dbInteger, 0) ; 
			fld.ForeignName = mstrTMassTagsToProteinMapColName_Ref_ID; 
			relation.Fields.Append(fld) ; 
			oAccess.CurrentDb().Relations.Append(relation) ; 

			relation = oAccess.CurrentDb().CreateRelation("AnalysisIDRelationship", mstrTAnalysisDescriptionTable, mstrTPeptidesTable, dao.RelationAttributeEnum.dbRelationDontEnforce) ; 
			fld = relation.CreateField(mstrTAnalysisDescriptionColName_Job, dao.DataTypeEnum.dbInteger, 0) ; 
			fld.ForeignName = mstrTPeptidesColName_Analysis_Id ; 
			relation.Fields.Append(fld) ; 
			oAccess.CurrentDb().Relations.Append(relation) ; 

			oAccess.CloseCurrentDatabase() ;
			oAccess = null ; 
		}

		public string AccessDBPath
		{
			set
			{
				mstrAccessDBPath = value ;
			}
		}

		#region "Writing Region"
		private void WritePeptidesToFile(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numResults = results.marrXTandemResults.Length ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTPeptideFileName, true)) 
			{
				if (!mblnTPeptideHeaderWritten)
				{
					mblnTPeptideHeaderWritten = true ; 
					// Headers first.
					sw.Write(mstrTPeptidesColName_Peptide_Id);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Analysis_Id);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Scan_Number);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Number_Of_Scans);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Charge_State);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_MH);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Multiple_Proteins);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Peptide);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Mass_Tag_ID);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_GANET_Obs);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Scan_Time_Peak_Apex);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTPeptidesColName_Peak_Area);
					sw.Write(mstrDelim) ; 
					sw.WriteLine(mstrTPeptidesColName_Peak_SN_Ratio);
				}
				
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					int percentDone = (resultNum*100)/numResults ; 
					mevntPercentComplete(percentDone) ; 
					clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 
					clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
					clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 
					int massTagId = (int) mhashCurrentJobUniqIdToMassTagId[resultToSeq.mint_unique_seq_id] ; 

					if (xtResult.mdbl_log_peptide_e_value > mdbl_max_log_eval)
						continue ; 
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

		private void WriteXTandemScoresToFile(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{

			int numResults = results.marrXTandemResults.Length ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTScoreXTandemFileName, true)) 
			{
				if (!mblnTScoreXTandemFileNameHeaderWritten)
				{
					mblnTScoreXTandemFileNameHeaderWritten = true ; 
					// Headers first.
					sw.Write(mstrTScoreXTandemColName_Peptide_ID);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_Hyperscore);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_Log_EValue);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_DeltaCn2);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_Y_Score);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_Y_Ions);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_B_Score);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_B_Ions);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_DelM);
					sw.Write(mstrDelim) ; 
					sw.Write(mstrTScoreXTandemColName_Intensity);
					sw.Write(mstrDelim) ; 
					sw.WriteLine(mstrTScoreXTandemColName_Normalized_Score);
				}
				
				for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
				{
					int percentDone = (resultNum * 100)/ numResults ; 
					mevntPercentComplete(percentDone) ; 
					clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 

					if (xtResult.mdbl_log_peptide_e_value > mdbl_max_log_eval)
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

		private void WriteMassTagsToFile()
		{
			int numMassTags = marrMassTags.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTMassTagsFileName)) 
			{
				// Headers first.
				sw.Write( mstrTMassTagsColName_Mass_Tag_ID ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Peptide ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Monoisotopic_Mass ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Multiple_Proteins ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Created ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Last_Affected ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Number_Of_Peptides ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Peptide_Obs_Count_Passing_Filter ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_High_Normalized_Score ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_High_Peptide_Prophet_Probability ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Min_Log_EValue ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Mod_Count ); 		
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsColName_Mod_Description ); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( mstrTMassTagsColName_PMT_Quality_Score ); 
				
				for (int massTagNum = 0 ; massTagNum < numMassTags ; massTagNum++)
				{
					int percentDone = (massTagNum * 100)/ numMassTags ; 
					mevntPercentComplete(percentDone) ; 

					clsMassTag massTag = (clsMassTag) marrMassTags[massTagNum] ; 

					if (massTag.mdbl_min_log_evalue > mdbl_max_log_eval)
						continue ; 
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
					sw.WriteLine(massTag.mshort_PMT_Quality_Score) ; // a value that is not set yet.
				}
				sw.Close() ; 
			}

		}
		
		private void WriteMassTagsNETToFile()
		{
			int numMassTags = marrMassTags.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTMassTagsNETFileName)) 
			{
				// Headers first.
				sw.Write( mstrTMassTagsNETColName_Mass_Tag_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsNETColName_Min_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsNETColName_Max_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsNETColName_Avg_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsNETColName_Cnt_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsNETColName_StD_GANET); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsNETColName_StdError_GANET); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( mstrTMassTagsNETColName_PNET); 
				
				for (int massTagNum = 0 ; massTagNum < numMassTags ; massTagNum++)
				{
					int percentDone = (massTagNum * 100)/ numMassTags ; 
					mevntPercentComplete(percentDone) ; 

					clsMassTag massTag = (clsMassTag) marrMassTags[massTagNum] ; 

					if (massTag.mdbl_min_log_evalue > mdbl_max_log_eval)
						continue ; 
					
					sw.Write(massTag.mint_mass_tag_id+1) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_min_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_max_ganet) ; 
					sw.Write(mstrDelim) ; 
					sw.Write(massTag.mdbl_agv_ganet) ; 
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
		
		private void WriteProteinsToFile()
		{
			int numProteins = marrProteins.Count ; 
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTProteinsFileName)) 
			{
				// Headers first.
				sw.Write( mstrTProteinsColName_Ref_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTProteinsColName_Reference); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTProteinsColName_Description ); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTProteinsColName_Protein_Sequence); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTProteinsColName_Protein_Residue_Count); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTProteinsColName_Monoisotopic_Mass); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTProteinsColName_Protein_Collection_ID); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( mstrTProteinsColName_Last_Affected); 
				
				for (int proteinNum = 0 ; proteinNum < numProteins ; proteinNum++)
				{
					int percentDone = (proteinNum * 100)/ numProteins ; 
					mevntPercentComplete(percentDone) ; 
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
				}
				sw.Close() ; 
			}
		}

		private void WriteMassTagsToProteinMapToFile()
		{
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTMassTagsToProteinMapFileName)) 
			{
				// Headers first.
				sw.Write( mstrTMassTagsToProteinMapColName_Mass_Tag_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Mass_Tag_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Ref_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Cleavage_State); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Fragment_Number); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Fragment_Span); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Residue_Start); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Residue_End); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Repeat_Count); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTMassTagsToProteinMapColName_Terminus_State); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( mstrTMassTagsToProteinMapColName_Missed_Cleavage_Count); 

				int mapSize = mhashMassTagsToProteinMap.Count ; 
				int numDone = 0 ; 
				foreach (clsMassTagToProteinMap massTagToProteinMap in mhashMassTagsToProteinMap.Values)
				{
					int percentDone = (numDone * 100)/ mapSize ; 
					mevntPercentComplete(percentDone) ; 

					numDone++ ; 
					clsMassTag massTag = (clsMassTag) marrMassTags[massTagToProteinMap.mint_mass_tag_id] ; 
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
			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrTAnalysisDescriptionFileName)) 
			{				
				// Headers first.
				sw.Write( mstrTAnalysisDescriptionColName_Job); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Dataset); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Dataset_ID); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Dataset_Created_DMS); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Dataset_Acq_Time_Start); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Dataset_Acq_Time_End); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Dataset_Scan_Count); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Experiment); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Campaign); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Organism); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Instrument_Class); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Instrument); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Analysis_Tool); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Parameter_File_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Settings_File_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Organism_DB_Name); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Protein_Collection_List); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Protein_Options_List); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Completed); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_ResultType); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_Separation_Sys_Type); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_ScanTime_NET_Slope); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_ScanTime_NET_Intercept); 
				sw.Write(mstrDelim) ; 
				sw.Write( mstrTAnalysisDescriptionColName_ScanTime_NET_RSquared); 
				sw.Write(mstrDelim) ; 
				sw.WriteLine( mstrTAnalysisDescriptionColName_ScanTime_NET_Fit); 


				int numAnalyses = marrAnalyses.Count ; 
				for (int analysisNum = 0 ; analysisNum < numAnalyses ; analysisNum++)
				{
					int percentDone = (analysisNum * 100)/ numAnalyses ; 
					mevntPercentComplete(percentDone) ;
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
			slope = (num_pts * SumXY - SumX * SumY) / (num_pts * SumXX - SumX * SumX) ; 
			intercept = (SumY - slope * SumX) / num_pts ;

			double temp = (num_pts * SumXY - SumX * SumY) / Math.Sqrt((num_pts*SumXX - SumX * SumX)*(num_pts*SumYY - SumY * SumY)) ; 
			rsquare = temp * temp ; 
		}

#endregion
		#region "XTandem aligning functions" 
		public void AlignXTandemDatasetToAverageNETs(clsXTandemAnalysisReader results, Regressor.RegressionType regressionType,
			ref float [] peptideScans, ref float []peptidePredictedNET, ref double slope, ref double intercept, short minObs)
		{
			int numResults = results.marrXTandemResults.Length ; 
			// contains first scan number each observed peptide was seen in.
			Hashtable peptideTable = new Hashtable(results.marrSeqInfo.Length) ; 

			for (int resultNum = 0 ; resultNum < numResults ; resultNum++)
			{
				clsXTandemResults xtResult = results.marrXTandemResults[resultNum] ; 
				clsResultsToSeqMap resultToSeq = results.marrResultsToSeqMap[resultNum] ; 
				clsSeqInfo seqInfo = results.marrSeqInfo[resultToSeq.mint_unique_seq_id-1] ; 

				int massTagIndex = (int) mhashMassTags[xtResult.mstr_clean_peptide + seqInfo.mstr_mod_description] ; 
				clsMassTag massTag = (clsMassTag) marrMassTags[massTagIndex] ; 
				if (massTag.mshort_cnt_ganet < minObs)
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
				clsMassTag massTag = (clsMassTag) marrMassTags[massTagIndex] ; 
				peptideScans[numElementsSoFar] = Convert.ToSingle((int) peptideTable[massTagIndex]) ; 
				peptidePredictedNET[numElementsSoFar] = Convert.ToSingle(massTag.mdbl_agv_ganet); 
				numElementsSoFar++ ; 
			}
			mobjRegressor.SetPoints(ref peptideScans, ref peptidePredictedNET); 
			mobjRegressor.PerformRegression(regressionType) ; 
			slope = mobjRegressor.Slope ; 
			intercept = mobjRegressor.Intercept ; 
		}

		public void AlignXTandemDatasetToTheoreticalNETs(clsXTandemAnalysisReader results, Regressor.RegressionType regressionType,
			ref float [] peptideScans, ref float []peptidePredictedNET, ref double slope, ref double intercept, 
			ref int numScans, ref double rsquared)
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
				if (seqInfo.mshort_mod_count != 0)
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
				peptidePredictedNET[numElementsSoFar] = mobjKrokhinNET.GetElutionTime(cleanPeptide) ; 
				//Console.WriteLine(Convert.ToString(peptideScans[numElementsSoFar])+ "," + Convert.ToString(peptidePredictedNET[numElementsSoFar])) ; 
				numElementsSoFar++ ; 
			}
			LinearRegression(peptideScans, peptidePredictedNET, ref slope, ref intercept, ref rsquared) ; 

			mobjRegressor.SetPoints(ref peptideScans, ref peptidePredictedNET); 
			mobjRegressor.PerformRegression(regressionType) ;
			slope = mobjRegressor.Slope ; 
			intercept = mobjRegressor.Intercept ; 
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

		#region "Adding functions" 
		private void AddProteinsAndMassTagMap(clsXTandemAnalysisReader results, clsAnalysisDescription analysis)
		{
			int numSeqToProteinMap = results.marrSeqToProteinMap.Length ; 
			for (int mapNum = 0 ; mapNum < numSeqToProteinMap ; mapNum++)
			{
				clsSeqToProteinMap seqToProtein = results.marrSeqToProteinMap[mapNum] ; 
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

				int percentDone = (resultNum*100)/numResults ; 
				mevntPercentComplete(percentDone) ; 

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
					if (((clsMassTag)marrMassTags[massTagIndex]).mdbl_high_normalized_score < highNorm)
					{
						((clsMassTag)marrMassTags[massTagIndex]).mdbl_high_normalized_score = highNorm ; 
					}
					if (((clsMassTag)marrMassTags[massTagIndex]).mdbl_min_log_evalue > xtResult.mdbl_log_peptide_e_value)
					{
						((clsMassTag)marrMassTags[massTagIndex]).mdbl_min_log_evalue = xtResult.mdbl_log_peptide_e_value ; 
					}
					if (!massTag2NET.ContainsKey(massTagIndex))
					{
						((clsMassTag)marrMassTags[massTagIndex]).mint_number_of_peptides++ ;
						mhashCurrentJobUniqIdToMassTagId[seqInfo.mint_unique_seq_id] = massTagIndex ; 
					}
				}
				else
				{
					massTagIndex = marrMassTags.Count ;
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
					massTag.mdbl_predicted_net = mobjKrokhinNET.GetElutionTime(xtResult.mstr_clean_peptide) ; 
					mhashMassTags[peptideWithMod] = massTagIndex ; 
					marrMassTags.Add(massTag) ; 
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
				marrMassTagsInJobs.Add(pair) ;
			}
		}

		public void AddResults(clsXTandemAnalysisReader results, Regressor.RegressionType regressionType,
			clsAnalysisDescription analysis)
		{
			marrAnalyses.Add(analysis) ; 
			mhashCurrentJobUniqIdToMassTagId.Clear() ; 
			mevntStatusMessage("Aligning " + analysis.mstrDataset) ; 
			// Go through each line, check if mass tag, protein exists, if not, add them and get ids. 
			ApplyScanTransformation(results) ; 
			mevntStatusMessage("Adding Mass Tags: ") ; 
			AddMassTags(results, analysis) ; 
			mevntStatusMessage("Adding Proteins ") ; 
			AddProteinsAndMassTagMap(results, analysis) ; 
			mevntStatusMessage("Writing peptides to temporary files ") ; 
			WritePeptidesToFile(results, analysis) ; 
			mevntStatusMessage("Writing XTandem results to temporary files ") ; 
			WriteXTandemScoresToFile(results, analysis) ; 
			mintNumPeptides += results.marrXTandemResults.Length ; 
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
			marrMassTagsInJobs.Sort(new MassTagSorterClass()) ; 
			// Go through each guy and compute average. 
			int startIndex = 0 ; 
			int stopIndex = 0 ; 
			int numPairs = marrMassTagsInJobs.Count ; 
			while (startIndex < numPairs)
			{
				int massTagId = (int) ((clsPair)marrMassTagsInJobs[startIndex]).First ; 
				while (stopIndex < numPairs)
				{
					int massTagIdThis = (int) ((clsPair)marrMassTagsInJobs[stopIndex]).First ; 
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
					float val = (float) ((clsPair)marrMassTagsInJobs[index]).Second  ; 
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

				((clsMassTag)marrMassTags[massTagId]).mshort_cnt_ganet = numObs ;
				((clsMassTag)marrMassTags[massTagId]).mdbl_agv_ganet = sum/numObs ;
				((clsMassTag)marrMassTags[massTagId]).mdbl_std_ganet = std ;
				((clsMassTag)marrMassTags[massTagId]).mdbl_stderr_ganet = std / Math.Sqrt(numObs) ; 

				((clsMassTag)marrMassTags[massTagId]).mdbl_min_ganet = min_ganet ;
				((clsMassTag)marrMassTags[massTagId]).mdbl_max_ganet = max_ganet ;

				startIndex = stopIndex + 1 ; 
				stopIndex++ ; 
			}
		}
		public void CalculateProteinsPassingFilters()
		{
			mhashProteinsPassingConstraints.Clear() ; 
			mevntStatusMessage("Calculating Proteins with at least one mass tag passing filter") ; 
			int mapSize = mhashMassTagsToProteinMap.Count ; 
			int numDone = 0 ; 
			foreach (clsMassTagToProteinMap massTagToProteinMap in mhashMassTagsToProteinMap.Values)
			{
				int percentDone = (numDone * 100)/ mapSize ; 
				mevntPercentComplete(percentDone) ; 

				numDone++ ; 
				clsMassTag massTag = (clsMassTag) marrMassTags[massTagToProteinMap.mint_mass_tag_id] ; 
				if (massTag.mdbl_min_log_evalue > mdbl_max_log_eval)
					continue ; 
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
	}
}

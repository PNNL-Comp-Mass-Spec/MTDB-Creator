using System;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsAnalysisDescription.
	/// </summary>
	public class clsAnalysisDescription
	{
		// Header from DMS: 
		//"Job	Pri	State	Tool	Dataset	Campaign	Experiment	Instrument	Parm_File	Settings_File	Organism	
		// Organism_DB	ProteinCollection_List	Protein_Options	Comment	Created	Started	Finished	Processor	
		// Run_Request	Archive Folder Path"
		private const string mstrHeaderJobNum = "Job" ; 
		private const string mstrHeaderPriority = "Pri" ; 
		private const string mstrHeaderState = "State" ; 
		private const string mstrHeaderTool = "Tool" ; 
		private const string mstrHeaderDataset = "Dataset" ; 
		private const string mstrHeaderCampaign = "Campaign" ; 
		private const string mstrHeaderExperiment = "Experiment" ; 
		private const string mstrHeaderInstrument = "Instrument" ; 
		private const string mstrHeaderParameterFile = "Parm_File" ; 
		private const string mstrHeaderSettingsFile = "Settings_File" ; 
		private const string mstrHeaderOrganism = "Organism" ; 
		private const string mstrHeaderOrganismDB = "Organism_DB" ; 
		private const string mstrHeaderProteinCollectionList = "ProteinCollection_List" ; 
		private const string mstrHeaderProteinOptionsList = "Protein_Options" ; 
		private const string mstrHeaderCreated = "Created" ; 
		private const string mstrHeaderStarted = "Started" ; 
		private const string mstrHeaderFinished = "Finished" ; 
		private const string mstrHeaderArchivePath = "Archive Folder Path" ; 
		private static int mintNumAnalyses = 0 ;

		public string mstrDataset ; // maps to mstrHeaderDataset
		public int mintDatasetId ; 
		public int mintDMSJobNum ; // maps to mstrHeaderJobNum
		public DateTime mtimeCreated ; // maps to mstrHeaderCreated
		public DateTime mtimeAcqStart ; // maps to mstrHeaderStarted
		public DateTime mtimeAcqFinish ; // maps to mstrHeaderFinished
		public string mstrExperiment ; // maps to mstrHeaderExperiment
		public string mstrCampaign ; // maps to mstrHeaderCampaign
		public string mstrOrganism ; //maps to mstrHeaderOrganism
		public string mstrOrganismDB ; //maps to mstrHeaderOrganismDB
		public string mstrInstrument ; // maps to mstrHeaderInstrument 
		public string mstrAnalysisTool ; //maps to mstrHeaderTool 
		public string mstrParamFile ; //maps to mstrHeaderParameterFile 
		public string mstrSettingsFile ; //maps to mastrHeaderSettingsFile
		public string mstrProteinCollectionList ; //maps to mstrHeaderProteinCollectionList 
		public string mstrProteinOptionsList ; // maps to mstrProteinOptionsList
		public string mstrArchivePath ; // maps to mstrArchivePath

		public double mdbl_scan_net_slope ;		
		public double mdbl_scan_net_intercept ; 
		public double mdbl_scan_net_rsquared ; 
		public double mdbl_scan_net_fit ; 
		public int mint_num_scans ; 
		public int mint_num_unique_mass_tags ; 

		private static short mshortColNumDMSJobNum = -1  ; // maps to mshortColNumHeaderJobNum
		private static short mshortColNumAnalysisTool = -1; //maps to mshortColNumHeaderTool 
		private static short mshortColNumDataset = -1 ; // maps to mshortColNumHeaderDataset
		private static short mshortColNumCampaign = -1 ; // maps to mshortColNumHeaderCampaign
		private static short mshortColNumExperiment = -1 ; // maps to mshortColNumHeaderExperiment
		private static short mshortColNumInstrument = -1 ; // maps to mshortColNumHeaderInstrument 
		private static short mshortColNumParamFile = -1 ; //maps to mshortColNumHeaderParameterFile 
		private static short mshortColNumSettingsFile = -1 ; //maps to mstrHeaderSettingsFile
		private static short mshortColNumOrganism = -1 ; //maps to mshortColNumHeaderOrganism
		private static short mshortColNumOrganismDB = -1 ; //maps to mshortColNumHeaderOrganismDB
		private static short mshortColNumProteinCollectionList = -1 ; //maps to mshortColNumHeaderProteinCollectionList 
		private static short mshortColNumProteinOptionsList = -1 ; // maps to mshortColNumProteinOptionsList
		private static short mshortColNumCreated = -1 ; // maps to mshortColNumHeaderCreated
		private static short mshortColNumAcqStart = -1 ; // maps to mshortColNumHeaderStarted
		private static short mshortColNumAcqFinish = -1 ; // maps to mshortColNumHeaderFinished
		private static short mshortColNumArchivePath = -1 ; // maps to mshortCol

		public clsAnalysisDescription()
		{
		}
		public clsAnalysisDescription(string line, char [] delimiters)
		{
			string xTandemExtension = "_xt.txt" ; 
			bool removeXTandemExtension = false ; 
			bool firstProblem = true ; 
			
			string [] items = line.Split(delimiters) ; 
			if (mshortColNumDataset != -1)
			{
				mstrDataset = items[mshortColNumDataset]; 
				if (mstrDataset.LastIndexOf(xTandemExtension) != -1 
					&& mstrDataset.LastIndexOf(xTandemExtension) == mstrDataset.Length - xTandemExtension.Length)
				{
					if (firstProblem)
					{
						System.Windows.Forms.DialogResult rs = System.Windows.Forms.MessageBox.Show("Datasetname has xtandem extension. Program requires that no extensions be added. Remove ?", "Dataset Name with Extension", System.Windows.Forms.MessageBoxButtons.YesNo) ; 
						if (rs == System.Windows.Forms.DialogResult.Yes)
						{
							removeXTandemExtension = true ; 
						}
						firstProblem = false ; 
					}
					if (!firstProblem && removeXTandemExtension)
					{
						mstrDataset = mstrDataset.Substring(0,mstrDataset.LastIndexOf(xTandemExtension)) ; 
					}
				}
			}
			if (mshortColNumDMSJobNum != -1)
				mintDMSJobNum = Convert.ToInt32(items[mshortColNumDMSJobNum]) ; 
			if (mshortColNumCreated != -1)
				mtimeCreated = Convert.ToDateTime(items[mshortColNumCreated]) ; 
			if (mshortColNumAcqStart != -1)
				mtimeAcqStart = Convert.ToDateTime(items[mshortColNumAcqStart]) ; 
			if (mshortColNumAcqFinish != -1)
				mtimeAcqFinish = Convert.ToDateTime(items[mshortColNumAcqFinish]) ; 
			if (mshortColNumExperiment != -1)
				mstrExperiment = items[mshortColNumExperiment] ; 
			if (mshortColNumCampaign != -1)
				mstrCampaign = items[mshortColNumCampaign] ; 
			if (mshortColNumOrganism != -1)
				mstrOrganism = items[mshortColNumOrganism] ; 
			if (mshortColNumOrganismDB != -1)
				mstrOrganismDB = items[mshortColNumOrganismDB] ; 
			if (mshortColNumInstrument != -1)
				mstrInstrument = items[mshortColNumInstrument] ; 
			if (mshortColNumAnalysisTool != -1)
				mstrAnalysisTool = items[mshortColNumAnalysisTool] ; 
			if (mshortColNumParamFile != -1)
				mstrParamFile = items[mshortColNumParamFile] ; 
			if (mshortColNumSettingsFile != -1)
				mstrSettingsFile = items[mshortColNumSettingsFile] ; 
			if (mshortColNumProteinCollectionList != -1)
				mstrProteinCollectionList = items[mshortColNumProteinCollectionList] ; 
			if (mshortColNumProteinOptionsList != -1)
				mstrProteinOptionsList = items[mshortColNumProteinOptionsList] ; 
			if (mshortColNumArchivePath != -1)
				mstrArchivePath = items[mshortColNumArchivePath] ; 
			if (mintNumAnalyses != -1)
				mintDatasetId = mintNumAnalyses ; 
			mintNumAnalyses++ ; 
		}
		public static void SetHeader(string headerLine, char [] delimiters)
		{

			mshortColNumDMSJobNum = -1  ; // maps to mshortColNumHeaderJobNum
			mshortColNumAnalysisTool = -1; //maps to mshortColNumHeaderTool 
			mshortColNumDataset = -1 ; // maps to mshortColNumHeaderDataset
			mshortColNumCampaign = -1 ; // maps to mshortColNumHeaderCampaign
			mshortColNumExperiment = -1 ; // maps to mshortColNumHeaderExperiment
			mshortColNumInstrument = -1 ; // maps to mshortColNumHeaderInstrument 
			mshortColNumParamFile = -1 ; //maps to mshortColNumHeaderParameterFile 
			mshortColNumSettingsFile = -1 ; //maps to mstrHeaderSettingsFile
			mshortColNumOrganism = -1 ; //maps to mshortColNumHeaderOrganism
			mshortColNumOrganismDB = -1 ; //maps to mshortColNumHeaderOrganismDB
			mshortColNumProteinCollectionList = -1 ; //maps to mshortColNumHeaderProteinCollectionList 
			mshortColNumProteinOptionsList = -1 ; // maps to mshortColNumProteinOptionsList
			mshortColNumCreated = -1 ; // maps to mshortColNumHeaderCreated
			mshortColNumAcqStart = -1 ; // maps to mshortColNumHeaderStarted
			mshortColNumAcqFinish = -1 ; // maps to mshortColNumHeaderFinished
			mshortColNumArchivePath = -1 ; // maps to mshortCol
			mintNumAnalyses = 0 ;

			string [] colHeaders = headerLine.Split(delimiters) ; 
			for (int i = 0 ; i < colHeaders.Length ; i++)
			{
				switch(colHeaders[i])
				{
					case  mstrHeaderDataset:
						mshortColNumDataset = (short) i ; 
						break ;
					case  mstrHeaderJobNum:
						mshortColNumDMSJobNum = (short) i ; 
						break ;
					case  mstrHeaderCreated:
						mshortColNumCreated = (short) i ; 
						break ;
					case  mstrHeaderStarted:
						mshortColNumAcqStart = (short) i ;
						break ;
					case  mstrHeaderFinished:
						mshortColNumAcqFinish = (short) i ;
						break ;
					case  mstrHeaderExperiment:
						mshortColNumExperiment = (short) i ;
						break ;
					case  mstrHeaderCampaign:
						mshortColNumCampaign = (short) i ;
						break ;
					case  mstrHeaderOrganism:
						mshortColNumOrganism = (short) i ; 
						break ;
					case  mstrHeaderOrganismDB:
						mshortColNumOrganismDB = (short) i ; 
						break ;
					case  mstrHeaderInstrument :
						mshortColNumInstrument = (short) i ; 
						break ;
					case  mstrHeaderTool :
						mshortColNumAnalysisTool = (short) i ; 
						break ;
					case  mstrHeaderParameterFile :
						mshortColNumParamFile = (short) i ; 
						break ;
					case  mstrHeaderSettingsFile:
						mshortColNumSettingsFile = (short) i ; 
						break ;
					case  mstrHeaderProteinCollectionList :
						mshortColNumProteinCollectionList = (short) i ; 
						break ;
					case  mstrHeaderProteinOptionsList:
						mshortColNumProteinOptionsList = (short) i ; 
						break ;
					case  mstrHeaderArchivePath:
						mshortColNumArchivePath = (short) i ;
						break ;
					default:
						break ; 
				}
			}
			if (mshortColNumDataset == -1)
			{
				throw new System.Exception(mstrHeaderDataset + " column is missing. The following columns are needed: " 
					+ mstrHeaderDataset + "," + mstrHeaderArchivePath + "," + mstrHeaderTool + "," + mstrHeaderJobNum) ; 
			}
			if (mshortColNumArchivePath == -1)
			{
				throw new System.Exception(mstrHeaderArchivePath + " column is missing. The following columns are needed: " 
					+ mstrHeaderDataset + "," + mstrHeaderArchivePath + "," + mstrHeaderTool + "," + mstrHeaderJobNum) ; 
			}
			if (mshortColNumAnalysisTool == -1)
			{
				throw new System.Exception(mstrHeaderTool + " column is missing. The following columns are needed: " 
					+ mstrHeaderDataset + "," + mstrHeaderArchivePath + "," + mstrHeaderTool + "," + mstrHeaderJobNum) ; 
			}
			if (mshortColNumDMSJobNum == -1)
			{
				throw new System.Exception(mstrHeaderJobNum + " column is missing. The following columns are needed: " 
					+ mstrHeaderDataset + "," + mstrHeaderArchivePath + "," + mstrHeaderTool + "," + mstrHeaderJobNum) ; 
			}
		}
	}
}

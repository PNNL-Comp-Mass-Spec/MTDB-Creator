using System;
using System.IO ; 
using System.Collections ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsXTandemResults.
	/// </summary>
	public class clsSequestResults
	{
		public int mint_HitNum ; 
		public int mint_ScanNum ; 
		public short mshort_ScanCount ; 
		public short mshort_ChargeState	; 
		public double mdbl_MH ;
		public double mdbl_XCorr ; 
		public double mdbl_DelCn ;
		public double mdbl_Sp ;
		public string mstr_Reference ; 
		public short mshort_MultiProtein ; 				
		public string mstr_Peptide ; 
		public double mdbl_DelCn2 ; 
		public short mshort_RankSp ; 
		public short mshort_RankXc ; 
		public double mdbl_DelM ; 
		public double mdbl_XcRatio ; 
		public bool mbln_PassFilt ; 
		public double mdbl_MScore ; 
		public short mshort_NumTrypticEnds ; 

		public double mdbl_observed_net ; 
		public string mstr_clean_peptide ; 

		
		public string mstrHeader_HitNum = "" ; 
		public string mstrHeader_ScanNum = "" ; 
		public string mstrHeader_ScanCount = "" ; 
		public string mstrHeader_ChargeState	= "" ; 
		public string mstrHeader_MH = "" ;
		public string mstrHeader_XCorr = "" ; 
		public string mstrHeader_DelCn = "" ;
		public string mstrHeader_Sp = "" ;
		public string mstrHeader_Reference = "" ; 
		public string mstrHeader_MultiProtein = "" ; 				
		public string mstrHeader_Peptide = "" ; 
		public string mstrHeader_DelCn2 = "" ; 
		public string mstrHeader_RankSp = "" ; 
		public string mstrHeader_RankXc = "" ; 
		public string mstrHeader_DelM = "" ; 
		public string mstrHeader_XcRatio = "" ; 
		public string mstrHeader_PassFilt = "" ; 
		public string mstrHeader_MScore = "" ; 
		public string mstrHeader_NumTrypticEnds = "" ; 

		public static short mshortColNum_HitNum = 0 ; 
		public static short mshortColNum_ScanNum = 1 ; 
		public static short mshortColNum_ScanCount = 2 ; 
		public static short mshortColNum_ChargeState = 3 ; 
		public static short mshortColNum_MH = 4 ;
		public static short mshortColNum_XCorr = 5 ; 
		public static short mshortColNum_DelCn = 6 ;
		public static short mshortColNum_Sp = 7 ;
		public static short mshortColNum_Reference = 8 ; 
		public static short mshortColNum_MultiProtein = 9 ; 				
		public static short mshortColNum_Peptide = 10 ; 
		public static short mshortColNum_DelCn2 = 11 ; 
		public static short mshortColNum_RankSp = 12 ; 
		public static short mshortColNum_RankXc = 13 ; 
		public static short mshortColNum_DelM = 14 ; 
		public static short mshortColNum_XcRatio = 15 ; 
		public static short mshortColNum_PassFilt = 16 ; 
		public static short mshortColNum_MScore = 17 ; 
		public static short mshortColNum_NumTrypticEnds = 18 ; 


		public static string mstrDefaultHeader ="HitNum	ScanNum	ScanCount	ChargeState	MH	XCorr	DelCn	Sp	Reference	MultiProtein	Peptide	DelCn2	RankSp	RankXc	DelM	XcRatio	PassFilt	MScore	NumTrypticEnds" ; 

		public static void SetHeaderNames()
		{
			string []colNames = mstrDefaultHeader.Split(new char[]{'\t'}) ; 

			mstrHeader_HitNum = colNames[mshortColNum_HitNum] ; 
			mstrHeader_ScanNum = colNames[mshortColNum_ScanNum] ; 
			mstrHeader_ScanCount = colNames[mshortColNum_ScanCount] ; 
			mstrHeader_ChargeState	= colNames[mshortColNum_ChargeState] ; 
			mstrHeader_MH = colNames[mshortColNum_MH] ;
			mstrHeader_XCorr = colNames[mshortColNum_XCorr] ; 
			mstrHeader_DelCn = colNames[mshortColNum_DelCn] ;
			mstrHeader_Sp = colNames[mshortColNum_Sp] ;
			mstrHeader_Reference = colNames[mshortColNum_Reference] ; 
			mstrHeader_MultiProtein = colNames[mshortColNum_MultiProtein] ; 				
			mstrHeader_Peptide = colNames[mshortColNum_Peptide] ; 
			mstrHeader_DelCn2 = colNames[mshortColNum_DelCn2] ; 
			mstrHeader_RankSp = colNames[mshortColNum_RankSp] ; 
			mstrHeader_RankXc = colNames[mshortColNum_RankXc] ; 
			mstrHeader_DelM = colNames[mshortColNum_DelM] ; 
			mstrHeader_XcRatio = colNames[mshortColNum_XcRatio] ; 
			mstrHeader_PassFilt = colNames[mshortColNum_PassFilt] ; 
			mstrHeader_MScore = colNames[mshortColNum_MScore] ; 
			mstrHeader_NumTrypticEnds = colNames[mshortColNum_NumTrypticEnds] ; 

		}

		public static void SetHeaderColumns(string headerLine, char [] delimiters)
		{
			string [] colNames = headerLine.Split(delimiters) ; 
			for (int i = 0 ; i < colNames.Length ; i++)
			{
				if (colNames[i]	== mstrHeader_result_id)
					mshortColNum_result_id = (short) i ;
				else if (colNames[i] == mstrHeader_HitNum)
					mshortColNum_HitNum	= (short) i	; 
				else if (colNames[i] == mstrHeader_ScanNum)
					mshortColNum_ScanNum = (short) i ; 
				else if (colNames[i] == mstrHeader_ScanCount)
					mshortColNum_ScanCount = (short) i ; 
				else if (colNames[i] == mshortColNum_ChargeState)
					mshortColNum_ChargeState = (short) i ; 
				else if (colNames[i] == mstrHeader_MH)
					mshortColNum_MH	= (short) i	;
				else if (colNames[i] == mstrHeader_XCorr)
					mshortColNum_XCorr = (short) i ; 
				else if (colNames[i] == mstrHeader_DelCn)
					mshortColNum_DelCn = (short) i ;
				else if (colNames[i] == mstrHeader_Sp)
					mshortColNum_Sp	= (short) i	;
				else if (colNames[i] == mstrHeader_Reference)
					mshortColNum_Reference = (short) i ; 
				else if (colNames[i] == mstrHeader_MultiProtein)
					mshortColNum_MultiProtein =	(short)	i ;					
				else if (colNames[i] == mstrHeader_Peptide)
					mshortColNum_Peptide = (short) i ; 
				else if (colNames[i] == mstrHeader_DelCn2)
					mshortColNum_DelCn2	= (short) i	; 
				else if (colNames[i] == mstrHeader_RankSp)
					mshortColNum_RankSp	= (short) i	; 
				else if (colNames[i] == mstrHeader_RankXc)
					mshortColNum_RankXc	= (short) i	; 
				else if (colNames[i] == mstrHeader_DelM)
					mshortColNum_DelM =	(short)	i ;	
				else if (colNames[i] == mstrHeader_XcRatio)
					mshortColNum_XcRatio = (short) i ; 
				else if (colNames[i] == mstrHeader_PassFilt)
					mshortColNum_PassFilt =	(short)	i ;	
				else if (colNames[i] == mstrHeader_MScore)
					mshortColNum_MScore	= (short) i	; 
				else if (colNames[i] == mstrHeader_NumTrypticEnds)
					mshortColNum_NumTrypticEnds	= (short) i	; 
			}
		}
		

		
		public clsSequestResults(string line, char [] delimiters)
		{
			//
			// TODO: Add constructor logic here
			//
			string [] column = line.Split(delimiters) ; 
			mint_HitNum = Convert.ToInt32(column[mshortColNum_HitNum]); 
			mint_ScanNum = Convert.ToInt32(column[mshortColNum_ScanNum]); 
			mshort_ScanCount = Convert.ToInt16(column[mshortColNum_ScanCount]); 
			mshort_ChargeState	= Convert.ToInt16(column[mshortColNum_ChargeState]); 
			mdbl_MH = Convert.ToDouble(column[mshortColNum_MH]);
			mdbl_XCorr = Convert.ToDouble(column[mshortColNum_XCorr]); 
			mdbl_DelCn = Convert.ToDouble(column[mshortColNum_DelCn]);
			mdbl_Sp = Convert.ToDouble(column[mshortColNum_Sp]);
			mstr_Reference = column[mshortColNum_Reference] ; 
			mshort_MultiProtein = Convert.ToInt16(column[mshortColNum_MultiProtein]); 				
			mstr_Peptide = column[mshortColNum_Peptide]; 
			mdbl_DelCn2 = Convert.ToDouble(column[mshortColNum_DelCn2]); 
			mshort_RankSp = Convert.ToDouble(column[mshortColNum_RankSp]); 
			mshort_RankXc = Convert.To(column[mshortColNum_RankXc]); 
			mdbl_DelM = Convert.ToDouble(column[mshortColNum_DelM]); 
			mdbl_XcRatio = Convert.ToDouble(column[mshortColNum_XcRatio]); 
			mbln_PassFilt = Convert.ToBoolean(column[mshortColNum_PassFilt]); 
			mdbl_MScore = Convert.ToDouble(column[mshortColNum_MScore]); 
			mshort_NumTrypticEnds = Convert.ToInt16(column[mshortColNum_NumTrypticEnds]); 
		}

		private string CleanPeptide(string peptide)
		{
			char [] peptideChar = peptide.ToCharArray() ; 
			int startIndex = 2 ; 
			int stopIndex = peptideChar.Length - 3 ; 
			if (peptideChar[1] != '.')
			{
				startIndex = 0 ; 
			}
			if (peptideChar[stopIndex+1] != '.')
			{
				stopIndex = peptideChar.Length-1 ; 
			}

			int copyToIndex = startIndex ; 
			int copyFromIndex = startIndex ; 

			while (copyFromIndex <= stopIndex)
			{
				while (copyFromIndex <= stopIndex && (peptideChar[copyFromIndex] == '&' || peptideChar[copyFromIndex] == '*'))
				{
					copyFromIndex++ ; 
				}
				if (copyFromIndex > stopIndex)
					break ; 
				peptideChar[copyToIndex++] = peptideChar[copyFromIndex++] ; 
			}

			string cleanPeptide = new string(peptideChar, startIndex, copyToIndex-startIndex) ; 
			return cleanPeptide ; 
		}

	}

	public class clsSequestResultsReader
	{
		private int mintPercentRead ; 
		private frmStatus.dlgSetPercentComplete mevntPercentComplete ; 
		private frmStatus.dlgSetStatusMessage mevntStatusMessage ; 


		public clsSequestResultsReader(frmStatus statusForm)
		{
			mintPercentRead = 0 ; 
			mevntPercentComplete = new MTDBCreator.frmStatus.dlgSetPercentComplete(statusForm.SetPrecentComplete) ; 
			mevntStatusMessage = new MTDBCreator.frmStatus.dlgSetStatusMessage(statusForm.SetStatusMessage) ; 
		}

		public int PercentDone
		{
			get
			{
				return mintPercentRead ; 
			}
		}
		public clsSequestResults[] ReadSequestFile(string fileName)
		{
			ArrayList arrSequestResults = new ArrayList() ; 
			try 
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				FileInfo fInfo = new FileInfo(fileName) ; 
				long totalLength = fInfo.Length ; 
				using (StreamReader sr = new StreamReader(fileName)) 
				{
					mevntStatusMessage("Loading SEQUEST results file" ) ;
					char [] delimiters = {'\t'} ; 
					string headerLine = sr.ReadLine() ; 
					clsSequestResults.SetHeaderNames() ; 
					clsSequestResults.SetHeaderColumns(headerLine, delimiters) ; 
					string line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					long numRead = 0 ; 
					while ((line = sr.ReadLine()) != null) 
					{
						numRead += line.Length; 
						mintPercentRead = Convert.ToInt32((numRead*100)/totalLength) ; 
						if (numRead % 100 == 0)
							mevntPercentComplete(mintPercentRead) ; 
						arrSequestResults.Add(new clsSequestResults(line, delimiters)) ; 
					}
				}
			}
			catch (Exception e) 
			{
				// Let the user know what went wrong.
				System.Windows.Forms.MessageBox.Show(e.Message + e.StackTrace);
			}
			return (clsSequestResults []) arrSequestResults.ToArray(typeof(clsSequestResults)) ; 
		}
	}
}

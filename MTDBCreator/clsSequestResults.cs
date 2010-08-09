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
		public double mdbl_FScore ; 
		public double mdbl_MScore ; 
		public short mshort_NumTrypticEnds ; 

		public double mdbl_observed_net ; 
		public string mstr_clean_peptide ; 

		
		private static string mstrHeader_HitNum = "" ; 
		private static string mstrHeader_ScanNum = "" ; 
		private static string mstrHeader_ScanCount = "" ; 
		private static string mstrHeader_ChargeState	= "" ; 
		private static string mstrHeader_MH = "" ;
		private static string mstrHeader_XCorr = "" ; 
		private static string mstrHeader_DelCn = "" ;
		private static string mstrHeader_Sp = "" ;
		private static string mstrHeader_Reference = "" ; 
		private static string mstrHeader_MultiProtein = "" ; 				
		private static string mstrHeader_Peptide = "" ; 
		private static string mstrHeader_DelCn2 = "" ; 
		private static string mstrHeader_RankSp = "" ; 
		private static string mstrHeader_RankXc = "" ; 
		private static string mstrHeader_DelM = "" ; 
		private static string mstrHeader_XcRatio = "" ; 
		private static string mstrHeader_PassFilt = "" ; 
		private static string mstrHeader_MScore = "" ; 
		private static string mstrHeader_NumTrypticEnds = "" ; 

		private static short mshortColNum_HitNum = 0 ; 
		private static short mshortColNum_ScanNum = 1 ; 
		private static short mshortColNum_ScanCount = 2 ; 
		private static short mshortColNum_ChargeState = 3 ; 
		private static short mshortColNum_MH = 4 ;
		private static short mshortColNum_XCorr = 5 ; 
		private static short mshortColNum_DelCn = 6 ;
		private static short mshortColNum_Sp = 7 ;
		private static short mshortColNum_Reference = 8 ; 
		private static short mshortColNum_MultiProtein = 9 ; 				
		private static short mshortColNum_Peptide = 10 ; 
		private static short mshortColNum_DelCn2 = 11 ; 
		private static short mshortColNum_RankSp = 12 ; 
		private static short mshortColNum_RankXc = 13 ; 
		private static short mshortColNum_DelM = 14 ; 
		private static short mshortColNum_XcRatio = 15 ; 
		private static short mshortColNum_PassFilt = 16 ; 
		private static short mshortColNum_MScore = 17 ; 
		private static short mshortColNum_NumTrypticEnds = 18 ; 


		public static string mstrDefaultHeader ="HitNum	ScanNum	ScanCount	ChargeState	MH	XCorr	DelCn	Sp	Reference	MultiProtein	Peptide	DelCn2	RankSp	RankXc	DelM	XcRatio	PassFilt	MScore	NumTrypticEnds" ; 

		public const short MAX_CHARGE_FOR_FSCORE = 3 ;
		static readonly double [] consts = {0.646, -0.959, -1.460, -0.959, -0.959} ;
		static readonly double [] xcorrs = {5.49, 8.362, 9.933, 8.362, 8.362} ;
		static readonly double [] deltas = {4.643, 7.386, 11.149, 7.386, 7.386} ;
		static readonly double [] ranks = {-0.455, -0.194, -0.201, -0.194, -0.194} ;
		static readonly double [] massdiffs =  {-0.84, -0.314, -0.277, -0.314, -0.314} ;
		static readonly int [] max_pep_lens = {100, 15, 25, 50, 50} ;
		static readonly int [] num_frags = {2, 2, 4, 6, 6} ;

		public static double CalculatePeptideProphetDistriminantScore(clsSequestResults result)
		{
			short charge_ = result.mshort_ChargeState ; 
			if (charge_ > MAX_CHARGE_FOR_FSCORE)
				charge_ = MAX_CHARGE_FOR_FSCORE ; 
			double const_ = consts[charge_-1];
			double xcorr_p_wt_ = xcorrs[charge_-1];
			double delta_wt_ = deltas[charge_-1];
			double log_rank_wt_ = ranks[charge_-1];
			double abs_massd_wt_ = massdiffs[charge_-1];
			int max_pep_len_ = max_pep_lens[charge_-1];
			int num_frags_ = num_frags[charge_-1];

			int eff_pep_len = result.mstr_clean_peptide.Length ;
			if(eff_pep_len > max_pep_len_)
				eff_pep_len = max_pep_len_;
			double lg_xcorr = Math.Log(result.mdbl_XcRatio) ; 
			double lg_eff_len = Math.Log((float)(1.0*eff_pep_len * num_frags_)) ; 
			double adjustedXCorr = lg_xcorr / lg_eff_len ;

			double tot = const_;
			tot += xcorr_p_wt_ * adjustedXCorr ;
			tot += delta_wt_ * result.mdbl_DelCn2 ;
			double lg_val = Math.Log(1.0*result.mshort_RankSp) ; 
			tot += log_rank_wt_ * lg_val ;
			tot += abs_massd_wt_ * Math.Abs(result.mdbl_DelM) ;	
			return tot ;
		}
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
				if (colNames[i] == mstrHeader_HitNum)
					mshortColNum_HitNum	= (short) i	; 
				else if (colNames[i] == mstrHeader_ScanNum)
					mshortColNum_ScanNum = (short) i ; 
				else if (colNames[i] == mstrHeader_ScanCount)
					mshortColNum_ScanCount = (short) i ; 
				else if (colNames[i] == mstrHeader_ChargeState)
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
			try
			{
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
				mstr_clean_peptide = CleanPeptide(mstr_Peptide) ;  
				mdbl_DelCn2 = Convert.ToDouble(column[mshortColNum_DelCn2]); 
				mshort_RankSp = Convert.ToInt16(column[mshortColNum_RankSp]); 
				mshort_RankXc = Convert.ToInt16(column[mshortColNum_RankXc]); 
				mdbl_DelM = Convert.ToDouble(column[mshortColNum_DelM]); 
				mdbl_XcRatio = Convert.ToDouble(column[mshortColNum_XcRatio]); 
				mbln_PassFilt = Convert.ToBoolean(Convert.ToInt16(column[mshortColNum_PassFilt])); 
				mdbl_MScore = Convert.ToDouble(column[mshortColNum_MScore]); 
				mshort_NumTrypticEnds = Convert.ToInt16(column[mshortColNum_NumTrypticEnds]); 
				mdbl_FScore = CalculatePeptideProphetDistriminantScore(this) ; 
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message + ex.StackTrace) ; 
			}
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

	public class clsSequestResultsReader: ProcessorBase
    {
        private const int CONST_MAX_ATTEMPTS = 3;
		private int mintPercentRead ; 

		public clsSequestResultsReader()
		{
			mintPercentRead = 0 ; 
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
            int attempts = 0;
            bool success = false;
			
			try 
			{
                while (success == false)
                {
                    attempts++;
                    try
                    {
                        // Create an instance of StreamReader to read from a file.
                        // The using statement also closes the StreamReader.
                        FileInfo fInfo = new FileInfo(fileName);
                        long totalLength = fInfo.Length;
                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            StatusMessage("Loading SEQUEST results file");
                            char[] delimiters = { '\t' };
                            string headerLine = sr.ReadLine();
                            clsSequestResults.SetHeaderNames();
                            clsSequestResults.SetHeaderColumns(headerLine, delimiters);
                            string line;
                            // Read and display lines from the file until the end of 
                            // the file is reached.
                            long numRead = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                numRead += line.Length;
                                mintPercentRead = Convert.ToInt32((numRead * 100) / totalLength);
                                if (numRead % 100 == 0)
                                    PercentComplete(mintPercentRead);
                                arrSequestResults.Add(new clsSequestResults(line, delimiters));
                            }
                        }
                        success = true;
                    }
                    catch (System.IO.IOException ioException)
                    {
                        // Let the user know what went wrong.
                        ErrorMessage("Error reading ReadSequestFile file: " + ioException.Message);
                        Console.WriteLine("Error reading ReadSequestFile file: " + ioException.Message + ioException.StackTrace);
                        if (attempts > CONST_MAX_ATTEMPTS)
                        {
                            throw ioException;
                        }
                    }
                }
			}
			catch (Exception ex) 
			{
				// Let the user know what went wrong.
				ErrorMessage("Error reading Sequest results file: " + ex.Message ) ;
                throw ex;
			}
			return (clsSequestResults []) arrSequestResults.ToArray(typeof(clsSequestResults)) ; 
		}
	}
}

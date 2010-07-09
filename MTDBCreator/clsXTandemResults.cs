using System;
using System.IO ; 
using System.Collections ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsXTandemResults.
	/// </summary>
	public class clsXTandemResults
	{
		public int mint_result_id ; 
		public int mint_group_id ; 
		public int mint_scan ; 
		public double mdbl_observed_net ; 
		public short mshort_charge ; 
		public double mdbl_peptide_mh ;
		public double mdbl_peptide_hyperscore ; 
		public double mdbl_log_peptide_e_value ;
		public short mshort_multi_protein_count ; 
		public string mstr_peptide_sequence ; 
		public string mstr_clean_peptide ; 
		public double mdbl_deltaCN2 ; 
		public double mdbl_y_score ; 
		public short mshort_num_y_ions ; 
		public double mdbl_b_score ; 
		public short mshort_num_b_ions ; 
		public double mdbl_delta_mass ; 
		public double mdbl_log_intensity ; 
		public short mshort_tryptic_state ; 

		private static string mstrHeader_result_id= "" ;
		private static string mstrHeader_group_id= "" ; 
		private static string mstrHeader_scan= "" ; 
		private static string mstrHeader_charge= "" ; 
		private static string mstrHeader_peptide_mh= "" ; 
		private static string mstrHeader_peptide_hyperscore= "" ; 
		private static string mstrHeader_log_peptide_e_value= "" ; 
		private static string mstrHeader_multi_protein_count= "" ; 
		private static string mstrHeader_peptide_sequence= "" ; 
		private static string mstrHeader_deltaCN2= "" ; 
		private static string mstrHeader_y_score= "" ; 
		private static string mstrHeader_num_y_ions= "" ; 
		private static string mstrHeader_b_score= "" ; 
		private static string mstrHeader_num_b_ions= "" ; 
		private static string mstrHeader_delta_mass= "" ; 
		private static string mstrHeader_log_intensity= "" ; 


		private static short mshortColNum_result_id = 0 ;
		private static short mshortColNum_group_id = 1 ; 
		private static short mshortColNum_scan = 2 ; 
		private static short mshortColNum_charge = 3 ; 
		private static short mshortColNum_peptide_mh = 4 ; 
		private static short mshortColNum_peptide_hyperscore = 5 ; 
		private static short mshortColNum_log_peptide_e_value = 6 ; 
		private static short mshortColNum_multi_protein_count = 7 ; 
		private static short mshortColNum_peptide_sequence = 8 ; 
		private static short mshortColNum_deltaCN2 = 9 ; 
		private static short mshortColNum_y_score = 10 ; 
		private static short mshortColNum_num_y_ions = 11 ; 
		private static short mshortColNum_b_score = 12 ; 
		private static short mshortColNum_num_b_ions = 13 ; 
		private static short mshortColNum_delta_mass = 14 ; 
		private static short mshortColNum_log_intensity = 15 ; 

		public static string mstrDefaultHeader ="Result_ID	Group_ID	Scan	Charge	Peptide_MH	Peptide_Hyperscore	Peptide_Expectation_Value_Log(e)	Multiple_Protein_Count	Peptide_Sequence	DeltaCn2	y_score	y_ions	b_score	b_ions	Delta_Mass	Peptide_Intensity_Log(I)" ; 

		public static void SetHeaderNames()
		{
			string []colNames = mstrDefaultHeader.Split(new char[]{'\t'}) ; 
			mstrHeader_result_id = colNames[mshortColNum_result_id] ;
			mstrHeader_group_id = colNames[mshortColNum_group_id] ; 
			mstrHeader_scan = colNames[mshortColNum_scan] ; 
			mstrHeader_charge = colNames[mshortColNum_charge] ; 
			mstrHeader_peptide_mh = colNames[mshortColNum_peptide_mh] ; 
			mstrHeader_peptide_hyperscore = colNames[mshortColNum_peptide_hyperscore] ; 
			mstrHeader_log_peptide_e_value = colNames[mshortColNum_log_peptide_e_value] ; 
			mstrHeader_multi_protein_count = colNames[mshortColNum_multi_protein_count] ; 
			mstrHeader_peptide_sequence = colNames[mshortColNum_peptide_sequence] ; 
			mstrHeader_deltaCN2 = colNames[mshortColNum_deltaCN2] ; 
			mstrHeader_y_score = colNames[mshortColNum_y_score] ; 
			mstrHeader_num_y_ions = colNames[mshortColNum_num_y_ions] ; 
			mstrHeader_b_score = colNames[mshortColNum_b_score] ; 
			mstrHeader_num_b_ions = colNames[mshortColNum_num_b_ions] ; 
			mstrHeader_delta_mass = colNames[mshortColNum_delta_mass] ; 
			mstrHeader_log_intensity = colNames[mshortColNum_log_intensity] ; 
		}

		public static void SetHeaderColumns(string headerLine, char [] delimiters)
		{
			string [] colNames = headerLine.Split(delimiters) ; 
			for (int i = 0 ; i < colNames.Length ; i++)
			{
				if (colNames[i]	== mstrHeader_result_id)
					mshortColNum_result_id = (short) i ;
				else if	(colNames[i] == mstrHeader_group_id)
					mshortColNum_group_id =	(short)	i ;
				else if	(colNames[i] == mstrHeader_scan)
					mshortColNum_scan =	(short)	i ;
				else if	(colNames[i] == mstrHeader_charge)
					mshortColNum_charge	= (short) i	;
				else if	(colNames[i] == mstrHeader_peptide_mh)
					mshortColNum_peptide_mh	= (short) i	;
				else if	(colNames[i] == mstrHeader_peptide_hyperscore)
					mshortColNum_peptide_hyperscore	= (short) i	;
				else if	(colNames[i] == mstrHeader_log_peptide_e_value)
					mshortColNum_log_peptide_e_value = (short) i ;
				else if	(colNames[i] == mstrHeader_multi_protein_count)
					mshortColNum_multi_protein_count = (short) i ;
				else if	(colNames[i] == mstrHeader_peptide_sequence)
					mshortColNum_peptide_sequence =	(short)	i ;
				else if	(colNames[i] == mstrHeader_deltaCN2)
					mshortColNum_deltaCN2	= (short) i	;
				else if	(colNames[i] == mstrHeader_y_score)
					mshortColNum_y_score = (short) i ;
				else if	(colNames[i] == mstrHeader_num_y_ions)
					mshortColNum_num_y_ions	= (short) i	;
				else if	(colNames[i] == mstrHeader_b_score)
					mshortColNum_b_score = (short) i ;
				else if	(colNames[i] == mstrHeader_num_b_ions)
					mshortColNum_num_b_ions	= (short) i	;
				else if	(colNames[i] == mstrHeader_delta_mass)
					mshortColNum_delta_mass	= (short) i	;
				else if	(colNames[i] == mstrHeader_log_intensity)
					mshortColNum_log_intensity = (short) i ;
			}
		}
		
		public clsXTandemResults(string line, char [] delimiters)
		{
			string [] column = line.Split(delimiters) ; 
			mint_result_id = Convert.ToInt32(column[mshortColNum_result_id]) ;  
			mint_group_id = Convert.ToInt32(column[mshortColNum_group_id]) ;  
			mint_scan = Convert.ToInt32(column[mshortColNum_scan]) ;  
			mshort_charge = Convert.ToInt16(column[mshortColNum_charge]) ;  
			mdbl_peptide_mh = Convert.ToDouble(column[mshortColNum_peptide_mh]) ; 
			mdbl_peptide_hyperscore = Convert.ToDouble(column[mshortColNum_peptide_hyperscore]) ;  
			mdbl_log_peptide_e_value = Convert.ToDouble(column[mshortColNum_log_peptide_e_value]) ; 
			mshort_multi_protein_count = Convert.ToInt16(column[mshortColNum_multi_protein_count]) ;  
			mstr_peptide_sequence = column[mshortColNum_peptide_sequence] ;  
			mstr_clean_peptide = CleanPeptide(mstr_peptide_sequence) ;  
			mdbl_deltaCN2 = Convert.ToDouble(column[mshortColNum_deltaCN2]) ;  
			mdbl_y_score = Convert.ToDouble(column[mshortColNum_y_score]) ;  
			mshort_num_y_ions = Convert.ToInt16(column[mshortColNum_num_y_ions]) ;  
			mdbl_b_score = Convert.ToDouble(column[mshortColNum_b_score]) ;  
			mshort_num_b_ions = Convert.ToInt16(column[mshortColNum_num_b_ions]) ;  
			mdbl_delta_mass = Convert.ToDouble(column[mshortColNum_delta_mass]) ;  
			mdbl_log_intensity = Convert.ToDouble(column[mshortColNum_log_intensity]) ;  
			mshort_tryptic_state = CalculateTrypticState(mstr_peptide_sequence) ; 
		}

		private static string CleanPeptide(string peptide)
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
		private static short CalculateTrypticState(string peptide)
		{
			short trypticState = 0 ; 
			char [] peptideChar = peptide.ToCharArray() ; 
			int startIndex = 2 ; 
			int stopIndex = peptideChar.Length - 3 ; 

			if (peptideChar[1] != '.')
			{
				startIndex = 0 ;
				throw new ApplicationException("Peptide " + peptide + " does not have a . in the second position") ; 
			}

			if (peptideChar[stopIndex+1] != '.')
			{
				stopIndex = peptideChar.Length-1 ; 
				throw new ApplicationException("Peptide " + peptide + " does not have a . in the second last position") ; 
			}

			if (peptideChar[stopIndex] == 'R' || peptideChar[stopIndex] == 'K')
			{
				trypticState++ ; 
				if (peptideChar[peptideChar.Length-1] == 'P')
				{
					trypticState-- ; 
				}
			}
			else if (!Char.IsLetter(peptideChar[stopIndex]))
			{
				if (peptideChar[stopIndex-1] == 'R' || peptideChar[stopIndex-1] == 'K')
				{
					trypticState++ ; 
					if (peptideChar[peptideChar.Length-1] == 'P')
					{
						trypticState-- ; 
					}
				}
			}

			if (peptideChar[peptideChar.Length-1] == '-' && trypticState == 0)
			{
				trypticState++ ; 
			}

			if (peptideChar[0] == 'R' || peptideChar[0] == 'K')
			{
				trypticState++ ; 
				if (peptideChar[startIndex] == 'P')
				{
					trypticState-- ; 
				}
			}
			else if (peptideChar[0]=='-')
			{
				trypticState++ ; 
			}

			return trypticState ; 
		}

	}



	public class clsXTandemResultsReader: ProcessorBase
	{
		private int mintPercentRead ; 

		public clsXTandemResultsReader()
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
		public clsXTandemResults[] ReadXTandemFile(string fileName)
		{
			ArrayList arrXTandemResults = new ArrayList() ; 
			try 
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				FileInfo fInfo = new FileInfo(fileName) ; 
				long totalLength = fInfo.Length ; 
				using (StreamReader sr = new StreamReader(fileName)) 
				{
					StatusMessage("Loading XTandem results file" ) ;
					char [] delimiters = {'\t'} ; 
					string headerLine = sr.ReadLine() ; 
					clsXTandemResults.SetHeaderNames() ; 
					clsXTandemResults.SetHeaderColumns(headerLine, delimiters) ; 
					string line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					long numRead = 0 ; 
					while ((line = sr.ReadLine()) != null) 
					{
						numRead += line.Length; 
						mintPercentRead = Convert.ToInt32((numRead*100)/totalLength) ; 
						if (numRead % 100 == 0)
							PercentComplete(mintPercentRead) ; 
						arrXTandemResults.Add(new clsXTandemResults(line, delimiters)) ; 
					}
				}
			}
			catch (Exception ex) 
			{
				// Let the user know what went wrong.
				ErrorMessage("Error reading XTandem results file: " + ex.Message ) ;				
			}
			return (clsXTandemResults []) arrXTandemResults.ToArray(typeof(clsXTandemResults)) ; 
		}
	}
}

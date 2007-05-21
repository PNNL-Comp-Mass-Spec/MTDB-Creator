using System;
using System.Collections ; 
using System.IO ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsSeqToProteinMap.
	/// </summary>
	public class clsSeqToProteinMap
	{
		public int mint_unique_seq_id ; 
		public short mshort_cleavage_state ; 
		public short mshort_terminus_state ; 
		public string mstr_protein_name ; 
		public double mdbl_protein_e_value ;
		public double mdbl_protein_intensity_log ; 

		private const string mstrHeader_unique_seq_id = "Unique_Seq_ID" ; 
		private const string mstrHeader_cleavage_state = "Cleavage_State"; 
		private const string mstrHeader_terminus_state = "Terminus_State" ; 
		private const string mstrHeader_protein_name = "Protein_Name" ;
		private const string mstrHeader_protein_e_value = "Protein_Expectation_Value_Log(e)" ;
		private const string mstrHeader_protein_intensity_log = "Protein_Intensity_Log(I)" ;

		private static short mshortColNum_unique_seq_id = 0 ;
		private static short mshortColNum_cleavage_state = 1 ; 
		private static short mshortColNum_terminus_state = 2 ; 
		private static short mshortColNum_protein_name = 3 ; 
		private static short mshortColNum_protein_e_value = 4 ; 
		private static short mshortColNum_protein_intensity_log = 5 ; 

		public clsSeqToProteinMap(string line, char [] delimiters)
		{
			//
			// TODO: Add constructor logic here
			//
			string [] columns = line.Split(delimiters) ; 

			mint_unique_seq_id = Convert.ToInt32(columns[mshortColNum_unique_seq_id]);
			mshort_cleavage_state = Convert.ToInt16(columns[mshortColNum_cleavage_state]) ;
			mshort_terminus_state = Convert.ToInt16(columns[mshortColNum_terminus_state]) ;
			mstr_protein_name = columns[mshortColNum_protein_name] ;
			mdbl_protein_e_value = Convert.ToDouble(columns[mshortColNum_protein_e_value]) ;
			mdbl_protein_intensity_log = Convert.ToDouble(columns[mshortColNum_protein_intensity_log]) ;
		}

		public static void SetHeaderColumns(string headerLine, char [] delimiters)
		{
			string [] colHeaders = headerLine.Split(delimiters) ; 
			for (int i = 0 ; i < colHeaders.Length ; i++)
			{
				switch(colHeaders[i])
				{
					case  mstrHeader_unique_seq_id :
						mshortColNum_unique_seq_id = (short) i ; 
						break ;
					case  mstrHeader_cleavage_state :
						mshortColNum_cleavage_state = (short) i ; 
						break ;
					case  mstrHeader_terminus_state :
						mshortColNum_terminus_state = (short) i ; 
						break ;
					case  mstrHeader_protein_name :
						mshortColNum_protein_name = (short) i ; 
						break ;
					case  mstrHeader_protein_e_value :
						mshortColNum_protein_e_value = (short) i ; 
						break ;
					case  mstrHeader_protein_intensity_log :
						mshortColNum_protein_intensity_log = (short) i ; 
						break ;
					default:
						break ; 
				}
			}
		}
	}

	public class clsSeqToProteinMapReader
	{
		private int mintPercentRead ; 
		private frmStatus.dlgSetPercentComplete mevntPercentComplete ; 
		private frmStatus.dlgSetStatusMessage mevntStatusMessage ; 
		

		public clsSeqToProteinMapReader(frmStatus statusForm)
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
		public clsSeqToProteinMap[] ReadSeqToProteinMapFile(string fileName)
		{
			ArrayList arrSeqToProteinMaps = new ArrayList() ; 
			try 
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				FileInfo fInfo = new FileInfo(fileName) ; 
				long totalLength = fInfo.Length ; 
				using (StreamReader sr = new StreamReader(fileName)) 
				{
					char [] delimiters = {'\t'} ; 
					string headerLine = sr.ReadLine() ; 
					clsSeqToProteinMap.SetHeaderColumns(headerLine, delimiters) ; 
					string line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					long numRead = 0 ; 
					mevntStatusMessage("Loading SeqToProteinMap file" ) ;
					while ((line = sr.ReadLine()) != null) 
					{
						numRead += line.Length; 
						mintPercentRead = Convert.ToInt32((numRead*100)/totalLength) ; 
						if (numRead % 100 == 0)
							mevntPercentComplete(mintPercentRead) ; 
						arrSeqToProteinMaps.Add(new clsSeqToProteinMap(line, delimiters)) ; 
					}
				}
			}
			catch (Exception e) 
			{
				throw e ; 
			}
			return (clsSeqToProteinMap []) arrSeqToProteinMaps.ToArray(typeof(clsSeqToProteinMap)) ; 
		}
	}
}

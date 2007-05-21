using System;
using System.Collections ; 
using System.IO ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsSeqInfo.
	/// </summary>
	public class clsSeqInfo
	{
		private static string mstrDefaultHeader ="Unique_Seq_ID	Mod_Count	Mod_Description	Monoisotopic_Mass" ; 

		public int mint_unique_seq_id ; 
		public short mshort_mod_count ; 
		public string mstr_mod_description ; 
		public double mdbl_mono_mass ; 

		private static string mstrHeader_unique_seq_id = "" ;
		private static string mstrHeader_mod_count = "" ; 
		private static string mstrHeader_mod_description = "" ; 
		private static string mstrHeader_mono_mass = "" ; 

		private static short mshortColNum_unique_seq_id = 0 ; 
		private static short mshortColNum_mod_count = 1 ; 
		private static short mshortColNum_mod_description = 2 ; 
		private static short mshortColNum_mono_mass = 3 ; 

		public static void SetHeaderNames()
		{
			string []colNames = mstrDefaultHeader.Split(new char[]{'\t'}) ; 
			mstrHeader_unique_seq_id = colNames[mshortColNum_unique_seq_id] ;
			mstrHeader_mod_count = colNames[mshortColNum_mod_count] ; 
			mstrHeader_mod_description = colNames[mshortColNum_mod_description] ; 
			mstrHeader_mono_mass = colNames[mshortColNum_mono_mass] ; 
		}

		public static void SetHeaderColumns(string headerLine, char [] delimiters)
		{
			string [] colNames = headerLine.Split(delimiters) ; 
			for (int i = 0 ; i < colNames.Length ; i++)
			{
				if (colNames[i]	== mstrHeader_unique_seq_id)
					mshortColNum_unique_seq_id = (short) i ;
				else if (colNames[i]	== mstrHeader_mod_count)
					mshortColNum_mod_count = (short) i ;
				else if (colNames[i]	== mstrHeader_mod_description)
					mshortColNum_mod_description = (short) i ;
				else if (colNames[i]	== mstrHeader_mono_mass)
					mshortColNum_mono_mass = (short) i ;
			}
		}
		
		public clsSeqInfo(string line, char []delimiters)
		{
			//
			// TODO: Add constructor logic here
			//
			string [] column = line.Split(delimiters) ; 
			mint_unique_seq_id = Convert.ToInt32(column[mshortColNum_unique_seq_id]) ; 
			mshort_mod_count = Convert.ToInt16(column[mshortColNum_mod_count]) ; 
			mstr_mod_description = column[mshortColNum_mod_description] ; 
			mdbl_mono_mass = Convert.ToDouble(column[mshortColNum_mono_mass]) ; 
		}
	}

	public class clsSeqInfoReader
	{
		private int mintPercentRead ; 
		private frmStatus.dlgSetPercentComplete mevntPercentComplete ; 
		private frmStatus.dlgSetStatusMessage mevntStatusMessage ; 
		
		public clsSeqInfoReader(frmStatus statusForm)
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
		public clsSeqInfo[] ReadSeqInfoFile(string fileName)
		{
			ArrayList arrSeqInfo = new ArrayList() ; 
			try 
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				FileInfo fInfo = new FileInfo(fileName) ; 
				long totalLength = fInfo.Length ; 

				using (StreamReader sr = new StreamReader(fileName)) 
				{
					mevntStatusMessage("Loading SeqInfo file" ) ;
					char [] delimiters = {'\t'} ; 
					string headerLine = sr.ReadLine() ; 
					clsSeqInfo.SetHeaderNames() ; 
					clsSeqInfo.SetHeaderColumns(headerLine, delimiters) ; 
					string line;
					long numRead = 0 ; 
					while ((line = sr.ReadLine()) != null) 
					{
						numRead += line.Length; 
						mintPercentRead = Convert.ToInt32((numRead*100)/totalLength) ; 
						if (numRead % 100 == 0)
							mevntPercentComplete(mintPercentRead) ; 
						arrSeqInfo.Add(new clsSeqInfo(line, delimiters)) ; 
					}
				}
			}
			catch (Exception e) 
			{
				// Let the user know what went wrong.
				System.Windows.Forms.MessageBox.Show(e.Message + e.StackTrace);
			}
			return (clsSeqInfo []) arrSeqInfo.ToArray(typeof(clsSeqInfo)) ; 
		}
	}



}

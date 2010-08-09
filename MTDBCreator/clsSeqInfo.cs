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
			string [] column = line.Split(delimiters) ; 
			mint_unique_seq_id = Convert.ToInt32(column[mshortColNum_unique_seq_id]) ; 
			mshort_mod_count = Convert.ToInt16(column[mshortColNum_mod_count]) ; 
			mstr_mod_description = column[mshortColNum_mod_description] ; 
			mdbl_mono_mass = Convert.ToDouble(column[mshortColNum_mono_mass]) ; 
		}
	}

	public class clsSeqInfoReader: ProcessorBase
    {
        private const int CONST_MAX_ATTEMPTS = 3;
		private int mintPercentRead ; 
		
		public clsSeqInfoReader()
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
		public clsSeqInfo[] ReadSeqInfoFile(string fileName)
		{
			ArrayList arrSeqInfo = new ArrayList() ;
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

                            StatusMessage("Loading SeqInfo file");
                            char[] delimiters = { '\t' };
                            string headerLine = sr.ReadLine();
                            clsSeqInfo.SetHeaderNames();
                            clsSeqInfo.SetHeaderColumns(headerLine, delimiters);
                            string line;
                            long numRead = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                numRead += line.Length;
                                mintPercentRead = Convert.ToInt32((numRead * 100) / totalLength);
                                if (numRead % 100 == 0)
                                    PercentComplete(mintPercentRead);
                                arrSeqInfo.Add(new clsSeqInfo(line, delimiters));
                            }
                        }
                        success = true;
                    }
                    catch (System.IO.IOException ioException)
                    {
                        // Let the user know what went wrong.
                        ErrorMessage("Error reading ResultsToSeqMap file: " + ioException.Message);
                        Console.WriteLine("Error reading ResultsToSeqMap file: " + ioException.Message + ioException.StackTrace);
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
				ErrorMessage("Error reading SeqInfo file: " + ex.Message ) ;
				Console.WriteLine("Error reading SeqInfo file: " + ex.Message + ex.StackTrace);
			}
			return (clsSeqInfo []) arrSeqInfo.ToArray(typeof(clsSeqInfo)) ; 
		}
	}



}

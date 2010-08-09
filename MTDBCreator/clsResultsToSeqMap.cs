using System;
using System.Collections ; 
using System.IO ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsResultsToSeqMap.
	/// </summary>
	public class clsResultsToSeqMap
	{
		private static string mstrDefaultHeader ="Result_ID	Unique_Seq_ID" ; 

		public int mint_result_id ; 
		public int mint_unique_seq_id ; 

		private static string mstrHeader_result_id = "" ;
		private static string mstrHeader_unique_seq_id = "" ; 

		private static short mshortColNum_result_id = 0 ; 
		private static short mshortColNum_unique_seq_id = 1 ; 

		public static void SetHeaderNames()
		{
			string []colNames = mstrDefaultHeader.Split(new char[]{'\t'}) ; 
			mstrHeader_result_id = colNames[mshortColNum_result_id] ;
			mstrHeader_unique_seq_id = colNames[mshortColNum_unique_seq_id] ; 
		}

		public static void SetHeaderColumns(string headerLine, char [] delimiters)
		{
			string [] colNames = headerLine.Split(delimiters) ; 
			for (int i = 0 ; i < colNames.Length ; i++)
			{
				if (colNames[i]	== mstrHeader_result_id)
					mshortColNum_result_id = (short) i ;
				else if	(colNames[i] == mstrHeader_unique_seq_id)
					mshortColNum_unique_seq_id =	(short)	i ;
			}
		}
		
		public clsResultsToSeqMap(string line, char []delimiters)
		{
			string [] column = line.Split(delimiters) ; 
			mint_result_id = Convert.ToInt32(column[mshortColNum_result_id]) ;  
			mint_unique_seq_id = Convert.ToInt32(column[mshortColNum_unique_seq_id]) ;  
		}
	}


	public class clsResultsToSeqMapReader: ProcessorBase
	{
        private const int CONST_MAX_ATTEMPTS = 3;
		private int mintPercentRead ; 

		public clsResultsToSeqMapReader()
		{
			mintPercentRead      = 0; 
		}

		public int PercentDone
		{
			get
			{
				return mintPercentRead ; 
			}
		}
		public clsResultsToSeqMap[] ReadResultsToSeqMapFile(string fileName)
		{
			ArrayList arrResultsToSeqMap = new ArrayList() ;
            int attempts = 0;
            bool success = false;
            try
            {
                while (success == false)
                {
                    // Let the program try to download the data a number of times before giving up.
                    attempts++;
                    try
                    {
                        // Create an instance of StreamReader to read from a file.
                        // The using statement also closes the StreamReader.
                        FileInfo fInfo = new FileInfo(fileName);
                        long totalLength = fInfo.Length;

                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            StatusMessage("Loading ResultsToSeqMap file");
                            char[] delimiters = { '\t' };
                            string headerLine = sr.ReadLine();
                            clsResultsToSeqMap.SetHeaderNames();
                            clsResultsToSeqMap.SetHeaderColumns(headerLine, delimiters);
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
                                arrResultsToSeqMap.Add(new clsResultsToSeqMap(line, delimiters));
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
                            throw ioException;
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                ErrorMessage("Error reading ResultsToSeqMap file: " + e.Message);
                Console.WriteLine("Error reading ResultsToSeqMap file: " + e.Message + e.StackTrace);
                throw e;
            }
			return (clsResultsToSeqMap []) arrResultsToSeqMap.ToArray(typeof(clsResultsToSeqMap)) ; 
		}
	}

}

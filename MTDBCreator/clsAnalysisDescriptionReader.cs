using System;
using System.IO ; 
using System.Collections ; 


namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsAnalysisDescriptionReader.
	/// </summary>
	public class clsAnalysisDescriptionReader
	{
		private ArrayList marrAnalyses ; 
		public clsAnalysisDescriptionReader(string fileName)
		{
			//
			// TODO: Add constructor logic here
			//
			marrAnalyses = new ArrayList() ; 
			try 
			{
				string errors = "" ; 
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using (StreamReader sr = new StreamReader(fileName)) 
				{
					char [] delimiters = {'\t'} ; 
					string headerLine = sr.ReadLine() ; 
					clsAnalysisDescription.SetHeader(headerLine, delimiters) ; 
					string line;
					// Read and display lines from the file until the end of 
					// the file is reached.
					while ((line = sr.ReadLine()) != null) 
					{
						clsAnalysisDescription analysis = null ; 
						try
						{
							analysis = new clsAnalysisDescription(line, delimiters) ; 
							marrAnalyses.Add(analysis) ; 
						}
						catch (Exception ex)
						{
							errors +=  ex.Message ; 
						}
						if (analysis != null && !Directory.Exists(analysis.mstrArchivePath))
						{
							errors += "\n Directory " + analysis.mstrArchivePath + " does not exist" ; 
							marrAnalyses.RemoveAt(marrAnalyses.Count-1) ; 
						}
					}
				}
				if (errors != "")
					System.Windows.Forms.MessageBox.Show(errors, "Errors in loading") ; 
			}
			catch (Exception e) 
			{
				// Let the user know what went wrong.
				System.Windows.Forms.MessageBox.Show(e.Message);
			}
		}
		public clsAnalysisDescription [] Analyses
		{
			get
			{
				return (clsAnalysisDescription []) marrAnalyses.ToArray(typeof(clsAnalysisDescription)) ; 
			}
		}

	}
}

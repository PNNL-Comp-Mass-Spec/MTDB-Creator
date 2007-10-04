using System;
using System.Collections ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsAnalysisReader.
	/// </summary>
	public class clsSequestAnalysisReader
	{
		public enum enmState {IDLE=0, SEQTOPROTEINMAP, RESULTS, RESULTSTOSEQMAP, SEQINFO } ; 
		private enmState menmState ;

		public int PercentDone
		{
			get
			{
				switch(menmState)
				{
					case enmState.SEQTOPROTEINMAP:
						if (mobjSeqToProteinMapReader != null)
							return mobjSeqToProteinMapReader.PercentDone ;
						return 0 ; 
						break ; 
					case enmState.RESULTS:
						if (mobjSequestReader != null)
							return mobjSequestReader.PercentDone ;
						return 0 ; 
						break ; 
					case enmState.RESULTSTOSEQMAP:
						if (mobjResultsToSeqMapReader != null)
							return mobjResultsToSeqMapReader.PercentDone ;
						return 0 ; 
						break ; 
					default:
						return 0 ;
						break ; 
				}
			}
		}

		private const string mstrSeqToProteinMapExt = "_syn_SeqToProteinMap.txt" ; 
		private const string mstrSequestResultsExt = "_syn.txt" ; 
		private const string mstrResultToSeqMapExt = "_syn_ResultToSeqMap.txt" ; 
		private const string mstrSeqInfoExt = "_syn_SeqInfo.txt" ; 

		public clsSeqToProteinMap [] marrSeqToProteinMap ; 
		public clsSequestResults [] marrSequestResults ; 
		public clsResultsToSeqMap [] marrResultsToSeqMap ; 
		public clsSeqInfo [] marrSeqInfo ; 

		private clsSeqToProteinMapReader mobjSeqToProteinMapReader ; 
		private clsSequestResultsReader mobjSequestReader ; 
		private clsResultsToSeqMapReader mobjResultsToSeqMapReader ; 
		private clsSeqInfoReader mobjSeqInfoReader ; 

		public clsSequestAnalysisReader(string path, string name, frmStatus statusForm )
		{
			//
			// TODO: Add constructor logic here
			//

			string seqToProteinMapFile = "" ;
			if (path != null)
				seqToProteinMapFile = System.IO.Path.Combine(path, name + mstrSeqToProteinMapExt) ; 
			else
				seqToProteinMapFile = name + mstrSeqToProteinMapExt ; 
			mobjSeqToProteinMapReader = new clsSeqToProteinMapReader(statusForm) ; 
			menmState = enmState.SEQTOPROTEINMAP ; 
			marrSeqToProteinMap = mobjSeqToProteinMapReader.ReadSeqToProteinMapFile(seqToProteinMapFile) ; 

			string sequestResultsFile = "" ;
			if (path != null)
				sequestResultsFile = System.IO.Path.Combine(path, name + mstrSequestResultsExt) ; 
			else
				sequestResultsFile = name + mstrSequestResultsExt ; 

			mobjSequestReader = new clsSequestResultsReader(statusForm) ; 
			menmState = enmState.RESULTS ; 
			marrSequestResults = mobjSequestReader.ReadSequestFile(sequestResultsFile) ; 

			string resultsToSeqMapFile = "" ;
			if (path != null)
				resultsToSeqMapFile = System.IO.Path.Combine(path, name + mstrResultToSeqMapExt) ; 
			else
				resultsToSeqMapFile = name + mstrResultToSeqMapExt ; 

			mobjResultsToSeqMapReader = new clsResultsToSeqMapReader(statusForm) ; 
			menmState = enmState.RESULTSTOSEQMAP ; 
			marrResultsToSeqMap = mobjResultsToSeqMapReader.ReadResultsToSeqMapFile(resultsToSeqMapFile) ; 

			string seqInfoFile = "" ;
			if (path != null)
				seqInfoFile = System.IO.Path.Combine(path, name + mstrSeqInfoExt) ; 
			else
				seqInfoFile = name + mstrSeqInfoExt ; 

			mobjSeqInfoReader = new clsSeqInfoReader(statusForm) ; 
			menmState = enmState.SEQINFO ; 
			marrSeqInfo = mobjSeqInfoReader.ReadSeqInfoFile(seqInfoFile) ; 
			Clean() ; 
		}

		/// <summary>
		/// This function removes extra hit nums that are present in the sequest files 
		/// which are not needed. To do so, we move through elements of the marrResultsToSeqMap, 
		/// and remove 
		/// </summary>
		private void Clean()
		{
			ArrayList arrSequestResults = new ArrayList() ; 
			int numResultsToSeqMaps = marrResultsToSeqMap.Length ; 
			for (int resultMapNum = 0 ; resultMapNum < numResultsToSeqMaps ; resultMapNum++)
			{
				clsResultsToSeqMap result2SeqMap = marrResultsToSeqMap[resultMapNum] ; 
				arrSequestResults.Add(marrSequestResults[result2SeqMap.mint_result_id-1]) ; 
			}
			marrSequestResults = (clsSequestResults []) arrSequestResults.ToArray(typeof(clsSequestResults)) ; 
		}
	}
}

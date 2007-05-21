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
						if (mobjXTandemReader != null)
							return mobjXTandemReader.PercentDone ;
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
		// private clsSequestReader mobjSequestReader ; 
		private clsResultsToSeqMapReader mobjResultsToSeqMapReader ; 
		private clsSeqInfoReader mobjSeqInfoReader ; 

		public clsSequestAnalysisReader(string path, string name, frmStatus statusForm )
		{
			//
			// TODO: Add constructor logic here
			//

			string seqToProteinMapFile = "" ;
			if (path != null)
				seqToProteinMapFile = path + "\\" + name + mstrSeqToProteinMapExt ; 
			else
				seqToProteinMapFile = name + mstrSeqToProteinMapExt ; 
			mobjSeqToProteinMapReader = new clsSeqToProteinMapReader(statusForm) ; 
			menmState = enmState.SEQTOPROTEINMAP ; 
			marrSeqToProteinMap = mobjSeqToProteinMapReader.ReadSeqToProteinMapFile(seqToProteinMapFile) ; 

			string sequestResultsFile = "" ;
			if (path != null)
				sequestResultsFile = path + "\\" + name + mstrSequestResultsExt ; 
			else
				sequestResultsFile = name + mstrSequestResultsExt ; 

			mobjSequestReader = new clsSequestResultsReader(statusForm) ; 
			menmState = enmState.RESULTS ; 
			marrSequestResults = mobjSequestReader.ReadSequestFile(sequestResultsFile) ; 

			string resultsToSeqMapFile = "" ;
			if (path != null)
				resultsToSeqMapFile = path + "\\" + name + mstrResultToSeqMapExt ; 
			else
				resultsToSeqMapFile = name + mstrResultToSeqMapExt ; 

			mobjResultsToSeqMapReader = new clsResultsToSeqMapReader(statusForm) ; 
			menmState = enmState.RESULTSTOSEQMAP ; 
			marrResultsToSeqMap = mobjResultsToSeqMapReader.ReadResultsToSeqMapFile(resultsToSeqMapFile) ; 

			string seqInfoFile = "" ;
			if (path != null)
				seqInfoFile = path + "\\" + name + mstrSeqInfoExt ; 
			else
				seqInfoFile = name + mstrSeqInfoExt ; 

			mobjSeqInfoReader = new clsSeqInfoReader(statusForm) ; 
			menmState = enmState.SEQINFO ; 
			marrSeqInfo = mobjSeqInfoReader.ReadSeqInfoFile(seqInfoFile) ; 

		}
	}
}

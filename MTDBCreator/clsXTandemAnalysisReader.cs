using System;
using System.Collections ; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsAnalysisReader.
	/// </summary>
	public class clsXTandemAnalysisReader
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
					case enmState.RESULTS:
						if (mobjXTandemReader != null)
							return mobjXTandemReader.PercentDone ;
						return 0 ; 
					case enmState.RESULTSTOSEQMAP:
						if (mobjResultsToSeqMapReader != null)
							return mobjResultsToSeqMapReader.PercentDone ;
						return 0 ; 
					default:
						return 0 ;
				}
			}
		}

		private const string mstrSeqToProteinMapExt = "_xt_SeqToProteinMap.txt" ; 
		private const string mstrXTandemResultsExt = "_xt.txt" ; 
		private const string mstrResultToSeqMapExt = "_xt_ResultToSeqMap.txt" ; 
		private const string mstrSeqInfoExt = "_xt_SeqInfo.txt" ; 

		public clsSeqToProteinMap [] marrSeqToProteinMap ; 
		public clsXTandemResults [] marrXTandemResults ; 
		public clsResultsToSeqMap [] marrResultsToSeqMap ; 
		public clsSeqInfo [] marrSeqInfo ; 

		private clsSeqToProteinMapReader mobjSeqToProteinMapReader ; 
		private clsXTandemResultsReader mobjXTandemReader ; 
		private clsResultsToSeqMapReader mobjResultsToSeqMapReader ; 
		private clsSeqInfoReader mobjSeqInfoReader ; 

		public clsXTandemAnalysisReader(string path, string name, frmStatus statusForm )
		{
			//
			// TODO: Add constructor logic here
			//

			string seqToProteinMapFile = "" ;
			if (path != null)
				seqToProteinMapFile = System.IO.Path.Combine(path , name + mstrSeqToProteinMapExt) ; 
			else
				seqToProteinMapFile = name + mstrSeqToProteinMapExt ; 
			mobjSeqToProteinMapReader = new clsSeqToProteinMapReader(statusForm) ; 
			menmState = enmState.SEQTOPROTEINMAP ; 
			marrSeqToProteinMap = mobjSeqToProteinMapReader.ReadSeqToProteinMapFile(seqToProteinMapFile) ; 

			string xtandemResultsFile = "" ;
			if (path != null)
				xtandemResultsFile = System.IO.Path.Combine(path, name + mstrXTandemResultsExt) ; 
			else
				xtandemResultsFile = name + mstrXTandemResultsExt ; 
			mobjXTandemReader = new clsXTandemResultsReader(statusForm) ; 
			menmState = enmState.RESULTS ; 
			marrXTandemResults = mobjXTandemReader.ReadXTandemFile(xtandemResultsFile) ; 

			string resultsToSeqMapFile = "" ;
			if (path != null)
				resultsToSeqMapFile = System.IO.Path.Combine(path, name + mstrResultToSeqMapExt) ; 
			else
				resultsToSeqMapFile = name + mstrXTandemResultsExt ; 
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

		}
	}
}

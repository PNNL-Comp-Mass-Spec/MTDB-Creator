using System;
using System.Collections ;
using MTDBCreator.IO;
using MTDBCreator.Data;
using System.Collections.Generic;
using System.IO; 

namespace MTDBCreator
{   
	/// <summary>
	/// Summary description for clsAnalysisReader.
	/// </summary>
	public class SequestAnalysisReader: PhrpAnalysisReader, IAnalysisReader
	{			
        public SequestAnalysisReader()
        {
            mstrSeqToProteinMapExt  = "_syn_SeqToProteinMap.txt" ; 
            mstrResultsExt          = "_syn.txt" ; 
            mstrResultToSeqMapExt   = "_syn_ResultToSeqMap.txt" ; 
            mstrSeqInfoExt          = "_syn_SeqInfo.txt" ;

            m_analysisMap.Add(mstrSeqToProteinMapExt, "");
            m_analysisMap.Add(mstrResultsExt, "");
            m_analysisMap.Add(mstrResultToSeqMapExt, "");
            m_analysisMap.Add(mstrSeqInfoExt, "");

        }
        
        protected override List<Target> ReadTargets(string path)
        {
            /* Sequest File ----------------------------------------------------------------*/
            string sequestResultsFile           = m_analysisMap[mstrResultsExt];			
            SequestResultReader sequestReader   = new SequestResultReader();             
			List<SequestResult> results         = sequestReader.Read(path) ;
            List<Target> targets                = new List<Target>();
            results.ForEach(x => targets.Add(x));
            return targets;
        }
    }

}

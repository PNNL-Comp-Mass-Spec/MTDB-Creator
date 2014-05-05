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
	public class MsgfPlusAnalysisReader: PhrpAnalysisReader, IAnalysisReader
	{
        public MsgfPlusAnalysisReader()
        {
            mstrSeqToProteinMapExt  = "msgfdb_syn_SeqToProteinMap.txt" ; 
            mstrResultsExt          = "msgfdb_syn.txt" ; 
            mstrResultToSeqMapExt   = "msgfdb_syn_ResultToSeqMap.txt" ; 
            mstrSeqInfoExt          = "msgfdb_syn_SeqInfo.txt" ;

            m_analysisMap.Add(mstrSeqToProteinMapExt, "");
            m_analysisMap.Add(mstrResultsExt, "");
            m_analysisMap.Add(mstrResultToSeqMapExt, "");
            m_analysisMap.Add(mstrSeqInfoExt, "");

        }
        
        protected override List<Target> ReadTargets(string path)
        {
            /* Sequest File ----------------------------------------------------------------*/
            MsgfPlusResultsReader reader        = new MsgfPlusResultsReader();             
			List<MsgfPlusResult> results        = reader.Read(path) ;
            List<Target> targets                = new List<Target>();
            results.ForEach(x => targets.Add(x));
            return targets;
        }
    }

}

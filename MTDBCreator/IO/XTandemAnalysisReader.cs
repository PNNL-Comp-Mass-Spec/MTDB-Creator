using System;
using System.Collections ;
using MTDBCreator.IO;
using System.Collections.Generic;
using MTDBCreator.Data; 

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for clsAnalysisReader.
	/// </summary>
    public class XTandemAnalysisReader : PhrpAnalysisReader, IAnalysisReader
	{				           
        /// <summary>
        /// Default constructor.
        /// </summary>
        public XTandemAnalysisReader()
        {            
            mstrSeqToProteinMapExt = "_xt_SeqToProteinMap.txt" ; 
            mstrResultsExt         = "_xt.txt" ; 
            mstrResultToSeqMapExt  = "_xt_ResultToSeqMap.txt" ; 
            mstrSeqInfoExt         = "_xt_SeqInfo.txt" ;

            m_analysisMap.Add(mstrSeqToProteinMapExt, "");
            m_analysisMap.Add(mstrResultsExt,         "");       
            m_analysisMap.Add(mstrResultToSeqMapExt,  "");
            m_analysisMap.Add(mstrSeqInfoExt,         "");
        }
        /// <summary>
        /// Reads the path provided and returns a list of sequences
        /// </summary>
        /// <param name="path"></param>
        protected override List<Target> ReadTargets(string path)
        {
            
            // Read the X!Tandem results.
            XTandemResultsReader xTandemReader = new XTandemResultsReader();                        
            List<XTandemResult> xTandemData    = xTandemReader.Read(path);
            List<Target> targets               = new List<Target>();
            xTandemData.ForEach(x => targets.Add(x));
            return targets;
        }              
    }
}

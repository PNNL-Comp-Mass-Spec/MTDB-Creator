using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Algorithms
{
    public class XTandemTargetResultsFilter
    {
        //public void Filter()
        //{
        //    for (int resultNum = 0; resultNum < numResults; resultNum++)
        //    {
        //        XTandemResult xtResult          = results.SearchResults[resultNum] as XTandemResult;
        //        ResultToSequenceMap resultToSeq = results.ResultsToSequenceMap[resultNum];
        //        Sequence seqInfo                = results.Sequences[resultToSeq.UniqueSequenceId - 1];
        //        int massTagIndex                = (int)mhashMassTags[xtResult.CleanSequence + seqInfo.ModificationDescription];
                
                
        //        MassTag massTag                 = m_massTags[massTagIndex];
        //        if (massTag.GaNetCount < mobjOptions.MinObservationsForExport
        //            || !mobjOptions.IsToBeExported(xtResult)
        //            || xtResult.LogPeptideEValue > mobjOptions.MaxLogEValForXTandemAlignment)
        //            continue;

        //        if (peptideTable.ContainsKey(massTagIndex))
        //        {
        //            if ((int)peptideTable[massTagIndex] < xtResult.Scan)
        //                peptideTable[massTagIndex] = xtResult.Scan;
        //        }
        //        else
        //        {
        //            peptideTable[massTagIndex] = xtResult.Scan;
        //        }
        //    }
        //}
    }
}

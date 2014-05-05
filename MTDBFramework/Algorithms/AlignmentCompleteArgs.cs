using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.Alignment.LcmsWarp;

namespace MTDBFramework.Algorithms
{
    public class AlignmentCompleteArgs : EventArgs
    {
        public AlignmentCompleteArgs(IEnumerable<LcmsWarpAlignmentData> alignmentData)
        {
            AlignmentData = alignmentData;            
        }

        public IEnumerable<LcmsWarpAlignmentData> AlignmentData { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using PNNLOmics.Algorithms.Alignment.LcmsWarp;

namespace MTDBFramework.Algorithms
{
	/// <summary>
	/// Arguments for Alignment completion display
	/// </summary>
    public class AlignmentCompleteArgs : EventArgs
    {
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="alignmentData"></param>
        public AlignmentCompleteArgs(IEnumerable<LcmsWarpAlignmentData> alignmentData)
        {
            AlignmentData = alignmentData;            
        }

		/// <summary>
		/// Alignment data
		/// </summary>
        public IEnumerable<LcmsWarpAlignmentData> AlignmentData { get; private set; }
    }
}
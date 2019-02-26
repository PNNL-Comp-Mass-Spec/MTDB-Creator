using System;
using System.Collections.Generic;
using FeatureAlignment.Data.Alignment;

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
        public AlignmentCompleteArgs(IEnumerable<AlignmentData> alignmentData)
        {
            AlignmentData = alignmentData;
        }

        /// <summary>
        /// Alignment data
        /// </summary>
        public IEnumerable<AlignmentData> AlignmentData { get; }
    }
}
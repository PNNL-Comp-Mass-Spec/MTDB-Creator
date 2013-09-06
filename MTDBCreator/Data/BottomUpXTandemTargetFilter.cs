using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    /// <summary>
    /// Filters targets based on the user options
    /// </summary>
    public class BottomUpXTandemTargetFilter : ITargetFilter
    {
        /// <summary>
        /// Constructor for a bottom up target filter.
        /// </summary>
        /// <param name="options"></param>
        public BottomUpXTandemTargetFilter(Options options)
        {
            Options = options;
        }

        public Options Options { get; set; }

        #region ITargetFilter Members
        /// <summary>
        /// Determines if a feature should be filtered or not.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool ShouldFilter(Target target)
        {
            Sequence sequence = target.SequenceData;

            if (sequence == null)
                return true;

            // in the alignment we will only use the unmodified peptides
            if (sequence.ModificationCount > Options.MaxModificationsForAlignment)
                return true;

            XTandemResult result = target as XTandemResult;

            if (result == null)            
                return true;
            
            if (result.LogPeptideEValue > Options.MaxLogEValForXTandemAlignment)
                return true;

            return false;
        }

        #endregion
    }
}

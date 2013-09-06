using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{

    /// <summary>
    /// Filters targets based on the user options
    /// </summary>
    public class BottomUpSequestTargetFilter : ITargetFilter
    {
        /// <summary>
        /// Constructor for a bottom up target filter.
        /// </summary>
        /// <param name="options"></param>
        public BottomUpSequestTargetFilter(Options options)
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

            SequestResult result = target as SequestResult;

            if (result == null)            
                return true;
                

            if (result.XCorr < Options.MinXCorrForAlignment)
                return true;

            return false;
        }

        #endregion
    }
}

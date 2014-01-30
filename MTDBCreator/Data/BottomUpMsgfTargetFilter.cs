using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    public class BottomUpMsgfTargetFilter : ITargetFilter
    {
        /// <summary>
        /// Constructor for a bottom up target filter.
        /// </summary>
        /// <param name="options"></param>
        public BottomUpMsgfTargetFilter(Options options)
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

            MsgfPlusResult result = target as MsgfPlusResult;

            if (result == null)
                return true;


            if (result.Fdr > Options.MsgfFDR)
                return true;


            if (result.SpectralProbability > Options.MsgfSpectralEValue)
                return true;

            return false;
        }

        #endregion
    }
}

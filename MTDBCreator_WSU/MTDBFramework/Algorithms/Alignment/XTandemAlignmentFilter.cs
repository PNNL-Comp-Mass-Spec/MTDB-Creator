﻿#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class XTandemAlignmentFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public XTandemAlignmentFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Target target)
        {
            //if (target.IsSeqInfoExist != 1)
            //{
            //    return true;
            //}

            // In the alignment we will only use the unmodified peptides
            if (target.ModificationCount > this.FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}

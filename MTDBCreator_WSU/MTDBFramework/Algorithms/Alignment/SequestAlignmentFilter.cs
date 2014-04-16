﻿#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class SequestAlignmentFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public SequestAlignmentFilter(Options options)
        {
            FilterOptions = options;
        }

        public bool ShouldFilter(Evidence evidence)
        {
            // In the alignment we will only use the unmodified peptides
            if (evidence.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}

﻿#region Namespaces

using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    /// <summary>
    /// Alignment Filtering for Sequest Workflows
    /// </summary>
    public class SequestAlignmentFilter : ITargetFilter
    {
        /// <summary>
        /// Options
        /// </summary>
        public Options FilterOptions { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public SequestAlignmentFilter(Options options)
        {
            FilterOptions = options;
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="evidence"></param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            if (evidence.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}

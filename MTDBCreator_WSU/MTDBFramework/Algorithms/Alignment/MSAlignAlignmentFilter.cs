using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBFramework.Data;

namespace MTDBFramework.Algorithms.Alignment
{
    public class MSAlignAlignmentFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MSAlignAlignmentFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Target target)
        {
            // In the alignment we will only use the unmodified peptides
            if (target.ModificationCount > this.FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}

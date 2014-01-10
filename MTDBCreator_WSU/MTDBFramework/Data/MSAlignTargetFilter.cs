using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Data
{
    public class MSAlignTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MSAlignTargetFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Target target)
        {
            MSAlignResult result = target as MSAlignResult;

            if (result == null)
            {
                return true;
            }

            if (result.EScore > this.FilterOptions.MaxLogEValForMSAlignAlignment)
            {
                return true;
            }

            return false;
        }
    }
}

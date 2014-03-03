using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Data
{
    public class MSGFPlusTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MSGFPlusTargetFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Target target)
        {
            MSGFPlusResult result = target as MSGFPlusResult;

            if (result == null)
            {
                return true;
            }
            
            if(result.Sequence == null)
            {
                return true;
            }

            if (result.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            if(result.Fdr > FilterOptions.MsgfFDR)
            {
                return true;
            }

            if (result.SpectralProbability > FilterOptions.MsgfSpectralEValue)
            {
                return true;
            }
            return false;
        }
    }
}

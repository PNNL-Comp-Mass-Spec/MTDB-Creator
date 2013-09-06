using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    /// <summary>
    /// Generates filter objects for different workflows.
    /// </summary>
    public class TargetFilterFactory
    {
        /// <summary>
        /// Creates a list of filters
        /// </summary>
        /// <returns></returns>
        public static ITargetFilter CreateFilters(SupportedTools tool, Options options)
        {
            ITargetFilter filter = null;

            switch (tool)
	        {
		        case SupportedTools.Sequest:
                    filter = new BottomUpSequestTargetFilter(options);
                    break;
                case SupportedTools.XTandem:
                    filter = new BottomUpSequestTargetFilter(options);
                    break;
                case SupportedTools.NotSupported:
                    break;
                default:
                    break;
	        }
            
            return filter;
        }
    }

    /// <summary>
    /// Filter Types for MTDB Creator
    /// </summary>
    public enum FilterType
    {
        BottomUp,
        TopDown
    }


}

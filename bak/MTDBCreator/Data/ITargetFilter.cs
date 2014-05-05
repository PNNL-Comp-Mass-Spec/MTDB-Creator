using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    public interface ITargetFilter
    {
        /// <summary>
        /// Determines if a target should be filtered.  True is yes, False is allow to pass through.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool ShouldFilter(Target t);
    }
}

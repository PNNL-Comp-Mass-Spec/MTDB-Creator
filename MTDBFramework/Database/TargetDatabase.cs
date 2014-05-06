#region Namespaces

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    public class TargetDatabase
    {
        public TargetDatabase()
        {
            ConsensusTargets = new List<ConsensusTarget>();
        }

        public IList<ConsensusTarget> ConsensusTargets { get; set; }
    }
}
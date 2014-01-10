#region Namespaces

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MTDBFramework.Algorithms.Clustering;

#endregion

namespace MTDBFramework.Database
{
    public class TargetDatabase
    {
        public ObservableCollection<ConsensusTarget> ConsensusTargets { get; set; }
    }
}
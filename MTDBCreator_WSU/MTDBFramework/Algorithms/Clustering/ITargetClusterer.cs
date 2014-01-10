#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    public interface ITargetClusterer
    {
        IEnumerable<ConsensusTarget> Cluster(IEnumerable<Target> targets);
    }
}

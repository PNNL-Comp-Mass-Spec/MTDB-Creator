#region Namespaces

using System.Collections.Generic;
using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    /// <summary>
    /// Interface for target clustering
    /// </summary>
    public interface ITargetClusterer
    {
        /// <summary>
        /// Perform target clustering
        /// </summary>
        /// <param name="evidences"></param>
        /// <returns></returns>
        IEnumerable<ConsensusTarget> Cluster(IEnumerable<Evidence> evidences);
    }
}

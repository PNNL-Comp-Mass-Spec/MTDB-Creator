#region Namespaces

using System;
using System.Collections.Generic;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    /// <summary>
    /// Target clustering for Spectral Workflows
    /// </summary>
    public class SpectralTargetClusterer : ITargetClusterer
    {
        /// <summary>
        /// Perform Target clustering
        /// </summary>
        /// <param name="evidences"></param>
        /// <returns></returns>
        public IEnumerable<ConsensusTarget> Cluster(IEnumerable<Evidence> evidences)
        {
            throw new NotImplementedException();
        }
    }
}

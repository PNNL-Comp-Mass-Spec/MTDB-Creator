#region Namespaces

using System.Collections.Generic;
using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    /// <summary>
    /// Target clustering using sequences
    /// </summary>
    public class SequenceTargetClusterer : ITargetClusterer
    {
        /// <summary>
        /// Perform target clustering
        /// </summary>
        /// <param name="evidences"></param>
        /// <returns></returns>
        public IEnumerable<ConsensusTarget> Cluster(IEnumerable<Evidence> evidences)
        {
            var consensusTargetList = new List<ConsensusTarget>();
            var targetMap = new Dictionary<string, ConsensusTarget>();

            foreach (var t in evidences)
            {
                var sequence = t.EncodedNonNumericSequence;

                if (!targetMap.ContainsKey(sequence))
                {
                    targetMap.Add(sequence, new ConsensusTarget());
                }

                targetMap[sequence].AddEvidence(t);

                if (!targetMap[sequence].Charges.Contains(t.Charge))
                {
                    targetMap[sequence].Charges.Add(t.Charge);
                }

                foreach (var protein in t.Proteins)
                {
                    if (!targetMap[sequence].Proteins.Contains(protein))
                    {
                        targetMap[sequence].AddProtein(protein);
                    }
                }
            }

            foreach (var consensusTarget in targetMap.Values)
            {
                consensusTarget.CalculateStatistics();

                consensusTargetList.Add(consensusTarget);
            }

            return consensusTargetList;
        }
    }
}

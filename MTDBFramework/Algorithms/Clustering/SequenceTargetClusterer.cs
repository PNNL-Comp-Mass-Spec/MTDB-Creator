#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    public class SequenceTargetClusterer : ITargetClusterer
    {
        public IEnumerable<ConsensusTarget> Cluster(IEnumerable<Evidence> evidences)
        {
            var consensusTargetList = new List<ConsensusTarget>();
            var targetMap = new Dictionary<string, ConsensusTarget>();

            foreach (var t in evidences)
            {
                var sequence = t.Sequence;
                 
                if (!targetMap.ContainsKey(sequence))
                {
                    targetMap.Add(sequence, new ConsensusTarget());
                }

                targetMap[sequence].AddTarget(t);

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

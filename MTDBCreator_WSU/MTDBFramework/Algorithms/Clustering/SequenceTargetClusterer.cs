#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    public class SequenceTargetClusterer : ITargetClusterer
    {
        public IEnumerable<ConsensusTarget> Cluster(IEnumerable<Target> targets)
        {
            List<ConsensusTarget> consensusTargetList = new List<ConsensusTarget>();
            Dictionary<string, ConsensusTarget> targetMap = new Dictionary<string, ConsensusTarget>();

            foreach (Target t in targets)
            {
                string sequence = t.Sequence;

                if (!targetMap.ContainsKey(sequence))
                {
                    targetMap.Add(sequence, new ConsensusTarget());
                }

                targetMap[sequence].AddTarget(t);
            }

            foreach (ConsensusTarget consensusTarget in targetMap.Values)
            {
                consensusTarget.CalculateStatistics();

                consensusTargetList.Add(consensusTarget);
            }

            return consensusTargetList;
        }
    }
}

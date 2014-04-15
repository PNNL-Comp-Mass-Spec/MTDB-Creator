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
            List<ConsensusTarget> consensusTargetList = new List<ConsensusTarget>();
            Dictionary<string, ConsensusTarget> targetMap = new Dictionary<string, ConsensusTarget>();

            foreach (Evidence t in evidences)
            {
                string sequence = t.Sequence;
                 
                if (!targetMap.ContainsKey(sequence))
                {
                    targetMap.Add(sequence, new ConsensusTarget());
                }

                targetMap[sequence].AddTarget(t);
                foreach (ProteinInformation protein in t.Proteins)
                {
                    if (!targetMap[sequence].Proteins.Contains(protein))
                    {
                        targetMap[sequence].AddProtein(protein);
                    }
                    //protein.Consensus.Add(targetMap[sequence]);
                }
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

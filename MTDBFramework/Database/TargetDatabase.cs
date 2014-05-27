#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    /// <summary>
    /// Maintains information about targets and unique proteins.
    /// </summary>
    public sealed class TargetDatabase
    {
        private readonly Dictionary<int, ProteinInformation> m_uniqueProteins;
 
        public TargetDatabase()
        {
            ConsensusTargets    = new List<ConsensusTarget>();
            Proteins            = new List<ProteinInformation>();
            m_uniqueProteins    = new Dictionary<int, ProteinInformation>();
        }
        /// <summary>
        /// Adds a target and its matching proteins to the database (unique)
        /// </summary>
        /// <param name="target"></param>
        public void AddConsensusTarget(ConsensusTarget target)
        {
            ConsensusTargets.Add(target);
            //foreach (var protein in target.Proteins)
            //{
            //    var proteinId = protein.Id;
            //    if (!m_uniqueProteins.ContainsKey(proteinId))
            //    {
            //        m_uniqueProteins.Add(proteinId, protein);
            //        Proteins.Add(protein);
            //    }
            //}
        }

        /// <summary>
        /// Clears an existing target and protein data
        /// </summary>
        public void ClearTargets()
        {
            ConsensusTargets.Clear();
            Proteins.Clear();
        }
        
        public IList<ConsensusTarget>    ConsensusTargets { get; private set; }
        public IList<ProteinInformation> Proteins { get; private set; }
    }
}
#region Namespaces

#endregion

using System.Collections.Generic;
using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.Database
{
    /// <summary>
    /// Maintains information about targets and unique proteins.
    /// </summary>
    public sealed class TargetDatabase
    {
        // Unused: private readonly Dictionary<int, ProteinInformation> m_uniqueProteins;

        /// <summary>
        /// Constructor
        /// </summary>
        public TargetDatabase()
        {
            ConsensusTargets    = new List<ConsensusTarget>();
            Proteins            = new List<ProteinInformation>();
            // Unused: m_uniqueProteins    = new Dictionary<int, ProteinInformation>();
        }
        /// <summary>
        /// Adds a target and its matching proteins to the database (unique)
        /// </summary>
        /// <param name="target"></param>
        public void AddConsensusTarget(ConsensusTarget target)
        {
            ConsensusTargets.Add(target);
        }

        /// <summary>
        /// Clears an existing target and protein data
        /// </summary>
        public void ClearTargets()
        {
            ConsensusTargets.Clear();
            Proteins.Clear();
        }

        /// <summary>
        /// All consensus targets
        /// </summary>
        public IList<ConsensusTarget>    ConsensusTargets { get; }

        /// <summary>
        /// All proteins
        /// </summary>
        public IList<ProteinInformation> Proteins { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace MTDBCreator.Data
{
	/// <summary>
	/// Summary description for clsProtein
	/// </summary>
	public class Protein
	{
        /// <summary>
        /// Map of targets that map to this protein
        /// </summary>
        private Dictionary<int, ConsensusTarget> m_targets;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Protein()
        {
            m_targets = new Dictionary<int,ConsensusTarget>();
        }
        /// <summary>
        /// Protein Accession ID
        /// </summary>
        public int Id { get; set; }

        private int m_proteinId;


        /// <summary>
        /// Protein Sequence
        /// </summary>
        public string Reference
        {
            get
            {
                return StringCache.Cache.UniqueProteins.GetString(m_proteinId);
            }
            set
            {
                m_proteinId = StringCache.Cache.UniqueProteins.AddString(value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public double EValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double IntensityLog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public short TerminusState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public short CleavageState { get; set; }

        public override string ToString()
        {
            return Reference;
        }

        
        /// <summary>
        /// Gets a list of the proteins available.
        /// </summary>
        public List<ConsensusTarget> GetMappedTargets()
        {            
            List<ConsensusTarget> targets = new List<ConsensusTarget>();
            foreach (ConsensusTarget target in m_targets.Values)
            {
                targets.Add(target);
            }
            return targets;            
        }        
        /// <summary>
        /// Adds a target to the protein.  If it already exists, the target is ignored.
        /// </summary>
        /// <param name="target"></param>
        public void AddConsensusTarget(ConsensusTarget target)
        {
            if (!m_targets.ContainsKey(target.Id))
            {
                m_targets.Add(target.Id, target);
            }
        }
    }
}

using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    /// <summary>
    /// Relation between a ConsensusTarget object and a ProteinInformation object for database use
    /// </summary>
    public class ConsensusProteinPair
    {
        /// <summary>
        /// ConsensusTarget object
        /// </summary>
        public ConsensusTarget Consensus { get; set; }

        /// <summary>
        /// ProteinInformation object
        /// </summary>
        public ProteinInformation Protein { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        private int m_consensus;
        private int m_proteinId;

        /// <summary>
        /// ConsensusTarget Id
        /// </summary>
        public int ConsensusId { get => Consensus?.Id ?? m_consensus;
            set => m_consensus = value;
        }

        /// <summary>
        /// ProteinInformation Id
        /// </summary>
        public int ProteinId { get => Protein?.Id ?? m_proteinId;
            set => m_proteinId = value;
        }

        /// <summary>
        /// Cleavage State
        /// </summary>
        public short CleavageState { get; set; }

        /// <summary>
        /// Terminus State
        /// </summary>
        public short TerminusState { get; set; }

        /// <summary>
        /// Residue Start Position
        /// </summary>
        public int ResidueStart { get; set; }

        /// <summary>
        /// Residue End Position
        /// </summary>
        public int ResidueEnd { get; set; }

        /// <summary>
        /// Overloaded equality comparison
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!(obj is ConsensusProteinPair cp))
                return false;

            return cp.Consensus.Id == Consensus.Id &&
                   cp.Protein.Id == Protein.Id;
        }

        /// <summary>
        /// Overloaded Hash
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Consensus != null ? Consensus.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Protein != null ? Protein.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CleavageState.GetHashCode();
                hashCode = (hashCode * 397) ^ TerminusState.GetHashCode();
                hashCode = (hashCode * 397) ^ ResidueStart;
                hashCode = (hashCode * 397) ^ ResidueEnd;
                return hashCode;
            }
        }
    }
}

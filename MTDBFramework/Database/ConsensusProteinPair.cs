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
        private int m_prot;

		/// <summary>
		/// ConsensusTarget Id
		/// </summary>
        public int ConsensusId { get { return (Consensus != null) ? Consensus.Id : m_consensus; } private set { m_consensus = value; } }

		/// <summary>
		/// ProteinInformation Id
		/// </summary>
        public int ProteinId { get { return (Protein != null) ? Protein.Id : m_prot; } private set { m_prot = value; } }

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
        public short ResidueStart { get; set; }

		/// <summary>
		/// Residue End Position
		/// </summary>
        public short ResidueEnd { get; set; }

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
            var cp = obj as ConsensusProteinPair;

            return ((cp.Consensus.Id == this.Consensus.Id)
                 && (cp.Protein.Id == this.Protein.Id));
        }

		/// <summary>
		/// Overloaded Hash
		/// </summary>
		/// <returns></returns>
        public override int GetHashCode()
        {
            return 9999;
        }
    }
}

using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class ConsensusProteinPair
    {
        public ConsensusTarget Consensus { get; set; }
        public ProteinInformation Protein { get; set; }

        public int Id { get; set; }

        private int m_consensus;
        private int m_prot;

        public int ConsensusId { get { return (Consensus != null) ? Consensus.Id : m_consensus; } private set { /*Consensus.Id*/m_consensus = value; } }
        public int ProteinId { get { return (Protein != null) ? Protein.Id : m_prot; } private set { /*Protein.Id*/        m_prot = value; } }

        public short CleavageState { get; set; }
        public short TerminusState { get; set; }
        public short ResidueStart { get; set; }
        public short ResidueEnd { get; set; }

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

        public override int GetHashCode()
        {
            return 9999;
        }
    }
}

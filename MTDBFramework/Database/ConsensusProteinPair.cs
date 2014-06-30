using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class ConsensusProteinPair
    {
        public ConsensusTarget Consensus { get; set; }
        public ProteinInformation Protein { get; set; }

        public int Id { get; set; }

        public int ConsensusId { get { return Consensus.Id; } private set { Consensus.Id = value; } }
        public int ProteinId { get { return Protein.Id; } private set { Protein.Id = value; } }

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

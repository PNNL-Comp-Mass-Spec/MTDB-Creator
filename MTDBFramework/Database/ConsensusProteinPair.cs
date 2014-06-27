using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class ConsensusProteinPair
    {
        public ConsensusTarget Consensus { get; set; }
        public ProteinInformation Protein { get; set; }

        public int CleavageState { get; set; }
        public int TerminusState { get; set; }
        public int ResidueStart { get; set; }
        public int ResidueEnd { get; set; }

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

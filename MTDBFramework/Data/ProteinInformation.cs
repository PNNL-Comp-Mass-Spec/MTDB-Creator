using System;
using System.Collections.Generic;
using PHRPReader;

namespace MTDBFramework.Data
{
    public class ProteinInformation:IEquatable<ProteinInformation>
    {
        public ProteinInformation()
        {
            Consensus = new List<ConsensusTarget>();
        }
        public IList<ConsensusTarget> Consensus { get; set; }
        public int Id { get; set; }
        public string ProteinName  { get; set; }
        public clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants TerminusState { get; set; }
        public clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants CleavageState { get; set; }
        public int    ResidueStart { get; set; }
        public int    ResidueEnd   { get; set; }

        public override bool Equals(object obj)
        {                        
            if (obj == null)
            {
                return false;
            }
            var objAsProt = obj as ProteinInformation;
            if (objAsProt == null)
            {
                return false;
            }
            return (ProteinName.Equals(objAsProt.ProteinName));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(ProteinInformation other)
        {
            if (other == null)
            {
                return false;
            }
            return (ProteinName.Equals(other.ProteinName));
        }
    }
}

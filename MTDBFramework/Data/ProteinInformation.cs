using System;
using System.Collections.Generic;
using PHRPReader;
using MTDBFramework.Database;

namespace MTDBFramework.Data
{
	/// <summary>
	/// Protein information container
	/// </summary>
    public class ProteinInformation:IEquatable<ProteinInformation>
    {
		/// <summary>
		/// Constructor
		/// </summary>
        public ProteinInformation()
        {
            Consensus = new List<ConsensusTarget>();
            ConsensusProtein = new List<ConsensusProteinPair>();
        }

		/// <summary>
		/// Consensus targets that use the protein
		/// </summary>
        public IList<ConsensusTarget> Consensus { get; set; }

		/// <summary>
		/// Id
		/// </summary>
        public int Id { get; set; }

		/// <summary>
		/// Protein Name
		/// </summary>
        public string ProteinName  { get; set; }

		/// <summary>
		/// Terminus State
		/// </summary>
        public clsPeptideCleavageStateCalculator.ePeptideTerminusStateConstants TerminusState { get; set; }

		/// <summary>
		/// Cleavage State
		/// </summary>
        public clsPeptideCleavageStateCalculator.ePeptideCleavageStateConstants CleavageState { get; set; }

		/// <summary>
		/// Residue Start Position
		/// </summary>
        public int    ResidueStart { get; set; }

		/// <summary>
		/// Residue End Position
		/// </summary>
        public int    ResidueEnd   { get; set; }

		/// <summary>
		/// Consensus Protein data
		/// </summary>
        public IList<ConsensusProteinPair> ConsensusProtein { get; set; }

		/// <summary>
		/// Overloaded comparison
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Overloaded Hash
		/// </summary>
		/// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

		/// <summary>
		/// Comparison
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
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

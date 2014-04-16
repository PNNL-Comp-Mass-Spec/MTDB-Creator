using System.Collections.Generic;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class TargetPeptideInfo
    {
        public int Id { get; set; }
        public string CleanPeptide { get; set; }
        public string Peptide { get; set; }
        public string PeptideWithNumericMods { get; set; }

        //Added Target List
        public IList<Evidence> Evidences { get; set; }
    }
}

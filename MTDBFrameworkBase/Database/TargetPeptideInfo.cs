namespace MTDBFrameworkBase.Database
{
    /// <summary>
    /// Peptide Information container
    /// </summary>
    public class TargetPeptideInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TargetPeptideInfo()
        {
            Peptide = "";
            PeptideWithNumericMods = "";
            CleanPeptide = "";
            //Evidences = new List<Evidence>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Clean Peptide Sequence
        /// </summary>
        public string CleanPeptide { get; set; }

        /// <summary>
        /// Peptide Sequence with Mod Symbols
        /// </summary>
        public string Peptide { get; set; }

        /// <summary>
        /// Peptide Sequence with numeric mods
        /// </summary>
        public string PeptideWithNumericMods { get; set; }

        ///// <summary>
        ///// Added Target List
        ///// </summary>
        //public IList<Evidence> Evidences { get; set; }
    }
}

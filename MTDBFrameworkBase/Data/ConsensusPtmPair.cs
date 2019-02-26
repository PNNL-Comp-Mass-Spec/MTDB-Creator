namespace MTDBFrameworkBase.Data
{
    /// <summary>
    /// Relation between a ConsensusTarget object and a PostTranslationalModification object for database use
    /// </summary>
    public class ConsensusPtmPair
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the consensus target
        /// </summary>
        public int ConsensusId { get; set; }

        /// <summary>
        /// Id of the PostTranslationalModification
        /// </summary>
        public int PtmId { get; set; }

        /// <summary>
        /// Location in the peptide which was modified
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// Consensus target this pair maps to
        /// </summary>
        public ConsensusTarget Target { get; set; }

        /// <summary>
        /// Post Translational Modification this pair maps to
        /// </summary>
        public PostTranslationalModification PostTranslationalModification { get; set; }
    }
}

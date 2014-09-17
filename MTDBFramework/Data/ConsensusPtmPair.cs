namespace MTDBFramework.Data
{
    /// <summary>
    /// Relation between a ConsensusTarget object and a PostTranslationalModification object for database use
    /// </summary>
    public class ConsensusPtmPair
    {
        private int m_id;
        private int m_consensusId;
        private int m_ptmId;
        private int m_location;
        private ConsensusTarget m_target;
        private PostTranslationalModification m_ptm;

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// Id of the consensus target
        /// </summary>
        public int ConsensusId
        {
            get { return m_consensusId; }
            set { m_consensusId = value; }
        }

        /// <summary>
        /// Id of the PostTranslationalModification
        /// </summary>
        public int PtmId
        {
            get { return m_ptmId; }
            set { m_ptmId = value; }
        }

        /// <summary>
        /// Location in the peptide which was modified
        /// </summary>
        public int Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        /// <summary>
        /// Consensus target this pair maps to
        /// </summary>
        public ConsensusTarget Target
        {
            get { return m_target; }
            set { m_target = value; }
        }

        /// <summary>
        /// Post Translational Modification this pair maps to
        /// </summary>
        public PostTranslationalModification PostTranslationalModification
        {
            get { return m_ptm; }
            set { m_ptm = value; }
        }

    }
}

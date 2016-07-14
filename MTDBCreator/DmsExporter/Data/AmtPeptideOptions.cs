namespace MTDBCreator.DmsExporter.Data
{
    public class AmtPeptideOptions
    {
        private decimal m_pmtQualityScore;
        private int m_mtCountPassing;
        private int m_filterSetId;
        private string m_filterSetName;
        private string m_filterSetDescription;

        public decimal PmtQualityScore
        {
            get
            {
                return m_pmtQualityScore;
            }
            set
            {
                m_pmtQualityScore = value;

            }
        }

        public int MtCountPassing
        {
            get { return m_mtCountPassing; }
            set
            {
                m_mtCountPassing = value;

            }
        }

        public int FilterSetId
        {
            get { return m_filterSetId; }
            set
            {
                m_filterSetId = value;

            }
        }

        public string FilterSetName
        {
            get { return m_filterSetName; }
            set
            {
                m_filterSetName = value;

            }
        }

        public string FilterSetDescription
        {
            get { return m_filterSetDescription; }
            set
            {
                m_filterSetDescription = value;

            }
        }
    }
}

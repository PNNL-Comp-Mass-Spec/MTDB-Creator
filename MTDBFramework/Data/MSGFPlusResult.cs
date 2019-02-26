using MTDBFrameworkBase.Data;

namespace MTDBFramework.Data
{
    /// <summary>
    /// MSGF+ input storage for evidence
    /// </summary>
    public class MsgfPlusResult : Evidence
    {
        private string m_reference;
        private short m_numTrypticEnds;
        private int m_deNovoScore;
        private int m_msgfScore;
        private double m_specEValue;
        private int m_rankSpecEValue;
        private double m_eValue;
        private double m_qValue;        // holds FDR if old MSGF-DB results
        private double m_pepQValue;
        private int m_isotopeError;

        /// <summary>
        /// Reference
        /// </summary>
        public string Reference
        {
            get => m_reference;
            set
            {
                m_reference = value;
                OnPropertyChanged("Reference");
            }
        }

        /// <summary>
        /// Number of Tryptic Ends
        /// </summary>
        public short NumTrypticEnds
        {
            get => m_numTrypticEnds;
            set
            {
                m_numTrypticEnds = value;
                OnPropertyChanged("NumTrypticEnds");
            }
        }

        /// <summary>
        /// DeNovo Score
        /// </summary>
        public int DeNovoScore
        {
            get => m_deNovoScore;
            set
            {
                m_deNovoScore = value;
                OnPropertyChanged("DeNovoScore");
            }
        }

        /// <summary>
        /// MSGF Score
        /// </summary>
        public int MsgfScore
        {
            get => m_msgfScore;
            set
            {
                m_msgfScore = value;

                switch (Charge)
                {
                    case 1:
                        NormalizedScore = (0.0197 * value) + 0.75;
                        break;

                    case 2:
                        NormalizedScore = (0.0165*value) + 1.3;
                        break;

                    default:
                        NormalizedScore = (0.0267*value) + 1;
                        break;
                }
                if (NormalizedScore < 0)
                {
                    NormalizedScore = 0;
                }

                OnPropertyChanged("MSGFScore");
            }
        }

        /// <summary>
        /// Spec E Value
        /// </summary>
        public double SpecEValue
        {
            get => m_specEValue;
            set
            {
                m_specEValue = value;
                OnPropertyChanged("SpecEValue");
            }
        }

        /// <summary>
        /// Rank Spec E Value
        /// </summary>
        public int RankSpecEValue
        {
            get => m_rankSpecEValue;
            set
            {
                m_rankSpecEValue = value;
                OnPropertyChanged("Rank_SpecEValue");
            }
        }

        /// <summary>
        /// E Value
        /// </summary>
        public double EValue
        {
            get => m_eValue;
            set
            {
                m_eValue = value;
                OnPropertyChanged("EValue");
            }
        }

        /// <summary>
        /// Q Value
        /// </summary>
        public double QValue
        {
            get => m_qValue;
            set
            {
                m_qValue = value;
                OnPropertyChanged("QValue");
            }
        }

        /// <summary>
        /// Peptide Q Value
        /// </summary>
        public double PepQValue
        {
            get => m_pepQValue;
            set
            {
                m_pepQValue = value;
                OnPropertyChanged("PepQValue");
            }
        }

        /// <summary>
        /// Isotope Error
        /// </summary>
        public int IsotopeError
        {
            get => m_isotopeError;
            set
            {
                m_isotopeError = value;
                OnPropertyChanged("IsotopeError");
            }
        }

    }
}

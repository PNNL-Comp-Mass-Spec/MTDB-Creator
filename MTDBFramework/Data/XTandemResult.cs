using MTDBFrameworkBase.Data;

namespace MTDBFramework.Data
{
    /// <summary>
    /// X!Tandem input storage for evidence
    /// </summary>
    public class XTandemResult : Evidence
    {
        #region Private Fields

        private int m_groupId;
        private double m_bScore;
        private double m_deltaCn2;
        private double m_logIntensity;
        private double m_logPeptideEValue;
        private short m_numberYIons;
        private short m_numberBIons;
        private double m_peptideHyperscore;
        private double m_yScore;
        private short m_numTrypticEnds;

        #endregion

        #region Public Properties

        /// <summary>
        /// Group Id
        /// </summary>
        public int GroupId
        {
            get => m_groupId;
            set
            {
                m_groupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        /// <summary>
        /// B Score
        /// </summary>
        public double BScore
        {
            get => m_bScore;
            set
            {
                m_bScore = value;
                OnPropertyChanged("BScore");
            }
        }

        /// <summary>
        /// Delta CN2
        /// </summary>
        public double DeltaCn2
        {
            get => m_deltaCn2;
            set
            {
                m_deltaCn2 = value;
                OnPropertyChanged("DeltaCn2");
            }
        }

        /// <summary>
        /// Log Intensity
        /// </summary>
        public double LogIntensity
        {
            get => m_logIntensity;
            set
            {
                m_logIntensity = value;
                OnPropertyChanged("LogIntensity");
            }
        }

        /// <summary>
        /// Log Peptide E Value
        /// </summary>
        public double LogPeptideEValue
        {
            get => m_logPeptideEValue;
            set
            {
                m_logPeptideEValue = value;
                OnPropertyChanged("LogPeptideEValue");
            }
        }

        /// <summary>
        /// Number of Y Ions
        /// </summary>
        public short NumberYIons
        {
            get => m_numberYIons;
            set
            {
                m_numberYIons = value;
                OnPropertyChanged("NumberYIons");
            }
        }

        /// <summary>
        /// Number of B Ions
        /// </summary>
        public short NumberBIons
        {
            get => m_numberBIons;
            set
            {
                m_numberBIons = value;
                OnPropertyChanged("NumberBIons");
            }
        }

        /// <summary>
        /// Peptide Hyperscore
        /// </summary>
        public double PeptideHyperscore
        {
            get => m_peptideHyperscore;
            set
            {
                m_peptideHyperscore = value;

                switch (Charge)
                {
                    case 1:
                        NormalizedScore = 0.082*value;
                        break;

                    case 2:
                        NormalizedScore = 0.085*value;
                        break;

                    default:
                        NormalizedScore = 0.0874*value;
                        break;
                }


                OnPropertyChanged("PeptideHyperscore");
            }
        }

        /// <summary>
        /// Y Score
        /// </summary>
        public double YScore
        {
            get => m_yScore;
            set
            {
                m_yScore = value;
                OnPropertyChanged("YScore");
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
                OnPropertyChanged("TrypticState");
            }
        }

        #endregion
    }
}
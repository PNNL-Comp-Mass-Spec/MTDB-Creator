#region Namespaces

using System;

#endregion

namespace MTDBFramework.Data
{
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
        private double m_highNormalizedScore;

        #endregion

        #region Public Properties

        public int GroupId
        {
            get
            {
                return m_groupId;
            }
            set
            {
                m_groupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        public double BScore
        {
            get
            {
                return m_bScore;
            }
            set
            {
                m_bScore = value;
                OnPropertyChanged("BScore");
            }
        }

        public double DeltaCn2
        {
            get
            {
                return m_deltaCn2;
            }
            set
            {
                m_deltaCn2 = value;
                OnPropertyChanged("DeltaCn2");
            }
        }

        public double LogIntensity
        {
            get
            {
                return m_logIntensity;
            }
            set
            {
                m_logIntensity = value;
                OnPropertyChanged("LogIntensity");
            }
        }

        public double LogPeptideEValue
        {
            get
            {
                return m_logPeptideEValue;
            }
            set
            {
                m_logPeptideEValue = value;
                OnPropertyChanged("LogPeptideEValue");
            }
        }

        public short NumberYIons
        {
            get
            {
                return m_numberYIons;
            }
            set
            {
                m_numberYIons = value;
                OnPropertyChanged("NumberYIons");
            }
        }

        public short NumberBIons
        {
            get
            {
                return m_numberBIons;
            }
            set
            {
                m_numberBIons = value;
                OnPropertyChanged("NumberBIons");
            }
        }

        public double PeptideHyperscore
        {
            get
            {
                return m_peptideHyperscore;
            }
            set
            {
                m_peptideHyperscore = value;
                OnPropertyChanged("PeptideHyperscore");
            }
        }

        public double YScore
        {
            get
            {
                return m_yScore;
            }
            set
            {
                m_yScore = value;
                OnPropertyChanged("YScore");
            }
        }

        public short NumTrypticEnds
        {
            get
            {
                return m_numTrypticEnds;
            }
            set
            {
                m_numTrypticEnds = value;
                OnPropertyChanged("TrypticState");
            }
        }

        [Obsolete("This property is not used anywhere in this project")]
        public double HighNormalizedScore
        {
            get
            {
                return m_highNormalizedScore;
            }
            set
            {
                m_highNormalizedScore = value;
                OnPropertyChanged("HighNormalizedScore");
            }
        }

        #endregion        
    }
}
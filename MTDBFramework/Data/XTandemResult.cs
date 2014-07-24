#region Namespaces

using System;

#endregion

namespace MTDBFramework.Data
{
	/// <summary>
	/// XTandem input storage for evidence
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

		/// <summary>
		/// B Score
		/// </summary>
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

		/// <summary>
		/// Delta CN2
		/// </summary>
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

		/// <summary>
		/// Log Intensity
		/// </summary>
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

		/// <summary>
		/// Log Peptide E Value
		/// </summary>
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

		/// <summary>
		/// Number of Y Ions
		/// </summary>
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

		/// <summary>
		/// Number of B Ions
		/// </summary>
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

		/// <summary>
		/// Peptide Hyperscore
		/// </summary>
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

		/// <summary>
		/// Y Score
		/// </summary>
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

		/// <summary>
		/// Number of Tryptic Ends
		/// </summary>
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

        #endregion        
    }
}
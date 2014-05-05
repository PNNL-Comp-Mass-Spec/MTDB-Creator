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
        private short m_trypticState;
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

        public short TrypticState
        {
            get
            {
                return m_trypticState;
            }
            set
            {
                m_trypticState = value;
                OnPropertyChanged("TrypticState");
            }
        }

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

        public static short CalculateTrypticState(string peptide)
        {
            short trypticState = 0;
            var peptideChar = peptide.ToCharArray();
            var startIndex = 2;
            var stopIndex = peptideChar.Length - 3;

            if (peptideChar[1] != '.')
            {
                startIndex = 0;

                throw new ApplicationException(String.Format("Peptide {0} does not have a . in the second position", peptide));
            }

            if (peptideChar[stopIndex + 1] != '.')
            {
                stopIndex = peptideChar.Length - 1;

                throw new ApplicationException(String.Format("Peptide {0} does not have a . in the second last position", peptide));
            }

            if (peptideChar[stopIndex] == 'R' || peptideChar[stopIndex] == 'K')
            {
                trypticState++;

                if (peptideChar[peptideChar.Length - 1] == 'P')
                {
                    trypticState--;
                }
            }
            else if (!Char.IsLetter(peptideChar[stopIndex]))
            {
                if (peptideChar[stopIndex - 1] == 'R' || peptideChar[stopIndex - 1] == 'K')
                {
                    trypticState++;

                    if (peptideChar[peptideChar.Length - 1] == 'P')
                    {
                        trypticState--;
                    }
                }
            }

            if (peptideChar[peptideChar.Length - 1] == '-' && trypticState == 0)
            {
                trypticState++;
            }

            if (peptideChar[0] == 'R' || peptideChar[0] == 'K')
            {
                trypticState++;

                if (peptideChar[startIndex] == 'P')
                {
                    trypticState--;
                }
            }
            else if (peptideChar[0] == '-')
            {
                trypticState++;
            }

            return trypticState;
        }
    }
}
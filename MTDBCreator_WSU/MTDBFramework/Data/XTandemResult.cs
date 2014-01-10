#region Namespaces

using System;

#endregion

namespace MTDBFramework.Data
{
    public class XTandemResult : Target
    {
        #region Private Fields

        private int m_GroupId;
        private double m_BScore;
        private double m_DeltaCn2;
        private double m_DeltaMass;
        private double m_LogIntensity;
        private double m_LogPeptideEValue;
        private short m_NumberYIons;
        private short m_NumberBIons;
        private double m_PeptideHyperscore;
        private double m_YScore;
        private short m_TrypticState;
        private double m_HighNormalizedScore;

        #endregion

        #region Public Properties

        public int GroupId
        {
            get
            {
                return m_GroupId;
            }
            set
            {
                m_GroupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        public double BScore
        {
            get
            {
                return m_BScore;
            }
            set
            {
                m_BScore = value;
                OnPropertyChanged("BScore");
            }
        }

        public double DeltaCn2
        {
            get
            {
                return m_DeltaCn2;
            }
            set
            {
                m_DeltaCn2 = value;
                OnPropertyChanged("DeltaCn2");
            }
        }

        public double DeltaMass
        {
            get
            {
                return m_DeltaMass;
            }
            set
            {
                m_DeltaMass = value;
                OnPropertyChanged("DeltaMass");
            }
        }

        public double LogIntensity
        {
            get
            {
                return m_LogIntensity;
            }
            set
            {
                m_LogIntensity = value;
                OnPropertyChanged("LogIntensity");
            }
        }

        public double LogPeptideEValue
        {
            get
            {
                return m_LogPeptideEValue;
            }
            set
            {
                m_LogPeptideEValue = value;
                OnPropertyChanged("LogPeptideEValue");
            }
        }

        public short NumberYIons
        {
            get
            {
                return m_NumberYIons;
            }
            set
            {
                m_NumberYIons = value;
                OnPropertyChanged("NumberYIons");
            }
        }

        public short NumberBIons
        {
            get
            {
                return m_NumberBIons;
            }
            set
            {
                m_NumberBIons = value;
                OnPropertyChanged("NumberBIons");
            }
        }

        public double PeptideHyperscore
        {
            get
            {
                return m_PeptideHyperscore;
            }
            set
            {
                m_PeptideHyperscore = value;
                OnPropertyChanged("PeptideHyperscore");
            }
        }

        public double YScore
        {
            get
            {
                return m_YScore;
            }
            set
            {
                m_YScore = value;
                OnPropertyChanged("YScore");
            }
        }

        public short TrypticState
        {
            get
            {
                return m_TrypticState;
            }
            set
            {
                m_TrypticState = value;
                OnPropertyChanged("TrypticState");
            }
        }

        public double HighNormalizedScore
        {
            get
            {
                return m_HighNormalizedScore;
            }
            set
            {
                m_HighNormalizedScore = value;
                OnPropertyChanged("HighNormalizedScore");
            }
        }

        #endregion

        public static short CalculateTrypticState(string peptide)
        {
            short trypticState = 0;
            char[] peptideChar = peptide.ToCharArray();
            int startIndex = 2;
            int stopIndex = peptideChar.Length - 3;

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
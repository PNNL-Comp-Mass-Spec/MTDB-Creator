using System;

namespace MTDBFramework.Data
{
    public class MsgfPlusResult : Evidence
    {
        private double m_precursorMonoMass;
        private double m_precursorMz;
        private string m_reference;
        private short m_numTrypticEnds;
        private double m_fdr;
        private int m_deNovoScore;
        private int m_msgfScore;
        private double m_specEValue;
        private int m_rankSpecEValue;
        private double m_eValue;
        private double m_qValue;
        private double m_pepQValue;
        private int m_isotopeError;

        public double PrecursorMonoMass
        {
            get
            {
                return m_precursorMonoMass;
            }
            set
            {
                m_precursorMonoMass = value;
                OnPropertyChanged("PrecursorMonoMass");
            }
        }
        public double PrecursorMz 
        {
            get
            {
                return m_precursorMz;
            }
            set
            {
                 m_precursorMz = value;
                OnPropertyChanged("PrecursorMZ");
            }
        }
        public string Reference
        {
            get
            {
                return m_reference;
            }
            set
            {
                 m_reference = value;
                OnPropertyChanged("Reference");
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
                OnPropertyChanged("NumTrypticEnds");
            }
        }
        public double Fdr
        {
            get
            {
                return m_fdr;
            }
            set
            {
                m_fdr = value;
                OnPropertyChanged("FDR");
            }
        }
        public int DeNovoScore
        {
            get
            {
                return m_deNovoScore;
            }
            set
            {
                m_deNovoScore = value;
                OnPropertyChanged("DeNovoScore");
            }
        }
        public int MsgfScore
        {
            get
            {
                return m_msgfScore;
            }
            set
            {
                m_msgfScore = value;
                OnPropertyChanged("MSGFScore");
            }
        }
        public double SpecEValue
        {
            get
            {
                return m_specEValue;
            }
            set
            {
                m_specEValue = value;
                OnPropertyChanged("SpecEValue");
            }
        }
        public int RankSpecEValue
        {
            get
            {
                return m_rankSpecEValue;
            }
            set
            {
                m_rankSpecEValue = value;
                OnPropertyChanged("Rank_SpecEValue");
            }
        }
        public double EValue
        {
            get
            {
                return m_eValue;
            }
            set
            {
                m_eValue = value;
                OnPropertyChanged("EValue");
            }
        }
        public double QValue 
        {
            get
            {
                return m_qValue;
            }
            set
            {
                m_qValue = value;
                OnPropertyChanged("QValue");
            }
        }
        public double PepQValue
        {
            get
            {
                return m_pepQValue;
            }
            set
            {
                m_pepQValue = value;
                OnPropertyChanged("PepQValue");
            }
        }
        public int IsotopeError
        {
            get
            {
                return m_isotopeError;
            }
            set
            {
                m_isotopeError = value;
                OnPropertyChanged("IsotopeError");
            }
        }

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

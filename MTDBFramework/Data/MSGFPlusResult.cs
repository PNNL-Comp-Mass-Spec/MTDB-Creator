using System;

namespace MTDBFramework.Data
{
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
       
    }	
}

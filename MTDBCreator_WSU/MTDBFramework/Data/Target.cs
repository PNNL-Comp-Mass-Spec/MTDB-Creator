#region Namespaces

using MTDBFramework.Algorithms.Clustering;
using MTDBFramework.Database;
using MTDBFramework.UI;
using NHibernate.Hql.Classic;
using PNNLOmics.Data;

#endregion

namespace MTDBFramework.Data
{
    public class Target : ObservableObject
    {
        #region Private Fields

        private MSSpectra m_Spectrum;

        private TargetDataSet m_DataSet;
        private TargetPeptideInfo m_PeptideInfo;

        private int m_Id;
        private int m_Scan;
        private int m_AnalysisId;
        private short m_MultiProteinCount;
        private double m_MonoisotopicMass;
        private double m_observedMonoMass;

        private ConsensusTarget m_Parent;

        private short m_Charge;
        private double m_ObservedNet;
        private double m_PredictedNet;
        private double m_Mz;

        private string m_Sequence;
        private string m_CleanPeptide;

        private double m_delM;
        private double m_delM_PPM;

        // SequenceInfo fields
        private short m_IsSeqInfoExist;
        private short m_ModificationCount;
        private string m_ModificationDescription;
        private double m_SeqInfoMonoisotopicMass;
        private string m_SeqWithNumericMods;

        #endregion

        #region Public Properties

        // Not used - Put off
        public MSSpectra Spectrum
        {
            get
            {
                return m_Spectrum;
            }
            set
            {
                m_Spectrum = value;
                OnPropertyChanged("Spectrum");
            }
        }

        //For 3NF:
        public TargetDataSet DataSet
        {
            get { return m_DataSet; }
            set
            {
                m_DataSet = value;
                OnPropertyChanged("DataSet");
            }
        }

        // Metadata
        public TargetPeptideInfo PeptideInfo
        {
            get { return m_PeptideInfo; }
            set
            {
                m_PeptideInfo = value;
                OnPropertyChanged("PeptideInfo");
            }
        }

        public int Id
        {
            get { return m_Id; }
            set
            {
                m_Id = value; 
                OnPropertyChanged("Id");
            }
        }

        public int Scan
        {
            get { return m_Scan; }
            set
            {
                m_Scan = value; 
                OnPropertyChanged("Scan");
            }
        }

        public int AnalysisId
        {
            get { return m_AnalysisId; }
            set
            {
                m_AnalysisId = value; 
                OnPropertyChanged("AnalysisId");
            }
        }

        public short MultiProteinCount
        {
            get { return m_MultiProteinCount; }
            set
            {
                m_MultiProteinCount = value; 
                OnPropertyChanged("MultiProteinCount");
            }
        }

        public double MonoisotopicMass
        {
            get { return m_MonoisotopicMass; }
            set
            {
                m_MonoisotopicMass = value;
                OnPropertyChanged("MonoisotopicMass");
            }
        }

        public double ObservedMonoisotopicMass
        {
            get { return m_observedMonoMass; }
            set
            {
                m_observedMonoMass = value;
                OnPropertyChanged("MonoisotopicMass");
            }
        }

        public ConsensusTarget Parent
        {
            get { return m_Parent; }
            set
            {
                m_Parent = value; 
                OnPropertyChanged("Parent");
            }
        }

        public short Charge
        {
            get { return m_Charge; }
            set
            {
                m_Charge = value;
                OnPropertyChanged("Charge");
            }
        }

        public double ObservedNet
        {
            get { return m_ObservedNet; }
            set
            {
                m_ObservedNet = value;
                OnPropertyChanged("ObservedNet");
            }
        }

        public double PredictedNet
        {
            get { return m_PredictedNet; }
            set
            {
                m_PredictedNet = value;
                OnPropertyChanged("PredictedNet");
            }
        }

        public double Mz
        {
            get { return m_Mz; }
            set
            {
                m_Mz = value;
                OnPropertyChanged("Mz");
            }
        }

        // PeptideInfo

        public string Sequence
        {
            get { return m_Sequence; }
            set
            {
                m_Sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        public string CleanPeptide
        {
            get { return m_CleanPeptide; }
            set
            {
                m_CleanPeptide = value;
                OnPropertyChanged("CleanPeptide");
            }
        }

        public double DelM
        {
            get
            {
                return m_delM;
            }
            set
            {
                m_delM = value;
                OnPropertyChanged("DelM");
            }
        }
        public double DelM_PPM
        {
            get
            {
                return m_delM_PPM;
            }
            set
            {
                m_delM_PPM = value;
                OnPropertyChanged("DelM_PPM");
            }
        }

        // SequenceInfo

        public short IsSeqInfoExist
        {
            get { return m_IsSeqInfoExist; }
            set
            {
                m_IsSeqInfoExist = value;
                OnPropertyChanged("IsSeqInfoExist");
            }
        }

        public short ModificationCount
        {
            get { return m_ModificationCount; }
            set
            {
                m_ModificationCount = value;
                OnPropertyChanged("ModificationCount");
            }
        }

        public string ModificationDescription
        {
            get { return m_ModificationDescription; }
            set
            {
                m_ModificationDescription = value;
                OnPropertyChanged("ModificationDescription");
            }
        }

        public double SeqInfoMonoisotopicMass
        {
            get { return m_SeqInfoMonoisotopicMass; }
            set
            {
                m_SeqInfoMonoisotopicMass = value;
                OnPropertyChanged("SeqInfoMonoisotopicMass");
            }
        }

        public string SeqWithNumericMods
        {
            get { return m_SeqWithNumericMods; }
            set
            {
                m_SeqWithNumericMods = value;
                OnPropertyChanged("SeqWithNumericMods");
            }
        }

        public double SpectralProbability { get; set; }

        #endregion

        public static string CleanSequence(string sequence)
        {
            char[] peptideChar = sequence.ToCharArray();
            int startIndex = 2;
            int stopIndex = peptideChar.Length - 3;

            if (peptideChar[1] != '.')
            {
                startIndex = 0;
            }

            if (peptideChar[stopIndex + 1] != '.')
            {
                stopIndex = peptideChar.Length - 1;
            }

            int copyToIndex = startIndex;
            int copyFromIndex = startIndex;

            while (copyFromIndex <= stopIndex)
            {
                while (copyFromIndex <= stopIndex && (peptideChar[copyFromIndex] == '&' || peptideChar[copyFromIndex] == '*'))
                {
                    copyFromIndex++;
                }

                if (copyFromIndex > stopIndex)
                {
                    break;
                }

                peptideChar[copyToIndex++] = peptideChar[copyFromIndex++];
            }

            string cleanPeptide = new string(peptideChar, startIndex, copyToIndex - startIndex);

            return cleanPeptide;
        }

        public override string ToString()
        {
            return this.Sequence;
        }
    }
}
#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Database;
using MTDBFramework.UI;
using PNNLOmics.Data;

#endregion

namespace MTDBFramework.Data
{
    public class Evidence : ObservableObject
    {
        #region Private Fields

        private MSSpectra m_spectrum;

        private TargetDataSet m_dataSet;
        private TargetPeptideInfo m_peptideInfo;

        private int m_id;
        private int m_scan;
        private int m_analysisId;
        private short m_multiProteinCount;
        private double m_monoisotopicMass;
        private double m_observedMonoMass;

        private ConsensusTarget m_parent;

        private short m_charge;
        private double m_observedNet;
        private double m_predictedNet;
        private double m_mz;

        private string m_sequence;
        private string m_cleanPeptide;

        private double m_delM;
        private double m_delMPpm;

        private double m_specProb;

        private IList<ProteinInformation> m_proteins;

        // SequenceInfo fields
        private short m_isSeqInfoExist;
        private short m_modificationCount;
        private string m_modificationDescription;
        private double m_seqInfoMonoisotopicMass;
        private string m_seqWithNumericMods;

        #endregion


        public Evidence()
        {
            m_proteins = new List<ProteinInformation>();

            DataSet = new TargetDataSet{
                Path = "",
                Name = ""
            };
        }

        #region Public Properties

        // Not used - Put off
        public MSSpectra Spectrum
        {
            get
            {
                return m_spectrum;
            }
            set
            {
                m_spectrum = value;
                OnPropertyChanged("Spectrum");
            }
        }

        //For 3NF:
        public TargetDataSet DataSet
        {
            get { return m_dataSet; }
            set
            {
                m_dataSet = value;
                OnPropertyChanged("DataSet");
            }
        }

        // Metadata
        public TargetPeptideInfo PeptideInfo
        {
            get { return m_peptideInfo; }
            set
            {
                m_peptideInfo = value;
                OnPropertyChanged("PeptideInfo");
            }
        }

        public int Id
        {
            get { return m_id; }
            set
            {
                m_id = value; 
                OnPropertyChanged("Id");
            }
        }

        public int Scan
        {
            get { return m_scan; }
            set
            {
                m_scan = value; 
                OnPropertyChanged("Scan");
            }
        }

        public int AnalysisId
        {
            get { return m_analysisId; }
            set
            {
                m_analysisId = value; 
                OnPropertyChanged("AnalysisId");
            }
        }

        public short MultiProteinCount
        {
            get { return m_multiProteinCount; }
            set
            {
                m_multiProteinCount = value; 
                OnPropertyChanged("MultiProteinCount");
            }
        }

        public double MonoisotopicMass
        {
            get { return m_monoisotopicMass; }
            set
            {
                m_monoisotopicMass = value;
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
            get { return m_parent; }
            set
            {
                m_parent = value; 
                OnPropertyChanged("Parent");
            }
        }

        public short Charge
        {
            get { return m_charge; }
            set
            {
                m_charge = value;
                OnPropertyChanged("Charge");
            }
        }

        public double ObservedNet
        {
            get { return m_observedNet; }
            set
            {
                m_observedNet = value;
                OnPropertyChanged("ObservedNet");
            }
        }

        public double PredictedNet
        {
            get { return m_predictedNet; }
            set
            {
                m_predictedNet = value;
                OnPropertyChanged("PredictedNet");
            }
        }

        public double Mz
        {
            get { return m_mz; }
            set
            {
                m_mz = value;
                OnPropertyChanged("Mz");
            }
        }

        public double SpecProb
        {
            get { return m_specProb; }
            set
            {
                m_specProb = value;
                OnPropertyChanged("SpecProb");
            }
        }

        // PeptideInfo

        public string Sequence
        {
            get { return m_sequence; }
            set
            {
                m_sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        public string CleanPeptide
        {
            get { return m_cleanPeptide; }
            set
            {
                m_cleanPeptide = value;
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
        public double DelMPpm
        {
            get
            {
                return m_delMPpm;
            }
            set
            {
                m_delMPpm = value;
                OnPropertyChanged("DelM_PPM");
            }
        }

        // SequenceInfo

        public short IsSeqInfoExist
        {
            get { return m_isSeqInfoExist; }
            set
            {
                m_isSeqInfoExist = value;
                OnPropertyChanged("IsSeqInfoExist");
            }
        }

        public short ModificationCount
        {
            get { return m_modificationCount; }
            set
            {
                m_modificationCount = value;
                OnPropertyChanged("ModificationCount");
            }
        }

        public string ModificationDescription
        {
            get { return m_modificationDescription; }
            set
            {
                m_modificationDescription = value;
                OnPropertyChanged("ModificationDescription");
            }
        }

        public double SeqInfoMonoisotopicMass
        {
            get { return m_seqInfoMonoisotopicMass; }
            set
            {
                m_seqInfoMonoisotopicMass = value;
                OnPropertyChanged("SeqInfoMonoisotopicMass");
            }
        }

        public string SeqWithNumericMods
        {
            get { return m_seqWithNumericMods; }
            set
            {
                m_seqWithNumericMods = value;
                OnPropertyChanged("SeqWithNumericMods");
            }
        }

        public double SpectralProbability { get; set; }

        public IList<ProteinInformation> Proteins
        {
            get { return m_proteins; }
            set
            {
                m_proteins = value;
                OnPropertyChanged("Proteins");
            }
        }


        #endregion

        public void AddProtein(ProteinInformation protein)
        {
            Proteins.Add(protein);
        }

        public static string CleanSequence(string sequence)
        {
            var peptideChar = sequence.ToCharArray();
            var startIndex = 2;
            var stopIndex = peptideChar.Length - 3;

            if (peptideChar[1] != '.')
            {
                startIndex = 0;
            }

            if (peptideChar[stopIndex + 1] != '.')
            {
                stopIndex = peptideChar.Length - 1;
            }

            var copyToIndex = startIndex;
            var copyFromIndex = startIndex;

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

            var cleanPeptide = new string(peptideChar, startIndex, copyToIndex - startIndex);

            return cleanPeptide;
        }

        public override string ToString()
        {
            return Sequence;
        }
    }
}
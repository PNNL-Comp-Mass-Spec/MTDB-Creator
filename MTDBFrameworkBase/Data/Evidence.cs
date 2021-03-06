﻿#region Namespaces

using System;
using System.Collections.Generic;
using System.ComponentModel;
using MTDBFrameworkBase.Database;

#endregion

namespace MTDBFrameworkBase.Data
{
    /// <summary>
    /// Collection of data pertaining to a single peptide evidence
    /// </summary>
    public class Evidence : INotifyPropertyChanged, IComparable<Evidence>
    {
        #region Private Fields

        private TargetDataSet m_dataSet;
        private TargetPeptideInfo m_peptideInfo;

        private int m_id;
        private int m_scan;
        private int m_analysisId;
        private short m_multiProteinCount;
        private double m_monoisotopicMass;
        private double m_observedMonoMass;

        private ConsensusTarget m_parent;

        private IList<PostTranslationalModification> m_ptms;

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

        private double m_discriminantValue; // holder for QValue (msgf+/mzid), Log Peptide E Value (X!Tandem),
                                            // xCorr (sequest) or EValue (MSAlign) depending on dataset type.

        // SequenceInfo fields
        private short m_isSeqInfoExist;
        private short m_modificationCount;
        private string m_modificationDescription;
        private double m_seqInfoMonoisotopicMass;
        private string m_seqWithNumericMods;
        private string m_encodedNonNumericSequence;

        private double m_netShift;

        private double m_normalizedScore;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Evidence()
        {
            m_proteins = new List<ProteinInformation>();
            m_ptms = new List<PostTranslationalModification>();

            DataSet = new TargetDataSet{
                Path = "",
                Name = ""
            };
        }

        #region Public Properties

        //For 3NF:
        /// <summary>
        /// Dataset the evidence occurs in
        /// </summary>
        public TargetDataSet DataSet
        {
            get => m_dataSet;
            set
            {
                m_dataSet = value;
                OnPropertyChanged("DataSet");
            }
        }

        // Metadata
        /// <summary>
        /// Peptide that is evidenced
        /// </summary>
        public TargetPeptideInfo PeptideInfo
        {
            get => m_peptideInfo;
            set
            {
                m_peptideInfo = value;
                OnPropertyChanged("PeptideInfo");
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get => m_id;
            set
            {
                m_id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Scan id number
        /// </summary>
        public int Scan
        {
            get => m_scan;
            set
            {
                m_scan = value;
                OnPropertyChanged("Scan");
            }
        }

        /// <summary>
        /// Analysis Id
        /// </summary>
        public int AnalysisId
        {
            get => m_analysisId;
            set
            {
                m_analysisId = value;
                OnPropertyChanged("AnalysisId");
            }
        }

        /// <summary>
        /// Number of proteins the evidenced peptide occurs in
        /// </summary>
        public short MultiProteinCount
        {
            get => m_multiProteinCount;
            set
            {
                m_multiProteinCount = value;
                OnPropertyChanged("MultiProteinCount");
            }
        }

        /// <summary>
        /// Computed monoisotopic mass (uncharged, theoretical mass, including mods)
        /// </summary>
        public double MonoisotopicMass
        {
            get => m_monoisotopicMass;
            set
            {
                m_monoisotopicMass = value;
                OnPropertyChanged("MonoisotopicMass");
            }
        }

        /// <summary>
        /// Observed monoisotopic (of the precursor if MS/MS)
        /// </summary>
        public double ObservedMonoisotopicMass
        {
            get => m_observedMonoMass;
            set
            {
                m_observedMonoMass = value;
                OnPropertyChanged("MonoisotopicMass");
            }
        }

        /// <summary>
        /// The evidenced protein
        /// </summary>
        public ConsensusTarget Parent
        {
            get => m_parent;
            set
            {
                m_parent = value;
                OnPropertyChanged("Parent");
            }
        }

        /// <summary>
        /// Charge
        /// </summary>
        public short Charge
        {
            get => m_charge;
            set
            {
                m_charge = value;
                OnPropertyChanged("Charge");
            }
        }

        /// <summary>
        /// Observed Elution time
        /// </summary>
        public double ObservedNet
        {
            get => m_observedNet;
            set
            {
                m_observedNet = value;
                OnPropertyChanged("ObservedNet");
            }
        }

        /// <summary>
        /// Predicted Elution time
        /// </summary>
        public double PredictedNet
        {
            get => m_predictedNet;
            set
            {
                m_predictedNet = value;
                OnPropertyChanged("PredictedNet");
            }
        }


        /// <summary>
        /// Observed m/z (of the precursor if MS/MS)
        /// </summary>
        public double Mz
        {
            get => m_mz;
            set
            {
                m_mz = value;
                OnPropertyChanged("Mz");
            }
        }

        /// <summary>
        /// SpecProb
        /// </summary>
        public double SpecProb
        {
            get => m_specProb;
            set
            {
                m_specProb = value;
                OnPropertyChanged("SpecProb");
            }
        }

        // PeptideInfo

        /// <summary>
        /// Sequence that is evidenced
        /// </summary>
        public string Sequence
        {
            get => m_sequence;
            set
            {
                m_sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        /// <summary>
        /// Clean peptide sequence - letters only
        /// </summary>
        public string CleanPeptide
        {
            get => m_cleanPeptide;
            set
            {
                m_cleanPeptide = value;
                if (m_cleanPeptide.Contains("."))
                {
                    m_cleanPeptide = CleanSequence(value);
                }
                OnPropertyChanged("CleanPeptide");
            }
        }

        /// <summary>
        /// DelM
        /// </summary>
        public double DelM
        {
            get => m_delM;
            set
            {
                m_delM = value;
                OnPropertyChanged("DelM");
            }
        }

        /// <summary>
        /// DelMPpm
        /// </summary>
        public double DelMPpm
        {
            get => m_delMPpm;
            set
            {
                m_delMPpm = value;
                OnPropertyChanged("DelM_PPM");
            }
        }

        // SequenceInfo

        /// <summary>
        /// Is Sequence Information Existing?
        /// </summary>
        public short IsSeqInfoExist
        {
            get => m_isSeqInfoExist;
            set
            {
                m_isSeqInfoExist = value;
                OnPropertyChanged("IsSeqInfoExist");
            }
        }

        /// <summary>
        /// Modifications occurring in sequence
        /// </summary>
        public short ModificationCount
        {
            get => m_modificationCount;
            set
            {
                m_modificationCount = value;
                OnPropertyChanged("ModificationCount");
            }
        }

        /// <summary>
        /// Description of modifications occurring in sequence
        /// </summary>
        public string ModificationDescription
        {
            get => m_modificationDescription;
            set
            {
                m_modificationDescription = value;
                OnPropertyChanged("ModificationDescription");
            }
        }

        /// <summary>
        /// Computed monoisotopic mass of the peptide sequence associated with this feature (uncharged, theoretical mass, including mods)
        /// </summary>
        public double SeqInfoMonoisotopicMass
        {
            get => m_seqInfoMonoisotopicMass;
            set
            {
                m_seqInfoMonoisotopicMass = value;
                OnPropertyChanged("SeqInfoMonoisotopicMass");
            }
        }

        /// <summary>
        /// Sequence containing numeric modifications
        /// </summary>
        public string SeqWithNumericMods
        {
            get => m_seqWithNumericMods;
            set
            {
                m_seqWithNumericMods = value;
                OnPropertyChanged("SeqWithNumericMods");
            }
        }

        /// <summary>
        /// Encoded sequence with modification data
        /// </summary>
        public string EncodedNonNumericSequence
        {
            get => m_encodedNonNumericSequence;
            set
            {
                m_encodedNonNumericSequence = value;
                OnPropertyChanged("EncodedNonNumericSequence");
            }
        }

        /// <summary>
        /// NET amount evidence was shifted by from LCMS Warp
        /// </summary>
        public double NetShift
        {
            get => m_netShift;
            set
            {
                m_netShift = value;
                OnPropertyChanged("NetShift");
            }
        }

        /// <summary>
        /// Discriminant score used in conjunction with spectral probability
        /// for filtering evidences. QValue for MSGF+ and mzId files, LogPeptide
        /// EValue for X!Tandem, xCorr for SEQUEST, EValue for MSAlign
        /// </summary>
        public double DiscriminantValue
        {
            get => m_discriminantValue;
            set
            {
                m_discriminantValue = value;
                OnPropertyChanged("QValue");
            }
        }

        /// <summary>
        /// Proteins evidenced
        /// </summary>
        public IList<ProteinInformation> Proteins
        {
            get => m_proteins;
            set
            {
                m_proteins = value;
                OnPropertyChanged("Proteins");
            }
        }

        /// <summary>
        /// Post Translational Modifications in sequence
        /// </summary>
        public IList<PostTranslationalModification> Ptms
        {
            get => m_ptms;
            set
            {
                m_ptms = value;
                OnPropertyChanged("PostTranslationalModifications");
            }
        }

        /// <summary>
        /// Normalized score for the evidence
        /// </summary>
        public double NormalizedScore
        {
            get => m_normalizedScore;
            set
            {
                m_normalizedScore = value;
                OnPropertyChanged("NormalizedScore");
            }
        }

        #endregion

        /// <summary>
        /// Add a protein to the list of proteins evidenced
        /// </summary>
        /// <param name="protein"></param>
        public void AddProtein(ProteinInformation protein)
        {
            Proteins.Add(protein);
        }

        /// <summary>
        /// Extract the clean sequence
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Overloaded string return
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Sequence;
        }

        /// <summary>
        /// Comparison function for sorting by <see cref="Sequence"/>, then by <see cref="Scan"/>, then by <see cref="MonoisotopicMass"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Evidence other)
        {
            var result = string.Compare(Sequence, other.Sequence, StringComparison.Ordinal);
            if (result == 0)
            {
                result = Scan.CompareTo(other.Scan);
                if (result == 0)
                {
                    result = MonoisotopicMass.CompareTo(other.MonoisotopicMass);
                }
            }
            return result;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
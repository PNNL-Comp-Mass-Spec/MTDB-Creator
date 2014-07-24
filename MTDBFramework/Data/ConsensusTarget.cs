using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBFramework.Data
{
	/// <summary>
	/// Store all information pertaining to a single consensus target
	/// </summary>
    public class ConsensusTarget : ObservableObject
    {
		/// <summary>
		/// Constructor
		/// </summary>
        public ConsensusTarget()
        {
            Evidences = new List<Evidence>();
            Proteins = new List<ProteinInformation>();
            Charges = new List<int>();
            ConsensusProtein = new List<ConsensusProteinPair>();
            PTMs = new List<PostTranslationalModification>();
        }

        #region Private fields
        private int m_id;
        private double m_averageNet;
        private double m_stdevNet;
        private double m_predictedNet;
        private double m_theoreticalMonoIsotopicMass;
        private string m_sequence;
        private string m_encodedNumericSequence;
        private string m_encodedNonNumericSequence; // TODO: change name to something more meaningful
        private string m_cleanSequence;
        private TargetDataSet m_dataset;
        private IList<Evidence> m_evidences;
        private IList<ProteinInformation> m_proteins;
        private IList<int> m_charges;
        private IList<PostTranslationalModification> m_ptms;
        #endregion

        #region Public Properties

		/// <summary>
		/// The Prefix Residue
		/// </summary>
        public char PrefixResidue { get { return (string.IsNullOrWhiteSpace(m_sequence)) ? '\0' : m_sequence.First(); } private set { char temp = value; } }

		/// <summary>
		/// The Suffix Residue
		/// </summary>
        public char SuffixResidue { get { return (string.IsNullOrWhiteSpace(m_sequence)) ? '\0' : m_sequence.Last(); } private set { char temp = value; } }

		/// <summary>
		/// The ConsensusTarget/ProteinInformation relations
		/// </summary>
        public IList<ConsensusProteinPair> ConsensusProtein { get; set; }

		/// <summary>
		/// Id
		/// </summary>
        public int Id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                OnPropertyChanged("Id");
            }
        }

		/// <summary>
		/// Average Elution Time
		/// </summary>
        public double AverageNet
        {
            get { return m_averageNet; }
            set
            {
                m_averageNet = value;
                OnPropertyChanged("Net");
            }
        }

		/// <summary>
		/// Elution Time Standard Deviation
		/// </summary>
        public double StdevNet
        {
            get { return m_stdevNet; }
            set
            {
                m_stdevNet = value;
                OnPropertyChanged("StdevNet");
            }
        }

		/// <summary>
		/// Predicted Elution Time
		/// </summary>
        public double PredictedNet
        {
            get { return m_predictedNet; }
            set
            {
                m_predictedNet = value;
                OnPropertyChanged("PredictedNet");
            }
        }

		/// <summary>
		/// Monoisotopic Mass - Theoretical
		/// </summary>
        public double TheoreticalMonoIsotopicMass
        {
            get { return m_theoreticalMonoIsotopicMass; }
            set
            {
                m_theoreticalMonoIsotopicMass = value;
                OnPropertyChanged("TheoreticalMonoIsotopicMass");
            }
        }

		/// <summary>
		/// Peptide Sequence
		/// </summary>
        public string Sequence
        {
            get { return m_sequence; }
            set
            {
                m_sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

		/// <summary>
		/// Peptide Sequence with numeric modifications
		/// </summary>
        public string EncodedNumericSequence
        {
            get { return m_encodedNumericSequence; }
            set 
            { 
                m_encodedNumericSequence = value;
                OnPropertyChanged("EncodedNumericSequence");
            }
        }

		/// <summary>
		/// Peptide Sequence with non-numeric modifications
		/// </summary>
        public string EncodedNonNumericSequence
        {
            get { return m_encodedNonNumericSequence; }
            set
            {
                m_encodedNonNumericSequence = value;
                OnPropertyChanged("EncodedNonNumericSequence");
            }
        }

        /// <summary>
		/// Sequence for the peptide with all PTMs excluded.
        /// </summary>
        public string CleanSequence
        {
            get { return m_cleanSequence; }
            set
            {
                m_cleanSequence = value;
                OnPropertyChanged("CleanSequence");
            }
        }

		/// <summary>
		/// Dataset the Consensus target occurs in
		/// </summary>
        public TargetDataSet Dataset
        {
            get { return m_dataset; }
            set
            {
                m_dataset = value;
                OnPropertyChanged("Dataset");
            }
        }

		/// <summary>
		/// Evidences of the ConsensusTarget
		/// </summary>
        public IList<Evidence> Evidences
        {
            get { return m_evidences; }
            set
            {
                m_evidences = value;
                OnPropertyChanged("Evidences");
            }
        }

		/// <summary>
		/// The Proteins the Consensus target occurs in
		/// </summary>
        public IList<ProteinInformation> Proteins
        {
            get { return m_proteins; }
            set
            {
                m_proteins = value;
                OnPropertyChanged("Proteins");
            }
        }

		/// <summary>
		/// The charges
		/// </summary>
        public IList<int> Charges
        {
            get { return m_charges; }
            set
            {
                m_charges = value;
                OnPropertyChanged("Charges");
            }
        }

		/// <summary>
		/// List of all Post-Translational Modifications observed in the ConsensusTarget
		/// </summary>
        public IList<PostTranslationalModification> PTMs
        {
            get { return m_ptms; }
            set
            {
                m_ptms = value;
                OnPropertyChanged("PTMs");
            }
        }
        #endregion

		/// <summary>
		/// Add another evidence of the ConsensusTarget
		/// </summary>
		/// <param name="evidence"></param>
        public void AddEvidence(Evidence evidence)
        {
            Evidences.Add(evidence);
            if (Sequence == null)
            {
                Sequence = evidence.Sequence;
            }
            evidence.Sequence = Sequence;
            if (PredictedNet == 0.0)
            {
                PredictedNet = evidence.PredictedNet;
            }
            // For rebuilding and getting the predicted NET into Evidence
            // when reloading back into the data objects
            evidence.PredictedNet = PredictedNet;

            if(MultiProteinCount == 0)
            {
                MultiProteinCount = evidence.MultiProteinCount;
            }
            evidence.MultiProteinCount = MultiProteinCount;

            if(string.IsNullOrWhiteSpace(ModificationDescription))
            {
                ModificationDescription = evidence.ModificationDescription;
            }
            evidence.ModificationDescription = ModificationDescription;
            
            if(ModificationCount == 0)
            {
                ModificationCount = evidence.ModificationCount;
            }
            evidence.ModificationCount = ModificationCount;

            if(evidence.PTMs.Count != 0 && PTMs.Count == 0)
            {
                foreach(var ptm in evidence.PTMs)
                {
                    PTMs.Add(ptm);
                    ptm.Parent = this;
                } 
            }
            var tempList = PTMs.ToList();
            tempList.Sort((x, y) => x.Location.CompareTo(y.Location));

            // Copy sequence as is up until you hit a modification
            //For numeric, add a bracket add +/- and copy the mass
            //For non numeric, add a bracket add +/- and copy the formula
            string numeric = "";
            string nonNumeric = "";
            string partialSeq = "";
            string cleanSeq = "";
            int sequencePos = 0;
            int symbolsRemoved = 0;
            foreach (var ptm in tempList)
            {
                partialSeq = Sequence.Substring(sequencePos, (ptm.Location + 2 + symbolsRemoved) - sequencePos);
                cleanSeq += partialSeq;
                numeric += partialSeq + string.Format("[{0}{1}]", ((ptm.Mass > 0) ? "+" : "-"), ptm.Mass);
                nonNumeric += partialSeq + string.Format("[{0}{1}]", ((ptm.Mass > 0) ? "+" : "-"), ptm.Formula);
                sequencePos = ptm.Location + 2;
                // To skip over non-alphanumeric characters which may show up in sequence such as "*" or "&"
                if (Sequence[sequencePos + symbolsRemoved] < 65 || Sequence[sequencePos + symbolsRemoved] > 90)
                {
                    sequencePos += ++symbolsRemoved;
                }
            }
            partialSeq = Sequence.Substring(sequencePos);
            cleanSeq += partialSeq;
            numeric += partialSeq;
            nonNumeric += partialSeq;

            CleanSequence = cleanSeq;
            if(string.IsNullOrWhiteSpace(evidence.CleanPeptide))
            {
                evidence.CleanPeptide = cleanSeq;
            }
            CleanSequence = cleanSeq;

            if (string.IsNullOrWhiteSpace(evidence.EncodedNonNumericSequence))
            {
                evidence.EncodedNonNumericSequence = nonNumeric;
            }
            EncodedNonNumericSequence = nonNumeric;

            if (!Charges.Contains(evidence.Charge))
            {
                Charges.Add(evidence.Charge);
            }

            evidence.Parent = this;
        }

		/// <summary>
		/// Add another protein the ConsensusTarget occurs in
		/// </summary>
		/// <param name="protein"></param>
        public void AddProtein(ProteinInformation protein)
        {
            Proteins.Add(protein);

            protein.Consensus.Add(this);
        }

		/// <summary>
		/// Number of modifications that were observed
		/// </summary>
        public short ModificationCount
        {
            get;
            set;
        }

		/// <summary>
		/// Descriptions for all observed modifications
		/// </summary>
        public string ModificationDescription
        {
            get;
            set;
        }

		/// <summary>
		/// The number of proteins the ConsensusTarget occurs in
		/// </summary>
        public short MultiProteinCount
        {
            get;
            set;
        }

        /// <summary>
        /// Calculate average mass and net and stdev mass and net for each Target.
        /// </summary>
        public void CalculateStatistics()
        {
            var massesList = Evidences.Select(c => c.MonoisotopicMass).ToList();
            var netList = Evidences.Select(c => c.ObservedNet).ToList();

            TheoreticalMonoIsotopicMass = massesList.Average();
            AverageNet = netList.Average();

            if (netList.Count == 1)
            {
                StdevNet = 0;
            }
            else
            {
                StdevNet = netList.StandardDeviation();
            }
        }
    }
}

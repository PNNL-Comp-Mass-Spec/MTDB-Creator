using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBFramework.Data
{
    public class ConsensusTarget : ObservableObject
    {
        public ConsensusTarget()
        {
            Evidences = new List<Evidence>();
            Proteins = new List<ProteinInformation>();
            Charges = new List<int>();
            ConsensusProtein = new List<ConsensusProteinPair>();
        }

        #region Private fields
        private int m_id;
        private double m_averageNet;
        private double m_stdevNet;
        private double m_predictedNet;
        private double m_theoreticalMonoIsotopicMass;
        private string m_sequence;
        private string m_cleanSequence;
        private TargetDataSet m_dataset;
        private IList<Evidence> m_evidences;
        private IList<ProteinInformation> m_proteins;
        private IList<int> m_charges;
        #endregion

        #region Public Properties

        public char PrefixResidue { get { return (string.IsNullOrWhiteSpace(m_sequence)) ? '\0' : m_sequence.First(); } private set { char temp = value; } }

        public char SuffixResidue { get { return (string.IsNullOrWhiteSpace(m_sequence)) ? '\0' : m_sequence.Last(); } private set { char temp = value; } }

        public IList<ConsensusProteinPair> ConsensusProtein { get; set; }

        public int Id
        {
            get { return m_id; }
            set
            {
                m_id = value;
                OnPropertyChanged("Id");
            }
        }

        public double AverageNet
        {
            get { return m_averageNet; }
            set
            {
                m_averageNet = value;
                OnPropertyChanged("Net");
            }
        }

        public double StdevNet
        {
            get { return m_stdevNet; }
            set
            {
                m_stdevNet = value;
                OnPropertyChanged("StdevNet");
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

        public double TheoreticalMonoIsotopicMass
        {
            get { return m_theoreticalMonoIsotopicMass; }
            set
            {
                m_theoreticalMonoIsotopicMass = value;
                OnPropertyChanged("TheoreticalMonoIsotopicMass");
            }
        }

        public string Sequence
        {
            get { return m_sequence; }
            set
            {
                m_sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        ///Sequence for the peptide with all PTMs excluded.
        //public string CleanSequence
        //{
        //    get { return m_cleanSequence; }
        //    set
        //    {
        //        m_cleanSequence = value;
        //        OnPropertyChanged("CleanSequence");
        //    }
        //}

        public TargetDataSet Dataset
        {
            get { return m_dataset; }
            set
            {
                m_dataset = value;
                OnPropertyChanged("Dataset");
            }
        }

        public IList<Evidence> Evidences
        {
            get { return m_evidences; }
            set
            {
                m_evidences = value;
                OnPropertyChanged("Evidences");
            }
        }

        public IList<ProteinInformation> Proteins
        {
            get { return m_proteins; }
            set
            {
                m_proteins = value;
                OnPropertyChanged("Proteins");
            }
        }

        public IList<int> Charges
        {
            get { return m_charges; }
            set
            {
                m_charges = value;
                OnPropertyChanged("Charges");
            }
        }
        #endregion

        public void AddEvidence(Evidence evidence)
        {
            Evidences.Add(evidence);

            Sequence = evidence.Sequence;
            //CleanSequence = evidence.CleanPeptide;
            if (PredictedNet == 0.0)
            {
                PredictedNet = evidence.PredictedNet;
            }
            // For rebuilding and getting the predicted NET into Evidence
            // when reloading back into the data objects
            else
            {
                evidence.PredictedNet = PredictedNet;
            }

            if (!Charges.Contains(evidence.Charge))
            {
                Charges.Add(evidence.Charge);
            }

            evidence.Parent = this;
        }

        public void AddProtein(ProteinInformation protein)
        {
            Proteins.Add(protein);

            protein.Consensus.Add(this);
        }

        public string SeqWithNumericMods
        {
            get;
            set;
        }

        public int ModificationCount
        {
            get;
            set;
        }

        public string ModificationDescription
        {
            get;
            set;
        }

        public int MultiProteinCount
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

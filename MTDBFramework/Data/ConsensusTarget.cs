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
        private double m_net; 
        private double m_stdevNet;
        private double m_predictedNet;
        private double m_theoreticalMonoIsotopicMass;
        private string m_sequence;
        private TargetDataSet m_dataset;
        private IList<Evidence> m_evidences;
        private IList<ProteinInformation> m_proteins;
        private IList<int> m_charges; 
        #endregion

        #region Public Properties

        public char PrefixResidue { get { return m_sequence.First(); } private set { char temp = value; } }

        public char SuffixResidue { get { return m_sequence.Last(); } private set { char temp = value; } }

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

        public double Net
        {
            get { return m_net; }
            set
            {
                m_net = value;
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
            PredictedNet = evidence.PredictedNet;

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

        /// <summary>
        /// Calculate average mass & net and stdev mass & net for each Target.
        /// </summary>
        public void CalculateStatistics()
        {
            var massesList = Evidences.Select(c => c.MonoisotopicMass).ToList();
            var netList = Evidences.Select(c => c.ObservedNet).ToList();

            TheoreticalMonoIsotopicMass = massesList.Average();
            Net = netList.Average();

            StdevNet = netList.StandardDeviation();
        }
    }
}

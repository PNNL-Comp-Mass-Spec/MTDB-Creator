#region Namespaces

using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Database;
using MathNet.Numerics.Statistics;
using MTDBFramework.Data;
using MTDBFramework.UI;

#endregion

namespace MTDBFramework.Algorithms.Clustering
{
    public class ConsensusTarget : ObservableObject
    {
        public ConsensusTarget()
        {
            Evidences = new List<Evidence>();
            Proteins = new List<ProteinInformation>();
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
        #endregion

        #region Public Properties

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
        #endregion

        public void AddTarget(Evidence evidence)
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

        /// <summary>
        /// Calculate average mass & net and stdev mass & net for each Target.
        /// </summary>
        public void CalculateStatistics()
        {
            List<double> massesList = Evidences.Select(c => c.MonoisotopicMass).ToList();
            List<double> netList = Evidences.Select(c => c.ObservedNet).ToList();

            TheoreticalMonoIsotopicMass = massesList.Average();
            Net = netList.Average();

            StdevNet = netList.StandardDeviation();
        }
    }
}

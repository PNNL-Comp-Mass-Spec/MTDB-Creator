#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Targets = new List<Target>();
        }

        #region Private fields
        private int m_Id;
        private double m_Net; 
        private double m_StdevNet;
        private double m_Mass;
        private double m_StdevMass;
        private string m_Sequence;
        private TargetDataSet m_Dataset;
        private IList<Target> m_Targets;
        #endregion

        #region Public Properties

        public int Id
        {
            get { return m_Id; }
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }

        public double Net
        {
            get { return m_Net; }
            set
            {
                m_Net = value;
                OnPropertyChanged("Net");
            }
        }

        public double StdevNet
        {
            get { return m_StdevNet; }
            set
            {
                m_StdevNet = value;
                OnPropertyChanged("StdevNet");
            }
        }

        public double Mass
        {
            get { return m_Mass; }
            set
            {
                m_Mass = value;
                OnPropertyChanged("Mass");
            }
        }

        public double StdevMass
        {
            get { return m_StdevMass; }
            set
            {
                m_StdevMass = value;
                OnPropertyChanged("StdevMass");
            }
        }

        public string Sequence
        {
            get { return m_Sequence; }
            set
            {
                m_Sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        public TargetDataSet Dataset
        {
            get { return m_Dataset; }
            set
            {
                m_Dataset = value;
                OnPropertyChanged("Dataset");
            }
        }

        public IList<Target> Targets
        {
            get { return m_Targets; }
            set
            {
                m_Targets = value;
                OnPropertyChanged("Targets");
            }
        }

        #endregion

        public void AddTarget(Target target)
        {
            Targets.Add(target);

            target.Parent = this;
        }

        ///<summary>
        /// Calculates the average mass based on the theoretical value for each Target
        /// </summary>
        public double TheoreticalMassAvg(List<Target> Targets)
        {
            double sum = 0;
            int count = 0;
            if (Targets.Count == 0)
            {
                return 0;
            }
            else
            {
                foreach(Target t in Targets)
                {
                    sum += t.MonoisotopicMass;
                }
                sum /= count;
                return sum;
            }
        }

        ///<summary>
        /// Calculates the average mass based on the theoretical value for each Target
        /// </summary>
        public double ObservedMassAvg(List<Target> Targets)
        {
            double sum = 0;
            int count = 0;
            if (Targets.Count == 0)
            {
                return 0;
            }
            else
            {
                foreach (Target t in Targets)
                {
                    sum += t.ObservedMonoisotopicMass;
                }
                sum /= count;
                return sum;
            }
        }

        /// <summary>
        /// Calculate average mass & net and stdev mass & net for each Target.
        /// </summary>
        public void CalculateStatistics()
        {
            List<double> massesList = Targets.Select(c => c.MonoisotopicMass).ToList();
            List<double> netList = Targets.Select(c => c.ObservedNet).ToList();

            this.Mass = massesList.Average();
            this.Net = netList.Average();

            this.StdevMass = massesList.StandardDeviation();
            this.StdevNet = netList.StandardDeviation();
        }
    }
}

#region Namespaces

using System.Collections.Generic;
using System.ComponentModel;
using FeatureAlignment.Algorithms.Regression;

#endregion

namespace MTDBFramework.Data
{
    /// <summary>
    /// LCMS dataset
    /// </summary>
    public class LcmsDataSet : INotifyPropertyChanged
    {
        #region Private Fields

        private string m_name;
        private LcmsIdentificationTool m_tool;
        private LinearRegressionResult m_regressionResult;
        private List<Evidence> m_evidences;
        private bool m_previouslyAnalyzed;

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of the dataset
        /// </summary>
        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Format of input data
        /// </summary>
        public LcmsIdentificationTool Tool
        {
            get => m_tool;
            set
            {
                m_tool = value;
                OnPropertyChanged("Tool");
            }
        }

        /// <summary>
        /// Evidences in the dataset
        /// </summary>
        public List<Evidence> Evidences
        {
            get => m_evidences;
            set
            {
                m_evidences = value;
                OnPropertyChanged("Evidences");
            }
        }

        /// <summary>
        /// Result of Linear Regression
        /// </summary>
        public LinearRegressionResult RegressionResult
        {
            get => m_regressionResult;
            set
            {
                m_regressionResult = value;
                OnPropertyChanged("RegressionResult");
            }
        }

        /// <summary>
        /// Whether data has been previously analyzed
        /// </summary>
        public bool PreviouslyAnalyzed
        {
            get => m_previouslyAnalyzed;
            private set
            {
                m_previouslyAnalyzed = value;
                OnPropertyChanged("PreviouslyAnalyzed");
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public LcmsDataSet()
        {
            PreviouslyAnalyzed = false;
            Name = string.Empty;
            Tool = LcmsIdentificationTool.Raw;
            Evidences = new List<Evidence>();
            RegressionResult = new LinearRegressionResult();

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="analyzed">Whether the dataset has already been analyzed</param>
        public LcmsDataSet(bool analyzed)
        {
            PreviouslyAnalyzed = analyzed;
            Name = string.Empty;
            Tool = LcmsIdentificationTool.Raw;
            Evidences = new List<Evidence>();
            RegressionResult = new LinearRegressionResult();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of dataset</param>
        /// <param name="tool">Format of dataset</param>
        /// <param name="evidences">Evidences in dataset</param>
        public LcmsDataSet(string name, LcmsIdentificationTool tool, IEnumerable<Evidence> evidences)
        {
            PreviouslyAnalyzed = false;
            Name = name;
            Tool = tool;
            Evidences = new List<Evidence>(evidences);
            RegressionResult = new LinearRegressionResult();
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
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
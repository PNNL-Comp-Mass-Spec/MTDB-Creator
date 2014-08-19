#region Namespaces

using System;
using System.Collections.Generic;
using MTDBFramework.UI;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Data
{
	/// <summary>
	/// LCMS dataset
	/// </summary>
    public class LcmsDataSet : ObservableObject
    {
        #region Private Fields

        private string m_name;
        private LcmsIdentificationTool m_tool;
        private LinearRegressionResult m_regressionResult;
        private List<Evidence> m_evidences;
        private bool m_previouslyAnalyzed;
        private double[] m_medNetDiffs;

        #endregion

        #region Public Properties

		/// <summary>
		/// Name of the dataset
		/// </summary>
        public string Name
        {
            get { return m_name; }
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
            get { return m_tool; }
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
            get { return m_evidences; }
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
            get { return m_regressionResult; }
            set
            {
                m_regressionResult = value;
                OnPropertyChanged("RegressionResult");
            }
        }

        /// <summary>
        /// Collection of the median NET differences from warping
        /// </summary>
	    public double[] MedianNetDifference
	    {
	        get { return m_medNetDiffs; }
	        set
	        {
	            m_medNetDiffs = value;
	            OnPropertyChanged("MedianNetDifferences");
	        }
	    }

		/// <summary>
		/// Whether data has been previously analyzed
		/// </summary>
        public bool PreviouslyAnalyzed
        {
            get { return m_previouslyAnalyzed; }
            private set { m_previouslyAnalyzed = value; }
        }

        #endregion 

		/// <summary>
		/// Constructor
		/// </summary>
        public LcmsDataSet()
        {
            PreviouslyAnalyzed = false; 
            Name = String.Empty;
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
            Name = String.Empty;
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
    }
}
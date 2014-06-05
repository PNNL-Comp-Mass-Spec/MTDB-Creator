#region Namespaces

using System;
using System.Collections.Generic;
using MTDBFramework.UI;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Data
{
    public class LcmsDataSet : ObservableObject
    {
        #region Private Fields

        private string m_name;
        private LcmsIdentificationTool m_tool;
        private LinearRegressionResult m_regressionResult;
        private List<Evidence> m_evidences;

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        public LcmsIdentificationTool Tool
        {
            get { return m_tool; }
            set
            {
                m_tool = value;
                OnPropertyChanged("Tool");
            }
        }
        
        public List<Evidence> Evidences
        {
            get { return m_evidences; }
            set
            {
                m_evidences = value;
                OnPropertyChanged("Evidences");
            }
        } 

        public LinearRegressionResult RegressionResult
        {
            get { return m_regressionResult; }
            set
            {
                m_regressionResult = value;
                OnPropertyChanged("RegressionResult");
            }
        }

        #endregion 

        public LcmsDataSet()
        {
            Name = String.Empty;
            Tool = LcmsIdentificationTool.Raw;
            Evidences = new List<Evidence>();
            RegressionResult = new LinearRegressionResult();
        }

        public LcmsDataSet(string name, LcmsIdentificationTool tool, IEnumerable<Evidence> evidences)
        {
            Name = name;
            Tool = tool;
            Evidences = new List<Evidence>(evidences);
            RegressionResult = new LinearRegressionResult();
        }
    }
}
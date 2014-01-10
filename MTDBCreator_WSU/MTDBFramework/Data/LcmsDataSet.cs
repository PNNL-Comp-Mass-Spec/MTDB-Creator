#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MTDBFramework.UI;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.Data
{
    public class LcmsDataSet : ObservableObject
    {
        #region Private Fields

        private string m_Name;
        private LcmsIdentificationTool m_Tool;
        private LinearRegressionResult m_RegressionResult;

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        public LcmsIdentificationTool Tool
        {
            get { return m_Tool; }
            set
            {
                m_Tool = value;
                OnPropertyChanged("Tool");
            }
        }

        public ObservableCollection<Target> Targets { get; set; }

        public LinearRegressionResult RegressionResult
        {
            get { return m_RegressionResult; }
            set
            {
                m_RegressionResult = value;
                OnPropertyChanged("RegressionResult");
            }
        }

        #endregion 

        public LcmsDataSet()
        {
            this.Name = String.Empty;
            this.Tool = LcmsIdentificationTool.Raw;
            this.Targets = new ObservableCollection<Target>();
            this.RegressionResult = new LinearRegressionResult();
        }

        public LcmsDataSet(string name, LcmsIdentificationTool tool, IEnumerable<Target> targets)
        {
            this.Name = name;
            this.Tool = tool;
            this.Targets = new ObservableCollection<Target>(targets);
            this.RegressionResult = new LinearRegressionResult();
        }
    }
}
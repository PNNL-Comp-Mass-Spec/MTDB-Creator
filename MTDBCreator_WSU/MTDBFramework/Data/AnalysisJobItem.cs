#region Namespaces

using System;
using System.IO;
using System.Linq;
using MTDBFramework.UI;

#endregion

namespace MTDBFramework.Data
{
    public class AnalysisJobItem : ObservableObject
    {
        #region Private Fields

        private string m_Title;
        private string m_FileName;
        private string m_BaseFolder;
        private string m_FilePath;
        private LcmsIdentificationTool m_Format;
        private LcmsDataSet m_DataSet;

        #endregion

        #region Public Properties

        public string Title
        {
            get { return m_Title; }
            set
            {
                m_Title = value;
                OnPropertyChanged("Title");
            }
        }

        public string FileName
        {
            get { return m_FileName; }
            set
            {
                m_FileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string BaseFolder
        {
            get { return m_BaseFolder; }
            set
            {
                m_BaseFolder = value;
                OnPropertyChanged("BaseFolder");
            }
        }

        public string FilePath
        {
            get { return m_FilePath; }
            set
            {
                m_FilePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public LcmsIdentificationTool Format
        {
            get { return m_Format; }
            set
            {
                m_Format = value;
                OnPropertyChanged("Format");
            }
        }

        public LcmsDataSet DataSet
        {
            get { return m_DataSet; }
            set
            {
                m_DataSet = value;
                OnPropertyChanged("DataSet");
            }
        }

        #endregion

        public AnalysisJobItem(string path, LcmsIdentificationTool format)
        {
            this.FileName = Path.GetFileName(path);
            this.BaseFolder = Path.GetDirectoryName(path);
            this.FilePath = path;
            this.Format = format;

            this.Title = this.FileName.Replace(this.FileName.Substring(this.FileName.LastIndexOf('_')), String.Empty);
        }

        public int TargetCount
        {
            get
            {
                return this.DataSet == null ? 0 : this.DataSet.Targets.Count;
            }
        }

        //public int ProteinCount
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}
    }
}
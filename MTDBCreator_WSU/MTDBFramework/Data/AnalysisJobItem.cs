#region Namespaces

using System;
using System.IO;
using MTDBFramework.UI;

#endregion

namespace MTDBFramework.Data
{
    public class AnalysisJobItem : ObservableObject
    {
        #region Private Fields

        private string m_title;
        private string m_fileName;
        private string m_baseFolder;
        private string m_filePath;
        private LcmsIdentificationTool m_format;
        private LcmsDataSet m_dataSet;

        #endregion

        #region Public Properties

        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                OnPropertyChanged("Title");
            }
        }

        public string FileName
        {
            get { return m_fileName; }
            set
            {
                m_fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string BaseFolder
        {
            get { return m_baseFolder; }
            set
            {
                m_baseFolder = value;
                OnPropertyChanged("BaseFolder");
            }
        }

        public string FilePath
        {
            get { return m_filePath; }
            set
            {
                m_filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public LcmsIdentificationTool Format
        {
            get { return m_format; }
            set
            {
                m_format = value;
                OnPropertyChanged("Format");
            }
        }

        public LcmsDataSet DataSet
        {
            get { return m_dataSet; }
            set
            {
                m_dataSet = value;
                OnPropertyChanged("DataSet");
            }
        }

        #endregion

        public AnalysisJobItem(string path, LcmsIdentificationTool format)
        {
            FileName = Path.GetFileName(path);
            BaseFolder = Path.GetDirectoryName(path);
            FilePath = path;
            Format = format;

            Title = FileName.Replace(FileName.Substring(FileName.LastIndexOf('_')), String.Empty);
        }

        public int TargetCount
        {
            get
            {
                return DataSet == null ? 0 : DataSet.Evidences.Count;
            }
        }
    }
}
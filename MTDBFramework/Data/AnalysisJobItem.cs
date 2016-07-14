#region Namespaces

using System;
using System.IO;
using MTDBFramework.UI;

#endregion

namespace MTDBFramework.Data
{
    /// <summary>
    /// Data encapsulation for Analysis Jobs
    /// </summary>
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

        /// <summary>
        /// Title Accessor
        /// </summary>
        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                OnPropertyChanged("Title");
            }
        }

        /// <summary>
        /// FileName Accessor
        /// </summary>
        public string FileName
        {
            get { return m_fileName; }
            set
            {
                m_fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        /// <summary>
        /// BaseFolder Accessor
        /// </summary>
        public string BaseFolder
        {
            get { return m_baseFolder; }
            set
            {
                m_baseFolder = value;
                OnPropertyChanged("BaseFolder");
            }
        }

        /// <summary>
        /// FilePath Accessor
        /// </summary>
        public string FilePath
        {
            get { return m_filePath; }
            set
            {
                m_filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        /// <summary>
        /// Format Accessor
        /// </summary>
        public LcmsIdentificationTool Format
        {
            get { return m_format; }
            set
            {
                m_format = value;
                OnPropertyChanged("Format");
            }
        }

        /// <summary>
        /// DataSet Accessor
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        public AnalysisJobItem(string path, LcmsIdentificationTool format)
        {
            FileName = Path.GetFileName(path);
            BaseFolder = Path.GetDirectoryName(path);
            FilePath = path;
            Format = format;

            Title = FileName.Replace(FileName.Substring(FileName.LastIndexOf('_')), String.Empty);
        }

        /// <summary>
        /// Accessor for count of targets
        /// </summary>
        public int TargetCount
        {
            get
            {
                return DataSet == null ? 0 : DataSet.Evidences.Count;
            }
        }
    }
}
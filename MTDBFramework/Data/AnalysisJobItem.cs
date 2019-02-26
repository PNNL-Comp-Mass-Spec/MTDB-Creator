#region Namespaces

using System.ComponentModel;
using System.IO;

#endregion

namespace MTDBFramework.Data
{
    /// <summary>
    /// Data encapsulation for Analysis Jobs
    /// </summary>
    public class AnalysisJobItem : INotifyPropertyChanged
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
            get => m_title;
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
            get => m_fileName;
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
            get => m_baseFolder;
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
            get => m_filePath;
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
            get => m_format;
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
            get => m_dataSet;
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

            if (FileName == null)
                Title = string.Empty;
            else
                Title = FileName.Replace(FileName.Substring(FileName.LastIndexOf('_')), string.Empty);
        }

        /// <summary>
        /// Accessor for count of targets
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public int TargetCount => DataSet?.Evidences.Count ?? 0;

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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Properties;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class AnalysisJobViewModel : ObservableObject
    {
        #region Private Fields

        private string m_Title = "NewJob";

        private ObservableCollection<AnalysisJobItem> m_AnalysisJobItems;
        private TargetDatabase m_Database;
        private Options m_Options;

        private ICommand m_ProcessAnalysisJobCommand;
        private ICommand m_RemoveAnalysisJobCommand;

        #endregion

        #region Public Properties

        public int Id { get; private set; } // For recent analysis job record purpose

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
                OnPropertyChanged("Title");
            }
        }

        public Options Options
        {
            get
            {
                return m_Options;
            }
            set
            {
                m_Options = value;
                OnPropertyChanged("Options");
            }
        }

        public TargetDatabase Database
        {
            get { return m_Database; }
            set
            {
                m_Database = value;
                OnPropertyChanged("Database");
            }
        }

        public ICommand ProcessAnalysisJobCommand
        {
            get
            {
                if (m_ProcessAnalysisJobCommand == null)
                {
                    m_ProcessAnalysisJobCommand = new RelayCommand(param => ProcessAnalysisJob(param));
                }

                return m_ProcessAnalysisJobCommand;
            }
        }

        public ICommand RemoveAnalysisJobCommand
        {
            get
            {
                if (m_RemoveAnalysisJobCommand == null)
                {
                    m_RemoveAnalysisJobCommand = new RelayCommand(items => RemoveAnalysisJob(items));
                }

                return m_RemoveAnalysisJobCommand;
            }
        }

        public ObservableCollection<AnalysisJobItem> AnalysisJobItems
        {
            get
            {
                return m_AnalysisJobItems;
            }
            set
            {
                m_AnalysisJobItems = value;
                OnPropertyChanged("AnalysisJobItems");
            }
        }

        #endregion

        #region Command Methods

        private void ProcessAnalysisJob(object param)
        {
            object result = ProcessAnalysisTargets();

            OnAnalysisJobProcessed(new MTDBResultChangedEventArgs(result));

            if (result != null && param == null)
            {
                result = ProcessAnalysisDatabase();

                OnAnalysisJobProcessed(new MTDBResultChangedEventArgs(result));
            }
        }

        private void RemoveAnalysisJob(object param)
        {
            IEnumerable<AnalysisJobItem> items = ((IEnumerable)param).Cast<AnalysisJobItem>().ToList();

            if (items.Any())
            {
                foreach (AnalysisJobItem analysisJobItem in items)
                {
                    AnalysisJobItems.Remove(analysisJobItem);
                }
            }
        }

        #endregion

        #region Public Methods

        public object ProcessAnalysisTargets()
        {
            return BackgroundWorkProcessHelper.Process(new AnalysisJobBackgroundWorkHelper(this));
        }

        public object ProcessAnalysisDatabase()
        {
            return BackgroundWorkProcessHelper.Process(new MTDBProcessorBackgroundWorkHelper(this));
        }

        public object SaveAnalysisDatabase(string fileName)
        {
            return BackgroundWorkProcessHelper.Process(new MTDBWriterBackgroundWorkHelper(this, fileName));
        }

        #endregion

        #region Events

        public event MTDBResultChangedEventHandler AnalysisJobProcessed;

        protected void OnAnalysisJobProcessed(MTDBResultChangedEventArgs e)
        {
            if (AnalysisJobProcessed != null)
            {
                AnalysisJobProcessed(this, e);
            }
        }

        #endregion

        public AnalysisJobViewModel()
        {
            this.Id = RecentAnalysisJobHelper.RecentAnalysisJobCount;

            this.AnalysisJobItems = new ObservableCollection<AnalysisJobItem>();
            this.Options = new Options();
        }
    }
}

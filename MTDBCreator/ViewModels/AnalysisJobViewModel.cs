﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public sealed class AnalysisJobViewModel : ObservableObject
    {
        #region Private Fields

        private string m_title = "NewJob";

        private ObservableCollection<AnalysisJobItem> m_analysisJobItems;
        private TargetDatabase m_database;
        private Options m_options;

        private ICommand m_processAnalysisJobCommand;
        private ICommand m_removeAnalysisJobCommand;

        #endregion

        #region Public Properties

        public int Id { get; private set; } // For recent analysis job record purpose

        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                m_title = value;
                OnPropertyChanged("Title");
            }
        }

        public Options Options
        {
            get
            {
                return m_options;
            }
            private set
            {
                m_options = value;
                OnPropertyChanged("Options");
            }
        }

        public TargetDatabase Database
        {
            get { return m_database; }
            set
            {
                m_database = value;
                OnPropertyChanged("Database");
            }
        }

        public ICommand ProcessAnalysisJobCommand
        {
            get
            {
                if (m_processAnalysisJobCommand == null)
                {
                    m_processAnalysisJobCommand = new RelayCommand(param => ProcessAnalysisJob(param));
                }

                return m_processAnalysisJobCommand;
            }
        }

        public ICommand RemoveAnalysisJobCommand
        {
            get
            {
                if (m_removeAnalysisJobCommand == null)
                {
                    m_removeAnalysisJobCommand = new RelayCommand(items => RemoveAnalysisJob(items));
                }

                return m_removeAnalysisJobCommand;
            }
        }

        public ObservableCollection<AnalysisJobItem> AnalysisJobItems
        {
            get
            {
                return m_analysisJobItems;
            }
            set
            {
                m_analysisJobItems = value;
                OnPropertyChanged("AnalysisJobItems");
            }
        }

        #endregion

        #region Command Methods

        private void ProcessAnalysisJob(object param)
        {
            var result = ProcessAnalysisTargets();

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
                foreach (var analysisJobItem in items)
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
            return BackgroundWorkProcessHelper.Process(new MtdbProcessorBackgroundWorkHelper(this));
        }

        public object SaveAnalysisDatabase(string fileName)
        {
            return BackgroundWorkProcessHelper.Process(new MTDBWriterBackgroundWorkHelper(this, fileName));
        }

        #endregion

        #region Events

        public event MTDBResultChangedEventHandler AnalysisJobProcessed;

        private void OnAnalysisJobProcessed(MTDBResultChangedEventArgs e)
        {
            if (AnalysisJobProcessed != null)
            {
                AnalysisJobProcessed(this, e);
            }
        }

        #endregion

        public AnalysisJobViewModel()
        {
            Id = RecentAnalysisJobHelper.RecentAnalysisJobCount;

            AnalysisJobItems = new ObservableCollection<AnalysisJobItem>();
            Options = new Options();
        }
    }
}
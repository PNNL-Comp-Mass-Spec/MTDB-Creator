using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.UI;
using Microsoft.Win32;
using System;

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
            DateTime start = DateTime.Now;
            var result = ProcessAnalysisTargets();

            OnAnalysisJobProcessed(new MtdbResultChangedEventArgs(result));
            DateTime end = DateTime.Now;
            Console.WriteLine("Analysis processed after " + (end-start));

            if (result != null && param == null)
            {
                DateTime procStart = DateTime.Now;
                result = ProcessAnalysisDatabase();

                if (result == null && BackgroundWorkProcessHelper.MostRecentResult != null)
                {
                    // This condition will be true if MultithreadingEnabled = false
                    result = BackgroundWorkProcessHelper.MostRecentResult;

                    if (Database == null && result is TargetDatabase)
                        Database = (TargetDatabase)result;
                }
                
                OnAnalysisJobProcessed(new MtdbResultChangedEventArgs(result));
                end = DateTime.Now;
                Console.WriteLine("Alignment processed after " + (end - start) + " total");
                Console.WriteLine("Alignment took " + (end - procStart));

                if (result != null)
                {
                    if (Options.DatabaseType != DatabaseType.NotSaved)
                    {
                        var saveDatabaseDialog = new SaveFileDialog();
                        if (RestoreDirectory == null)
                        {
                            RestoreDirectory = "C:\\";
                        }
                        saveDatabaseDialog.InitialDirectory = RestoreDirectory;
                        saveDatabaseDialog.RestoreDirectory = true;

                        if (Options.DatabaseType == DatabaseType.SQLite)
                        {
                            saveDatabaseDialog.Filter = "Mass Tag Database (*.mtdb)|*.mtdb|All Files (*.*)|*.*";
                            saveDatabaseDialog.Title = "Save to MTDB";
                        }
                        else
                        {
                            saveDatabaseDialog.Filter = "Access Database (*.mdb)|*.mdb|All Files (*.*)|*.*";
                            saveDatabaseDialog.Title = "Save to Access Database";
                        }

                        if (saveDatabaseDialog.ShowDialog() == true)
                        {
                            DateTime saveStart = DateTime.Now;
                            SaveAnalysisDatabase(saveDatabaseDialog.FileName);
                            end = DateTime.Now;
                            Console.WriteLine("Database Save took " + (end - saveStart));
                        }
                        if (saveDatabaseDialog.FileName != "")
                        {
                            RestoreDirectory = Path.GetDirectoryName(saveDatabaseDialog.FileName);
                            SavedDatabasePath = saveDatabaseDialog.FileName;
                            IsDatabaseSaved = true;
                        }
                    }

                }
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
            return BackgroundWorkProcessHelper.Process(new MtdbWriterBackgroundWorkHelper(this, fileName));
        }

        #endregion

        #region Events

        public event MtdbResultChangedEventHandler AnalysisJobProcessed;

        private void OnAnalysisJobProcessed(MtdbResultChangedEventArgs e)
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

        private string RestoreDirectory { get; set; }

        public string SavedDatabasePath { get; set; }

        public bool IsDatabaseSaved { get; set; }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MTDBAccessIO;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBFramework.Data;
using Microsoft.Win32;
using System;
using MTDBFrameworkBase.Data;
using MTDBFrameworkBase.Database;
using MTDBFrameworkBase.IO;
using PRISM;

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

        public int Id { get; } // For recent analysis job record purpose

        public string Title
        {
            get => m_title;
            set
            {
                m_title = value;
                OnPropertyChanged("Title");
            }
        }

        public Options Options
        {
            get => m_options;
            private set
            {
                m_options = value;
                OnPropertyChanged("Options");
            }
        }

        public TargetDatabase Database
        {
            get => m_database;
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
                    m_processAnalysisJobCommand = new RelayCommand(ProcessAnalysisJob);
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
                    m_removeAnalysisJobCommand = new RelayCommand(RemoveAnalysisJob);
                }

                return m_removeAnalysisJobCommand;
            }
        }

        public ObservableCollection<AnalysisJobItem> AnalysisJobItems
        {
            get => m_analysisJobItems;
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
            try
            {
                // Insert database save location here
                var saveDatabaseDialog = new SaveFileDialog();

                if (Options.DatabaseType != DatabaseType.NotSaved)
                {
                    if (string.IsNullOrWhiteSpace(RestoreDirectory))
                    {
                        if (AnalysisJobItems != null && AnalysisJobItems.Count > 0)
                        {
                            RestoreDirectory = AnalysisJobItems.First().BaseFolder;
                        }
                        else
                        {
                            RestoreDirectory = @"C:\";
                        }
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
                    saveDatabaseDialog.ShowDialog();
                    if (saveDatabaseDialog.FileName != "")
                    {
                        RestoreDirectory = Path.GetDirectoryName(saveDatabaseDialog.FileName);
                        SavedDatabasePath = saveDatabaseDialog.FileName;
                    }
                }

                var start = DateTime.Now;
                var result = ProcessAnalysisTargets();

                OnAnalysisJobProcessed(new MtdbResultChangedEventArgs(result));
                var end = DateTime.Now;
                Console.WriteLine("Analysis processed after " + (end - start));

                if (File.Exists(SavedDatabasePath))
                {
                    ITargetDatabaseReader reader;
                    IEnumerable<LcmsDataSet> databaseDatasets = null;
                    var loaded = false;
                    var errorResult = MessageBoxResult.OK;
                    if (SavedDatabasePath.EndsWith("mtdb"))
                        reader = new SqLiteTargetDatabaseReader();
                    else
                    {
                        reader = new AccessTargetDatabaseReader();
                    }
                    try
                    {
                        databaseDatasets = reader.Read(SavedDatabasePath);
                        loaded = true;
                    }
                    catch (Exception)
                    {
                        errorResult = MessageBox.Show(string.Format("{0} does not contain valid data to be imported into MTDBCreator", SavedDatabasePath),
                            "Error loading file", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                    }

                    if (loaded)
                    {
                        foreach (var dataset in databaseDatasets)
                        {
                            var exists = false;
                            var priorAnalysis = new AnalysisJobItem(dataset.Name, dataset.Tool) {
                                DataSet = dataset
                            };

                            foreach (var item in AnalysisJobItems)
                                if (item.Title == priorAnalysis.Title)
                                {
                                    item.DataSet = dataset;
                                    exists = true;
                                }
                            if (!exists)
                                AnalysisJobItems.Add(priorAnalysis);
                        }
                    }
                    else if (errorResult == MessageBoxResult.Cancel)
                    {
                        // User cancelled processing
                        return;
                    }
                }

                if (result == null || param != null) return;

                var procStart = DateTime.Now;
                result = ProcessAnalysisDatabase();

                if (result == null && BackgroundWorkProcessHelper.MostRecentResult != null)
                {
                    // This condition will be true if MultiThreadingEnabled = false
                    result = BackgroundWorkProcessHelper.MostRecentResult;

                    if (Database == null && result is TargetDatabase database)
                        Database = database;
                }

                OnAnalysisJobProcessed(new MtdbResultChangedEventArgs(result));
                end = DateTime.Now;
                Console.WriteLine("Alignment processed after " + (end - start) + " total");
                Console.WriteLine("Alignment took " + (end - procStart));

                if (result != null && saveDatabaseDialog.FileName != "")
                {
                    var saveStart = DateTime.Now;

                    SaveAnalysisDatabase(saveDatabaseDialog.FileName);
                    IsDatabaseSaved = true;
                    end = DateTime.Now;
                    Console.WriteLine("Database Save took " + (end - saveStart));
                }
            }
            catch (Exception ex)
            {
                var errMsg = "Exception in ProcessAnalysisJob";
                if (m_options.ConsoleMode)
                {
                    ConsoleMsgUtils.ShowError(errMsg, ex);
                }
                else
                {
                    MessageBox.Show(errMsg + ": " + ex.Message +
                                    StackTraceFormatter.GetExceptionStackTraceMultiLine(ex),
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        public bool ShowOpenOldAnalysis => System.Net.Dns.GetHostEntry("").HostName.Contains("pnl.gov");

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
            AnalysisJobProcessed?.Invoke(this, e);
        }

        #endregion

        public AnalysisJobViewModel()
        {
            Id = RecentAnalysisJobHelper.RecentAnalysisJobCount;
            AnalysisJobItems = new ObservableCollection<AnalysisJobItem>();
            Options = new Options();
        }

        /// <summary>
        /// Directory with the output database file
        /// </summary>
        private string RestoreDirectory { get; set; }

        /// <summary>
        /// Path to the output database file
        /// </summary>
        public string SavedDatabasePath { get; set; }

        public bool IsDatabaseSaved { get; set; }
    }
}

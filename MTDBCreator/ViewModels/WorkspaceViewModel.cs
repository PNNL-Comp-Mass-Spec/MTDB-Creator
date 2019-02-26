using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBFramework.IO;

namespace MTDBCreator.ViewModels
{
    public class WorkspaceViewModel : ObservableObject
    {
        #region Private Fields
        private ICommand m_createDatabaseCommand;
        private ICommand m_loadDatabaseCommand;
        private ICommand m_refreshCommand;

        private int m_selectedTabIndex;

        private AnalysisJobViewModel m_analysisJobViewModel;
        private DatasetPlotViewModel m_datasetPlotViewModel;
        private StatPlotViewModel m_statPlotViewModel;
        private TargetTreeViewModel m_targetTreeViewModel;

        private const string RefreshBoxText = "Are you sure you want to refresh the datasets?";
        private const string RefreshBoxCaption = "Refresh Datasets";
        private const MessageBoxButton RefreshBoxButton = MessageBoxButton.YesNo;
        private const MessageBoxImage RefreshBoxImage = MessageBoxImage.Question;

        #endregion

        #region Public Properties

        public WorkspaceViewModel()
        {
        }

        public ICommand CreateDatabaseCommand
        {
            get { return m_createDatabaseCommand ?? (m_createDatabaseCommand = new RelayCommand(param => CreateDatabase())); }
        }

        public ICommand LoadDatabaseCommand
        {
            get { return m_loadDatabaseCommand ?? (m_loadDatabaseCommand = new RelayCommand(param => ReadDatabase())); }
        }

        public ICommand RefreshCommand
        {
            get { return m_refreshCommand ?? (m_refreshCommand = new RelayCommand(param => Refresh())); }
        }

        private string RestoreDirectory { get; set; }

        public AnalysisJobViewModel AnalysisJobViewModel
        {
            get => m_analysisJobViewModel;
            set
            {
                m_analysisJobViewModel = value;
                OnPropertyChanged("AnalysisJobViewModel");
            }
        }

        public DatasetPlotViewModel DatasetPlotViewModel
        {
            get => m_datasetPlotViewModel;
            private set
            {
                m_datasetPlotViewModel = value;
                OnPropertyChanged("DatasetPlotViewModel");
            }
        }

        public StatPlotViewModel StatPlotViewModel
        {
            get => m_statPlotViewModel;
            private set
            {
                m_statPlotViewModel = value;
                OnPropertyChanged("StatPlotViewModel");
            }
        }

        public TargetTreeViewModel TargetTreeViewModel
        {
            get => m_targetTreeViewModel;
            private set
            {
                m_targetTreeViewModel = value;
                OnPropertyChanged("TargetTreeViewModel");
            }
        }

        public int SelectedTabIndex
        {
            get => m_selectedTabIndex;
            set
            {
                m_selectedTabIndex = value;
                TargetTreeViewModel.SelectedTabIndex = m_selectedTabIndex;
                OnPropertyChanged("SelectedTab");
            }
        }

        public bool IsDatabaseSaved { get; set; }

        public string SavedDatabasePath { get; set; }

        #endregion

        #region Command Methods

        private void Refresh()
        {
            if (AnalysisJobViewModel.Options.OptionsChanged)
            {
                var refreshResult = MessageBox.Show(RefreshBoxText,
                                                    RefreshBoxCaption,
                                                    RefreshBoxButton,
                                                    RefreshBoxImage);

                if (refreshResult == MessageBoxResult.Yes)
                {
                    AnalysisJobViewModel.ProcessAnalysisTargets();
                    AnalysisJobViewModel.ProcessAnalysisDatabase();

                    UpdateDataViewModels();
                    AnalysisJobViewModel.Options.OptionsChanged = false;

                }
            }
            else
            {
                MessageBox.Show("Datasets already current. \nNo need to refresh processing.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CreateDatabase()
        {
            if (!IsDatabaseSaved)
            {
                var saveDatabaseDialog = new SaveFileDialog
                {
                    Filter = "Mass Tag Database (*.mtdb)|*.mtdb|All Files (*.*)|*.*",
                    Title = "Save to MTDB"
                };

                if (RestoreDirectory == null)
                {
                    RestoreDirectory = @"C:\";
                }
                saveDatabaseDialog.InitialDirectory = RestoreDirectory;
                saveDatabaseDialog.RestoreDirectory = true;

                if (saveDatabaseDialog.ShowDialog() == true)
                {
                    AnalysisJobViewModel.SaveAnalysisDatabase(saveDatabaseDialog.FileName);
                }
                if (saveDatabaseDialog.FileName != "")
                {
                    RestoreDirectory = Path.GetDirectoryName(saveDatabaseDialog.FileName);
                    SavedDatabasePath = saveDatabaseDialog.FileName;
                    IsDatabaseSaved = true;
                }
            }
            else
            {
                var message = string.Format("Database already saved to {0}", SavedDatabasePath);
                MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReadDatabase()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Mass Tag Database (*.mtdb)|*.mtdb|AllFile (*.*)|*.*",
                Title = "Load MTDB from file",
                InitialDirectory = RestoreDirectory,
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() == true)
            {
                var reader = new SqLiteTargetDatabaseReader();
                //AnalysisJobViewModel.Database = reader.Read(dlg.FileName);
            }
        }

        #endregion

        #region Public Methods

        public void UpdateDataViewModels()
        {
            if (DatasetPlotViewModel == null)
            {
                DatasetPlotViewModel = new DatasetPlotViewModel(AnalysisJobViewModel);
            }
            else
            {
                DatasetPlotViewModel.UpdatePlotViewModel(AnalysisJobViewModel);
            }
            TargetTreeViewModel = new TargetTreeViewModel(AnalysisJobViewModel);
            StatPlotViewModel = new StatPlotViewModel(AnalysisJobViewModel);
            IsDatabaseSaved = false;
        }

        #endregion

        public WorkspaceViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            AnalysisJobViewModel = analysisJobViewModel;

            UpdateDataViewModels();
        }
    }
}

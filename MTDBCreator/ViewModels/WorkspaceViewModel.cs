using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MTDBCreator.Commands;
using MTDBCreator.Views;
using MTDBFramework.Data;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class WorkspaceViewModel : ObservableObject
    {
        #region Private Fields
        private ICommand m_createDatabaseCommand;
        private ICommand m_refreshCommand;

        private AnalysisJobViewModel m_analysisJobViewModel;
        private DatasetPlotViewModel m_datasetPlotViewModel;
        private StatPlotViewModel m_statPlotViewModel;
        private TargetTreeViewModel m_targetTreeViewModel;
        
        private const string RefreshBoxText             = "Are you sure you want to refresh the datasets?";
        private const string RefreshBoxCaption          = "Refresh Datasets";
        private const MessageBoxButton RefreshBoxButton = MessageBoxButton.YesNo;
        private const MessageBoxImage RefreshBoxImage   = MessageBoxImage.Question;
        
        #endregion

        #region Public Properties

        public WorkspaceViewModel()
        {
        }

        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (m_createDatabaseCommand == null)
                {
                    m_createDatabaseCommand = new RelayCommand(param => CreateDatabase());
                }

                return m_createDatabaseCommand;
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (m_refreshCommand == null)
                {
                    m_refreshCommand = new RelayCommand(param => Refresh());
                }

                return m_refreshCommand;
            }
        }

        private string RestoreDirectory { get; set; }

        public AnalysisJobViewModel AnalysisJobViewModel
        {
            get
            {
                return m_analysisJobViewModel;
            }
            set
            {
                m_analysisJobViewModel = value;
                OnPropertyChanged("AnalysisJobViewModel");
            }
        }

        public DatasetPlotViewModel DatasetPlotViewModel
        {
            get
            {
                return m_datasetPlotViewModel;
            }
            private set
            {
                m_datasetPlotViewModel = value;
                OnPropertyChanged("DatasetPlotViewModel");
            }
        }

        public StatPlotViewModel StatPlotViewModel
        {
            get
            {
                return m_statPlotViewModel;
            }
            private set
            {
                m_statPlotViewModel = value;
                OnPropertyChanged("StatPlotViewModel");
            }
        }

        public TargetTreeViewModel TargetTreeViewModel
        {
            get
            {
                return m_targetTreeViewModel;
            }
            private set
            {
                m_targetTreeViewModel = value;
                OnPropertyChanged("TargetTreeViewModel");
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
                MessageBoxResult refreshResult = MessageBox.Show(RefreshBoxText,
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
                var saveDatabaseDialog = new SaveFileDialog();
                saveDatabaseDialog.Filter = "Mass Tag Database (*.mtdb)|*.mtdb|All Files (*.*)|*.*";
                saveDatabaseDialog.Title = "Save to MTDB";
                if (RestoreDirectory == null)
                {
                    RestoreDirectory = "C:\\";
                }
                saveDatabaseDialog.InitialDirectory = RestoreDirectory;
                saveDatabaseDialog.RestoreDirectory = true;

                if (saveDatabaseDialog.ShowDialog() == true)
                {
                    AnalysisJobViewModel.SaveAnalysisDatabase(saveDatabaseDialog.FileName);
                }
                RestoreDirectory = Path.GetDirectoryName(saveDatabaseDialog.FileName);
                SavedDatabasePath = saveDatabaseDialog.FileName;
                IsDatabaseSaved = true;
            }
            else
            {
                var message = string.Format("Database already saved to {0}", SavedDatabasePath);
                MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ReadDatabase()
        {
            var dlg = new OpenFileDialog();

            dlg.Filter = "Mass Tag Database (*.mtdb)|*.mtdb|AllFile (*.*)|*.*";
            dlg.Title = "Load MTDB";

            dlg.RestoreDirectory = true;

            if(dlg.ShowDialog() == true)
            {
                AnalysisJobViewModel.ProcessAnalysisDatabase();//dlg.FileName);
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

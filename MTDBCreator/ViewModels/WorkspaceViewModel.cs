using System.Windows.Input;
using Microsoft.Win32;
using MTDBCreator.Commands;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class WorkspaceViewModel : ObservableObject
    {
        #region Private Fields

        private SaveFileDialog m_saveDatabaseDialog; 
        private ICommand m_createDatabaseCommand;
        private ICommand m_refreshCommand;

        private AnalysisJobViewModel m_analysisJobViewModel;
        private DatasetPlotViewModel m_datasetPlotViewModel;
        private StatPlotViewModel m_statPlotViewModel;
        private TargetTreeViewModel m_targetTreeViewModel;

        #endregion

        #region Public Properties

        public WorkspaceViewModel()
        {
            m_saveDatabaseDialog        = new SaveFileDialog();
            m_saveDatabaseDialog.Filter = "Mass Tag Database (*.mtdb)|*.mtdb|All Files (*.*)|*.*";
            m_saveDatabaseDialog.Title  = "Save to MTDB";
            m_saveDatabaseDialog.RestoreDirectory = true;
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

        #endregion

        #region Command Methods

        private void Refresh()
        {
            AnalysisJobViewModel.ProcessAnalysisTargets();
            AnalysisJobViewModel.ProcessAnalysisDatabase();

            UpdateDataViewModels();
        }

        private void CreateDatabase()
        {


            if (m_saveDatabaseDialog.ShowDialog() == true)
            {
                AnalysisJobViewModel.SaveAnalysisDatabase(m_saveDatabaseDialog.FileName);
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
        }

        #endregion

        public WorkspaceViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            AnalysisJobViewModel = analysisJobViewModel;

            UpdateDataViewModels();
        }
    }
}

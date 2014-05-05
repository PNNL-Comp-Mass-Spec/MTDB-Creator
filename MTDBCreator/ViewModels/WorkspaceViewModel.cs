using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using MTDBCreator.Commands;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Windows;
using MTDBFramework.Database;
using MTDBFramework.UI;

namespace MTDBCreator.ViewModels
{
    public class WorkspaceViewModel : ObservableObject
    {
        #region Private Fields

        private ICommand m_CreateDatabaseCommand;
        private ICommand m_RefreshCommand;

        private AnalysisJobViewModel m_AnalysisJobViewModel;
        private DatasetPlotViewModel m_DatasetPlotViewModel;
        private StatPlotViewModel m_StatPlotViewModel;
        private TargetTreeViewModel m_TargetTreeViewModel;

        #endregion

        #region Public Properties

        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (m_CreateDatabaseCommand == null)
                {
                    m_CreateDatabaseCommand = new RelayCommand(param => CreateDatabase());
                }

                return m_CreateDatabaseCommand;
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (m_RefreshCommand == null)
                {
                    m_RefreshCommand = new RelayCommand(param => Refresh());
                }

                return m_RefreshCommand;
            }
        }

        public AnalysisJobViewModel AnalysisJobViewModel
        {
            get
            {
                return m_AnalysisJobViewModel;
            }
            set
            {
                m_AnalysisJobViewModel = value;
                OnPropertyChanged("AnalysisJobViewModel");
            }
        }

        public DatasetPlotViewModel DatasetPlotViewModel
        {
            get
            {
                return m_DatasetPlotViewModel;
            }
            private set
            {
                m_DatasetPlotViewModel = value;
                OnPropertyChanged("DatasetPlotViewModel");
            }
        }

        public StatPlotViewModel StatPlotViewModel
        {
            get
            {
                return m_StatPlotViewModel;
            }
            private set
            {
                m_StatPlotViewModel = value;
                OnPropertyChanged("StatPlotViewModel");
            }
        }

        public TargetTreeViewModel TargetTreeViewModel
        {
            get
            {
                return m_TargetTreeViewModel;
            }
            private set
            {
                m_TargetTreeViewModel = value;
                OnPropertyChanged("TargetTreeViewModel");
            }
        }

        #endregion

        #region Command Methods

        private void Refresh()
        {
            this.AnalysisJobViewModel.ProcessAnalysisTargets();
            this.AnalysisJobViewModel.ProcessAnalysisDatabase();

            this.UpdateDataViewModels();
        }

        private void CreateDatabase()
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "Mass Tag Database (*.db)|*.db|All Files (*.*)|*.*";
            dlg.Title = "Save to MTDB";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                this.AnalysisJobViewModel.SaveAnalysisDatabase(dlg.FileName);
            }
        }

        private void ReadDatabase()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Mass Tag Database (*.db)|*.db|AllFile (*.*)|*.*";
            dlg.Title = "Load MTDB";

            dlg.RestoreDirectory = true;

            if(dlg.ShowDialog() == true)
            {
                this.AnalysisJobViewModel.ProcessAnalysisDatabase();//dlg.FileName);
            }
        }

        #endregion

        #region Public Methods

        public void UpdateDataViewModels()
        {
            if (this.DatasetPlotViewModel == null)
            {
                this.DatasetPlotViewModel = new DatasetPlotViewModel(this.AnalysisJobViewModel);
            }
            else
            {
                this.DatasetPlotViewModel.UpdatePlotViewModel(this.AnalysisJobViewModel);
            }

            this.TargetTreeViewModel = new TargetTreeViewModel(this.AnalysisJobViewModel);
            this.StatPlotViewModel = new StatPlotViewModel(this.AnalysisJobViewModel);
        }

        #endregion

        public WorkspaceViewModel(AnalysisJobViewModel analysisJobViewModel)
        {
            this.AnalysisJobViewModel = analysisJobViewModel;

            UpdateDataViewModels();
        }
    }
}

#region Namespaces

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.ViewModels;
using MTDBCreator.Views;
using MTDBCreator.Windows;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;
using OxyPlot.Wpf;

#endregion

namespace MTDBCreator.PageControls
{
    /// <summary>
    /// Interaction logic for WorkspacePage.xaml
    /// </summary>
    public partial class WorkspacePageUserControl : UserControl
    {
        public WorkspaceViewModel WorkspaceViewModel;

        public WorkspacePageUserControl()
        {
            InitializeComponent();
        }

        public WorkspacePageUserControl(AnalysisJobViewModel analysisJobViewModel)
            : this()
        {
            this.WorkspaceViewModel = new WorkspaceViewModel(analysisJobViewModel);
            this.DataContext = this.WorkspaceViewModel;
        }

        private void AddAnalysisJobItemsButton_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataJobWindow = new AddDataWindow(this.WorkspaceViewModel)
            {
                Owner = Application.Current.MainWindow
            };

            addDataJobWindow.ShowDialog();

            this.AnalysisJobDataGrid.SelectedIndex = -1;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow(WorkspaceViewModel.AnalysisJobViewModel.Options);
            optionsWindow.ShowDialog();
        }

        private void DataGridExpander_Expanded(object sender, RoutedEventArgs e)
        {
            WorkspaceGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);

            DataGridExpander.Header = "Hide Data Grid";
        }

        private void DataGridExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            WorkspaceGrid.RowDefinitions[2].Height = new GridLength(25);

            DataGridExpander.Header = "Show Data Grid";
        }
    }
}
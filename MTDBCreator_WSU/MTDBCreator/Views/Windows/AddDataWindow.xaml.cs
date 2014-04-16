#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism;
using Microsoft.Win32;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Helpers.Dialog;
using MTDBCreator.ViewModels;
using MTDBCreator.Views;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;

#endregion

namespace MTDBCreator.Windows
{
    /// <summary>
    /// Interaction logic for AddDataWindow.xaml
    /// </summary>
    public partial class AddDataWindow : Window
    {
        public AddDataWindow()
        {
            InitializeComponent();
        }

        public AddDataWindow(WorkspaceViewModel workspaceViewModel)
            : this()
        {
            this.DataContext = new AnalysisJobViewModel();
            this.WorkspaceViewModel = workspaceViewModel;

            this.AnalysisJobViewModel.AnalysisJobProcessed += AnalysisJobViewModel_AnalysisJobProcessed;

            if (workspaceViewModel != null)
            {
                this.AnalysisJobTitleTextBox.IsEnabled = false;
                this.OptionsButton.Visibility = Visibility.Hidden;

                this.AnalysisJobViewModel.Title = workspaceViewModel.AnalysisJobViewModel.Title;
            }
        }

        public AnalysisJobViewModel AnalysisJobViewModel
        {
            get
            {
                return this.DataContext as AnalysisJobViewModel;
            }
        }

        public WorkspaceViewModel WorkspaceViewModel { get; private set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddButton.ContextMenu.IsOpen = true;
        }

        private void AddFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem addFileMenuItem = e.Source as MenuItem;

            if (addFileMenuItem != null)
            {
                FileDialogFormatInfo formatInfo = FileDialogFormatInfoFactory.Create(addFileMenuItem.Tag.ToString());

                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    RestoreDirectory = true,

                    Multiselect = true,
                    Title = formatInfo.Title,
                    Filter = formatInfo.Filter
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    if (formatInfo.Format != LcmsIdentificationTool.Description)
                    {
                        foreach (string fileName in openFileDialog.FileNames)
                        {
                            this.AnalysisJobViewModel.AnalysisJobItems.Add(new AnalysisJobItem(fileName, formatInfo.Format));

                            if (formatInfo.Format == LcmsIdentificationTool.MSAlign)
                            {
                                this.AnalysisJobViewModel.Options.TargetFilterType = TargetWorkflowType.TOP_DOWN;
                            }
                        }
                    }
                    else
                    {
                        AnalysisJobDescriptionReader analysisJobDescriptionReader = new AnalysisJobDescriptionReader();

                        foreach (string fileName in openFileDialog.FileNames)
                        {
                            try
                            {
                                foreach (AnalysisJobItem analysisJobItem in analysisJobDescriptionReader.Read(fileName))
                                {
                                    this.AnalysisJobViewModel.AnalysisJobItems.Add(analysisJobItem);
                                }
                            }
                            catch
                            {
                                MessageBox.Show(
                                    this, String.Format(
                                        "MTDB Creator cannot read this file.{0}This is not a valid Dataset Description file, or its format is not correct.{0}{0}{1}",
                                        Environment.NewLine, fileName), Application.Current.MainWindow.Tag.ToString(),
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }

                    MainListView.Focus();
                }
            }
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow options = new OptionsWindow(this.AnalysisJobViewModel.Options);
            options.ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AnalysisJobViewModel_AnalysisJobProcessed(object sender, MTDBResultChangedEventArgs e)
        {
            if (e.Result == null)
            {
                this.ShowDialog();
            }
            else
            {
                if (e.Result is ObservableCollection<AnalysisJobItem>)
                {
                    if (this.WorkspaceViewModel != null)
                    {
                        foreach (AnalysisJobItem analysisJobItem in this.AnalysisJobViewModel.AnalysisJobItems)
                        {
                            this.WorkspaceViewModel.AnalysisJobViewModel.AnalysisJobItems.Add(analysisJobItem);
                        }

                        this.WorkspaceViewModel.AnalysisJobViewModel.ProcessAnalysisDatabase();
                        this.WorkspaceViewModel.UpdateDataViewModels();

                        RecentAnalysisJobHelper.AddRecentAnalysisJob(this.WorkspaceViewModel.AnalysisJobViewModel);

                        this.Close();
                    }
                }
                else if (e.Result is TargetDatabase)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.NewWorkspacePage(this.AnalysisJobViewModel);

                        RecentAnalysisJobHelper.AddRecentAnalysisJob(this.AnalysisJobViewModel);
                    }

                    this.Close();
                }
            }
        }

        private void AddDataWindow_Closed(object sender, EventArgs e)
        {
            this.AnalysisJobViewModel.AnalysisJobProcessed -= this.AnalysisJobViewModel_AnalysisJobProcessed;
        }
    }
}

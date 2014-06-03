#region Namespaces

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
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
            DataContext = new AnalysisJobViewModel();
            WorkspaceViewModel = workspaceViewModel;

            AnalysisJobViewModel.AnalysisJobProcessed += AnalysisJobViewModel_AnalysisJobProcessed;

            if (workspaceViewModel != null)
            {
                AnalysisJobTitleTextBox.IsEnabled = false;
                OptionsButton.Visibility = Visibility.Hidden;

                AnalysisJobViewModel.Title = workspaceViewModel.AnalysisJobViewModel.Title;
            }
        }

        public AnalysisJobViewModel AnalysisJobViewModel
        {
            get
            {
                return DataContext as AnalysisJobViewModel;
            }
        }

        public WorkspaceViewModel WorkspaceViewModel { get; private set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddButton.ContextMenu.IsOpen = true;
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            AddFolderButton.ContextMenu.IsOpen = true;
        }

        private void AddFolderFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var addFolderFileMenuItem = e.Source as MenuItem;

            if (addFolderFileMenuItem != null)
            {
                var formatInfo = FileDialogFormatInfoFactory.Create(addFolderFileMenuItem.Tag.ToString());

                string rootPath = Interaction.InputBox("Enter the root folder for the datasets.", "Add dataset folder");
                var filter = formatInfo.Filter.Split('|').Last();
                foreach (
                    var file in Directory.EnumerateFiles(rootPath, filter, SearchOption.AllDirectories))
                {
                    {
                        //Check to separate Sequest Files from MSGF+ files due to similar extensions
                        if ((formatInfo.Format == LcmsIdentificationTool.Sequest) && file.EndsWith("msgfdb_syn.txt"))
                        {
                            continue;
                        }
                        AnalysisJobViewModel.AnalysisJobItems.Add(new AnalysisJobItem(file, formatInfo.Format));

                        if (formatInfo.Format == LcmsIdentificationTool.MSAlign)
                        {
                            AnalysisJobViewModel.Options.TargetFilterType = TargetWorkflowType.TOP_DOWN;
                        }
                    }
                }
            }
        }

        private void AddFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var addFileMenuItem = e.Source as MenuItem;

            if (addFileMenuItem != null)
            {
                var formatInfo = FileDialogFormatInfoFactory.Create(addFileMenuItem.Tag.ToString());

                var openFileDialog = new OpenFileDialog
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
                        foreach (var fileName in openFileDialog.FileNames)
                        {
                            AnalysisJobViewModel.AnalysisJobItems.Add(new AnalysisJobItem(fileName, formatInfo.Format));

                            if (formatInfo.Format == LcmsIdentificationTool.MSAlign)
                            {
                                AnalysisJobViewModel.Options.TargetFilterType = TargetWorkflowType.TOP_DOWN;
                            }
                        }
                    }
                    else
                    {
                        var analysisJobDescriptionReader = new AnalysisJobDescriptionReader();

                        foreach (var fileName in openFileDialog.FileNames)
                        {
                            try
                            {
                                foreach (var analysisJobItem in analysisJobDescriptionReader.Read(fileName))
                                {
                                    AnalysisJobViewModel.AnalysisJobItems.Add(analysisJobItem);
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
            var options = new OptionsWindow(AnalysisJobViewModel.Options);
            options.ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AnalysisJobViewModel_AnalysisJobProcessed(object sender, MTDBResultChangedEventArgs e)
        {
            if (e.Result == null)
            {
                ShowDialog();
            }
            else
            {
                if (e.Result is ObservableCollection<AnalysisJobItem>)
                {
                    if (WorkspaceViewModel != null)
                    {
                        foreach (var analysisJobItem in AnalysisJobViewModel.AnalysisJobItems)
                        {
                            WorkspaceViewModel.AnalysisJobViewModel.AnalysisJobItems.Add(analysisJobItem);
                        }

                        WorkspaceViewModel.AnalysisJobViewModel.ProcessAnalysisDatabase();
                        WorkspaceViewModel.UpdateDataViewModels();

                        RecentAnalysisJobHelper.AddRecentAnalysisJob(WorkspaceViewModel.AnalysisJobViewModel);

                        Close();
                    }
                }
                else if (e.Result is TargetDatabase)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.NewWorkspacePage(AnalysisJobViewModel);

                        RecentAnalysisJobHelper.AddRecentAnalysisJob(AnalysisJobViewModel);
                    }

                    Close();
                }
            }
        }

        private void AddDataWindow_Closed(object sender, EventArgs e)
        {
            AnalysisJobViewModel.AnalysisJobProcessed -= AnalysisJobViewModel_AnalysisJobProcessed;
        }
    }
}

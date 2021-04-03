#region Namespaces

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using MTDBCreator.Commands;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.Dialog;
using MTDBCreator.ViewModels;
using MTDBCreator.Views;
using MTDBFramework.Data;
using MTDBFramework.IO;
using MTDBFrameworkBase.Data;
using MTDBFrameworkBase.Database;
using Ookii.Dialogs.Wpf;
using Application = System.Windows.Application;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

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
            if (!AnalysisJobViewModel.ShowOpenOldAnalysis)
            {
                AnalysisFolderItem.Visibility   = Visibility.Collapsed;
                AnalysisMenuItem.Visibility     = Visibility.Collapsed;
            }
        }

        public AnalysisJobViewModel AnalysisJobViewModel => DataContext as AnalysisJobViewModel;

        public WorkspaceViewModel WorkspaceViewModel { get; private set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddButton.ContextMenu != null) AddButton.ContextMenu.IsOpen = true;
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddFolderButton.ContextMenu != null) AddFolderButton.ContextMenu.IsOpen = true;
        }

        private void AddFolderFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is MenuItem addFolderFileMenuItem)
            {
                var formatInfo = FileDialogFormatInfoFactory.Create(addFolderFileMenuItem.Tag.ToString());

                var folderBrowser = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
                // folderBrowser.SelectedPath = "last path";

                var result = folderBrowser.ShowDialog();
                if (result == null || !result.Value)
                    return;

                var selectedFolder = folderBrowser.SelectedPath;
                if (string.IsNullOrEmpty(selectedFolder))
                    return;

                var filters = formatInfo.Filter.Split('|');

                var directory = new DirectoryInfo(selectedFolder);
                foreach(var filter in filters)
                {
                    if (!filter.Contains("."))
                        continue;

                    filters = filter.Split(';');

                    foreach (var fileExt in filters)
                    {
                        foreach (var file in directory.GetFiles(fileExt, SearchOption.AllDirectories))
                        {
                            if (file.Name.EndsWith("msgfdb_syn.txt") || file.Name.EndsWith("msgfplus_syn.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.MsgfPlus;
                            }
                            else if (file.Name.EndsWith("_syn.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.Sequest;
                            }
                            else if (file.Name.EndsWith("_xt.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.XTandem;
                            }
                            else if (file.Name.EndsWith("msalign_syn.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.MSAlign;
                            }

                            AnalysisJobViewModel.AnalysisJobItems.Add(new AnalysisJobItem(file.FullName,
                                formatInfo.Format));

                            if (formatInfo.Format == LcmsIdentificationTool.MSAlign)
                            {
                                AnalysisJobViewModel.Options.TargetFilterType = TargetWorkflowType.TOP_DOWN;
                            }
                        }
                    }
                }
            }
        }

        private void AddFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is MenuItem addFileMenuItem)
            {
                var formatInfo = FileDialogFormatInfoFactory.Create(addFileMenuItem.Tag.ToString());

                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    RestoreDirectory = true,

                    Multiselect = true,
                    Title = formatInfo.Title,
                    Filter = formatInfo.Filter
                };

                var dialogRes = openFileDialog.ShowDialog();

                if (dialogRes.HasValue && dialogRes.Value)
                {
                    if (formatInfo.Format != LcmsIdentificationTool.Description)
                    {
                        foreach (var fileName in openFileDialog.FileNames)
                        {
                            if (fileName.EndsWith("msgfdb_syn.txt") || fileName.EndsWith("msgfplus_syn.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.MsgfPlus;
                            }
                            else if (fileName.EndsWith("_syn.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.Sequest;
                            }
                            else if (fileName.EndsWith("_xt.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.XTandem;
                            }
                            else if (fileName.EndsWith("msalign_syn.txt"))
                            {
                                formatInfo.Format = LcmsIdentificationTool.MSAlign;
                            }

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
                                var caption = Application.Current.MainWindow?.Tag.ToString() ?? "Error";

                                MessageBox.Show(
                                    this, string.Format(
                                        "MTDB Creator cannot read this file.{0}This is not a valid Dataset Description file, or its format is not correct.{0}{0}{1}",
                                        Environment.NewLine, fileName), caption,
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

        private void AnalysisJobViewModel_AnalysisJobProcessed(object sender, MtdbResultChangedEventArgs e)
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
                    if (Application.Current.MainWindow is MainWindow mainWindow)
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

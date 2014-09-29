#region Namespaces

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MTDBCreator.ViewModels;
using MTDBCreator.ViewModels.TreeView;
using MTDBCreator.Views;
using MTDBCreator.Windows;

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
            WorkspaceViewModel = new WorkspaceViewModel(analysisJobViewModel);
            DataContext = WorkspaceViewModel;
        }

        private void AddAnalysisJobItemsButton_Click(object sender, RoutedEventArgs e)
        {
            var addDataJobWindow = new AddDataWindow(WorkspaceViewModel)
            {
                Owner = Application.Current.MainWindow
            };

            addDataJobWindow.ShowDialog();

            AnalysisJobDataGrid.SelectedIndex = -1;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            var optionsWindow = new OptionsWindow(WorkspaceViewModel.AnalysisJobViewModel.Options);
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

        private void SearchTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                WorkspaceViewModel.TargetTreeViewModel.EnterHandler();
            }
        }
    }
}
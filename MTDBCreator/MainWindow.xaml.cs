#region Namespaces

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MTDBCreator.PageControls;
using MTDBCreator.ViewModels;

#endregion

namespace MTDBCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void NewWorkspacePage(AnalysisJobViewModel analysisJobViewModel)
        {
            // At this point, analysisJobItems should have all LcmsDataSet read

            var workspacePage = new MTDBCreatorTabPage
                (analysisJobViewModel.Title,
                new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Images/Pages/WorkspacePage.png")), Stretch = Stretch.None },
                new WorkspacePageUserControl(analysisJobViewModel),
                TabPageCloseButton_Click);

            MainTabControl.Items.Add(workspacePage);
            MainTabControl.SelectedItem = workspacePage;
        }

        private void TabPageCloseButton_Click(object sender, RoutedEventArgs e)
        {
            var currentCloseButton = e.Source as Button;

            if (currentCloseButton != null)
            {
                var tabItem = currentCloseButton.Tag as TabItem;

                if (tabItem != null)
                {
                    MainTabControl.Items.Remove(tabItem);
                }
            }
        }
    }
}

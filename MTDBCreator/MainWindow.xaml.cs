#region Namespaces

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MTDBCreator.PageControls;
using MTDBCreator.ViewModels;
using MTDBFramework.Data;

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

            MTDBCreatorTabPage workspacePage = new MTDBCreatorTabPage
                (analysisJobViewModel.Title,
                new Image() { Source = new BitmapImage(new Uri("pack://application:,,,/Images/Pages/WorkspacePage.png")), Stretch = Stretch.None },
                new WorkspacePageUserControl(analysisJobViewModel),
                TabPageCloseButton_Click);

            MainTabControl.Items.Add(workspacePage);
            MainTabControl.SelectedItem = workspacePage;
        }

        private void TabPageCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Button currentCloseButton = e.Source as Button;

            if (currentCloseButton != null)
            {
                TabItem tabItem = currentCloseButton.Tag as TabItem;

                if (tabItem != null)
                {
                    MainTabControl.Items.Remove(tabItem);
                }
            }
        }
    }
}

#region Namespaces

using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Properties;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;

#endregion

namespace MTDBCreator.PageControls
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePageUserControl : UserControl
    {
        public HomePageUserControl()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Refresh recent opened items from user settings

            RecentAnalysisJobStackPanel.Children.Clear();

            if (Settings.Default.RecentAnalysisJobs.Count > 0)
            {
                foreach (var s in Settings.Default.RecentAnalysisJobs)
                {
                    var itemTextBlock = new TextBlock
                    {
                        Text = RecentAnalysisJobHelper.GetRecentAnalysisJobTitle(s),
                        TextTrimming = TextTrimming.CharacterEllipsis
                    };

                    var newRecentItemButton = new Button
                    {
                        Style = (Style)FindResource("LeftPanelHyperLinkButtonStyle"),
                        Content = itemTextBlock,
                        Tag = s
                    };

                    newRecentItemButton.Click += newRecentItemButton_Click;

                    RecentAnalysisJobStackPanel.Children.Add(newRecentItemButton);
                }
            }
            else
            {
                var itemTextBlock = new TextBlock
                {
                    Text = "(No jobs opened recently)",
                    Style = (Style)FindResource("LeftPanelDescriptionStyle")
                };

                RecentAnalysisJobStackPanel.Children.Add(itemTextBlock);
            }
        }


        private void newRecentItemButton_Click(object sender, RoutedEventArgs e)
        {
            var newRecentItemButton = e.OriginalSource as Button;

            if (newRecentItemButton != null)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;

                if (mainWindow != null && newRecentItemButton.Tag != null)
                {
                    var analysisJobViewModel = RecentAnalysisJobHelper.GetRecentAnalysisJobItem(newRecentItemButton.Tag.ToString());

                    var result = BackgroundWorkProcessHelper.Process(new AnalysisJobBackgroundWorkHelper(analysisJobViewModel));

                    if (result != null)
                    {
                        result = BackgroundWorkProcessHelper.Process(new MtdbProcessorBackgroundWorkHelper(analysisJobViewModel));

                        if (analysisJobViewModel.Database != null)
                        {
                            
                            mainWindow.NewWorkspacePage(analysisJobViewModel);
                            
                        }
                    }
                }
            }
        }

        private void NewMassTagOpenButton_Click(object sender, RoutedEventArgs e)
        {
            var addDataWindow = new AddDataWindow(null)
            {
                Owner = Application.Current.MainWindow
            };

            addDataWindow.ShowDialog();

        }

        private void NewAnalysisJobButton_Click(object sender, RoutedEventArgs e)
        {
            var addDataJobWindow = new AddDataWindow(null)
            {
                Owner = Application.Current.MainWindow
            };

            addDataJobWindow.ShowDialog();
        }
    }
}

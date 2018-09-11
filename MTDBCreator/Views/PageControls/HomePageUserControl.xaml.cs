#region Namespaces

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Properties;
using MTDBCreator.ViewModels;
using MTDBCreator.Views.Windows;
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

            if (!System.Net.Dns.GetHostEntry("").HostName.Contains("pnl.gov"))
            {
                DmsButton.Visibility = Visibility.Hidden;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Refresh recent opened items from user settings

            try
            {

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
            catch (Exception ex)
            {
                MessageBox.Show("Exception in Page_Loaded: " + ex.Message +
                                PRISM.clsStackTraceFormatter.GetExceptionStackTraceMultiLine(ex),
                                "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }


        private void newRecentItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is Button newRecentItemButton))
                return;

            if (!(Application.Current.MainWindow is MainWindow mainWindow) || newRecentItemButton.Tag == null)
                return;

            var analysisJobViewModel = RecentAnalysisJobHelper.GetRecentAnalysisJobItem(newRecentItemButton.Tag.ToString());

            var result = BackgroundWorkProcessHelper.Process(new AnalysisJobBackgroundWorkHelper(analysisJobViewModel));

            if (result == null)
                return;

            BackgroundWorkProcessHelper.Process(new MtdbProcessorBackgroundWorkHelper(analysisJobViewModel));

            if (analysisJobViewModel.Database != null)
            {
                mainWindow.NewWorkspacePage(analysisJobViewModel);
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
            try
            {
                var addDataJobWindow = new AddDataWindow(null)
                {
                    Owner = Application.Current.MainWindow
                };

                addDataJobWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception calling AddDataWindow: " + ex.Message +
                                PRISM.clsStackTraceFormatter.GetExceptionStackTraceMultiLine(ex),
                                "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void NewDmsExportButton_Click(object sender, RoutedEventArgs e)
        {
            var dmsExportWindow = new DmsExporterWindow(null)
            {
                Owner = Application.Current.MainWindow
            };

            dmsExportWindow.ShowDialog();
        }
    }
}

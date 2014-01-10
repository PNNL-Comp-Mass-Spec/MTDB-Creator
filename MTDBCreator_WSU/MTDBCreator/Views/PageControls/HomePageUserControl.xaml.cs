#region Namespaces

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MTDBCreator.Helpers;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Properties;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Data;

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
                foreach (string s in Settings.Default.RecentAnalysisJobs)
                {
                    TextBlock itemTextBlock = new TextBlock()
                    {
                        Text = RecentAnalysisJobHelper.GetRecentAnalysisJobTitle(s),
                        TextTrimming = TextTrimming.CharacterEllipsis
                    };

                    Button newRecentItemButton = new Button()
                    {
                        Style = (Style)this.FindResource("LeftPanelHyperLinkButtonStyle"),
                        Content = itemTextBlock,
                        Tag = s
                    };

                    newRecentItemButton.Click += newRecentItemButton_Click;

                    RecentAnalysisJobStackPanel.Children.Add(newRecentItemButton);
                }
            }
            else
            {
                TextBlock itemTextBlock = new TextBlock()
                {
                    Text = "(No jobs opened recently)",
                    Style = (Style)this.FindResource("LeftPanelDescriptionStyle")
                };

                RecentAnalysisJobStackPanel.Children.Add(itemTextBlock);
            }
        }


        private void newRecentItemButton_Click(object sender, RoutedEventArgs e)
        {
            Button newRecentItemButton = e.OriginalSource as Button;

            if (newRecentItemButton != null)
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                if (mainWindow != null && newRecentItemButton.Tag != null)
                {
                    AnalysisJobViewModel analysisJobViewModel = RecentAnalysisJobHelper.GetRecentAnalysisJobItem(newRecentItemButton.Tag.ToString());

                    object result = BackgroundWorkProcessHelper.Process(new AnalysisJobBackgroundWorkHelper(analysisJobViewModel));

                    if (result != null)
                    {
                        result = BackgroundWorkProcessHelper.Process(new MTDBProcessorBackgroundWorkHelper(analysisJobViewModel));

                        if (analysisJobViewModel.Database != null)
                        {
                            mainWindow.NewWorkspacePage(analysisJobViewModel);
                        }
                    }
                }
            }
        }

        private void NewAnalysisJobButton_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataJobWindow = new AddDataWindow(null)
            {
                Owner = Application.Current.MainWindow
            };

            addDataJobWindow.ShowDialog();
        }
    }
}

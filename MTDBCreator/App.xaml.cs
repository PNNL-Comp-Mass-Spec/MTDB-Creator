#region Namespaces

using System.Configuration;
using System.Windows;
using MTDBCreator.Properties;

#endregion

namespace MTDBCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Settings.Default.SettingsLoaded += Default_SettingsLoaded;
        }

        private void Default_SettingsLoaded(object sender, SettingsLoadedEventArgs e)
        {
            // Remove placeholder from user settings - Initialization purpose only

            Settings.Default.RecentAnalysisJobs.Remove("placeholder");
        }

    }
}

using System.Windows;
using MTDBCreator.Helpers.BackgroundWork;
using MTDBCreator.Windows;

namespace MTDBCreator.Helpers
{
    internal static class BackgroundWorkProcessHelper
    {
        public static object MostRecentResult { get; private set; }

        internal static object Process(IBackgroundWorkHelper backgroundWorkHelper)
        {
            var processWindow = new ProcessWindow(backgroundWorkHelper, Application.Current.MainWindow);

            if (processWindow.MultithreadingEnabled)
            {

                if (processWindow.ShowDialog() == true)
                {
                    return backgroundWorkHelper.Result;
                }
            }
            else
            {
                processWindow.Show();
                processWindow.StartProcessingNonThreaded(backgroundWorkHelper);
                processWindow.Close();

                MostRecentResult = processWindow.MostRecentResult;

                return backgroundWorkHelper.Result;
            }

            return null;
        }
    }
}
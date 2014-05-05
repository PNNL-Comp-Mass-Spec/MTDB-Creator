#region Namespaces

using System.ComponentModel;
using MTDBCreator.Windows;

#endregion

namespace MTDBCreator.Helpers.BackgroundWork
{
    public interface IBackgroundWorkHelper
    {
        void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
        void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e);
        void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);

        object Result { get; }

        ProcessWindow HostProcessWindow { get; set; }
    }
}
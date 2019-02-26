#region Namespaces

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Algorithms;
using MTDBFramework.UI;
using MTDBFrameworkBase.Database;

#endregion

namespace MTDBCreator.Helpers.BackgroundWork
{
    public sealed class MtdbProcessorBackgroundWorkHelper : IBackgroundWorkHelper
    {
        private readonly AnalysisJobViewModel m_analysisJobViewModel;
        private bool mAbortRequested;

        public MtdbProcessorBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel)
        {
            m_analysisJobViewModel = analysisJobViewModel;
        }

        public void BackgroundWorker_AbortProcessing()
        {
            mAbortRequested = true;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            mAbortRequested = false;

            var mtdbProcessor = new MtdbProcessor(m_analysisJobViewModel.Options);
            mtdbProcessor.AlignmentComplete +=  mtdbProcessor_AlignmentComplete;
            mtdbProcessor.ProgressChanged += mtdbProcessor_ProgressChanged;
            HostProcessWindow.MainBackgroundWorker.ReportProgress(0);

            try
            {
                    e.Result =
                        mtdbProcessor.Process(
                            m_analysisJobViewModel.AnalysisJobItems.Select(job => job.DataSet).ToList(), HostProcessWindow.MainBackgroundWorker);

                    if (HostProcessWindow.MainBackgroundWorker.CancellationPending || mAbortRequested)
                    {
                        e.Cancel = true;
                    }

            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void mtdbProcessor_AlignmentComplete(object sender, AlignmentCompleteArgs e)
        {
        }

        private void mtdbProcessor_ProgressChanged(object sender, PercentCompleteEventArgs e)
        {
            string statusMessage;

            if (string.IsNullOrEmpty(e.CurrentTask))
                statusMessage = "Processing dataset file";
            else
                statusMessage = e.CurrentTask;

            HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current,
                                                                  new object[] { e.Total.ToString(), statusMessage });

        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            HostProcessWindow.Status = "Processing Data...";

            HostProcessWindow.MainProgressBar.IsIndeterminate = false;

            if (e.UserState is object[] userStates)
            {
                var total = Convert.ToInt32(userStates[0].ToString());
                var status = userStates[1].ToString();

                HostProcessWindow.MainProgressBar.Value = e.ProgressPercentage;
                HostProcessWindow.MainProgressBar.Maximum = total;
                HostProcessWindow.Status = status;

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Processing was cancelled prior to completion");
            }
            else
            {
                if (e.Result is Exception ex)
                {
                    ErrorHelper.WriteExceptionTraceInformation(ex);

                    var caption = Application.Current.MainWindow?.Tag.ToString() ?? "Error";
                    MessageBox.Show(
                        string.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message),
                        caption, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    m_analysisJobViewModel.Database = e.Result as TargetDatabase;
                }

                HostProcessWindow.DialogResult = !(e.Result is Exception);
            }
            HostProcessWindow.Close();
        }

        public object Result => m_analysisJobViewModel.Database;

        public ProcessWindow HostProcessWindow { get; set; }
    }
}
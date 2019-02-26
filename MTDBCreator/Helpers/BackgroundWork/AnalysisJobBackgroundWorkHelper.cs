#region Namespaces

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Data;
using MTDBFramework.IO;
using MTDBFramework.UI;

#endregion

namespace MTDBCreator.Helpers.BackgroundWork
{
    public class AnalysisJobBackgroundWorkHelper : IBackgroundWorkHelper
    {
        private bool m_abortRequested;
        private AnalysisJobProcessor m_analysisJobProcessor;

        public AnalysisJobBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel)
        {
            AnalysisJobViewModel = analysisJobViewModel;
        }

        public void BackgroundWorker_AbortProcessing()
        {
            m_abortRequested = true;
            m_analysisJobProcessor.AbortProcessing();

        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_abortRequested = false;

            m_analysisJobProcessor = new AnalysisJobProcessor(AnalysisJobViewModel.Options);
            m_analysisJobProcessor.ProgressChanged += analysisJobProcessor_ProgressChanged;

            try
            {
                e.Result = m_analysisJobProcessor.Process(AnalysisJobViewModel.AnalysisJobItems, HostProcessWindow.MainBackgroundWorker);

                if (HostProcessWindow.MainBackgroundWorker.CancellationPending || m_abortRequested)
                {
                    e.Cancel = true;
                }
            }

            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            HostProcessWindow.MainProgressBar.IsIndeterminate = false;

            if (e.UserState is object[] userStates)
            {
                var total = Convert.ToInt32(userStates[0].ToString());
                var status = userStates[1].ToString();

                HostProcessWindow.MainProgressBar.Value = e.ProgressPercentage;
                HostProcessWindow.MainProgressBar.Maximum = total;
                HostProcessWindow.Status = status;

                // This is the WPF version of DoEvents
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(String.Format("Processing was cancelled prior to completion"));
            }
            else
            {
                if (e.Result is Exception)
                {
                    var ex = e.Result as Exception;

                    ErrorHelper.WriteExceptionTraceInformation(ex);

                    MessageBox.Show(String.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message), Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                HostProcessWindow.DialogResult = !(e.Result is Exception);

            }
            HostProcessWindow.Close();
        }

        public object Argument => AnalysisJobViewModel;

        public object Result => AnalysisJobViewModel.AnalysisJobItems;

        public ProcessWindow HostProcessWindow { get; set; }

        private void analysisJobProcessor_ProgressChanged(object sender, MtdbProgressChangedEventArgs e)
        {
            if (!(e.UserObject is AnalysisJobItem analysisJobItem))
            {
                if (!string.IsNullOrEmpty(e.CurrentTask))
                {
                    HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current,
                                                                          new object[]
                                                                          {e.Total.ToString(), e.CurrentTask});
                }
            }
            else
            {
                string statusMessage;

                if (string.IsNullOrEmpty(e.CurrentTask))
                    statusMessage = "Processing analysis file";
                else
                    statusMessage = e.CurrentTask;

                statusMessage += ": " + Path.GetFileName(analysisJobItem.FilePath);

                HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current,
                                                                      new object[]
                                                                      {e.Total.ToString(), statusMessage});
            }
        }

        private AnalysisJobViewModel AnalysisJobViewModel { get; }
    }
}
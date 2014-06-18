#region Namespaces

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Algorithms;
using MTDBFramework.Database;

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

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            HostProcessWindow.Status = "Processing Data...";
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

                    MessageBox.Show(
                        String.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message),
                        Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    m_analysisJobViewModel.Database = e.Result as TargetDatabase;
                }

                HostProcessWindow.DialogResult = !(e.Result is Exception);
            }
            HostProcessWindow.Close();
        }

        public object Result
        {
            get { return m_analysisJobViewModel.Database; }
        }

        public ProcessWindow HostProcessWindow { get; set; }        
    }
}
#region Namespaces

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Algorithms;
using MTDBFramework.Data;
using MTDBFramework.Database;

#endregion

namespace MTDBCreator.Helpers.BackgroundWork
{
    public class MTDBProcessorBackgroundWorkHelper : IBackgroundWorkHelper
    {
        public MTDBProcessorBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel)
        {
            this.m_AnalysisJobViewModel = analysisJobViewModel;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MTDBProcessor mtdbProcessor = new MTDBProcessor(this.m_AnalysisJobViewModel.Options);

            this.HostProcessWindow.MainBackgroundWorker.ReportProgress(0);

            try
            {
                e.Result = mtdbProcessor.Process(this.m_AnalysisJobViewModel.AnalysisJobItems.Select(job => job.DataSet).ToList());
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.HostProcessWindow.Status = "Processing Data...";
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                Exception ex = e.Result as Exception;

                ErrorHelper.WriteExceptionTraceInformation(ex);

                MessageBox.Show(String.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message), Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.m_AnalysisJobViewModel.Database = e.Result as TargetDatabase;
            }

            this.HostProcessWindow.DialogResult = !(e.Result is Exception);
            this.HostProcessWindow.Close();
        }

        public object Argument
        {
            get { return this.m_AnalysisJobViewModel; }
        }
        public object Result
        {
            get { return this.m_AnalysisJobViewModel.Database; }
        }

        public ProcessWindow HostProcessWindow { get; set; }

        private AnalysisJobViewModel m_AnalysisJobViewModel { get; set; }
    }
}
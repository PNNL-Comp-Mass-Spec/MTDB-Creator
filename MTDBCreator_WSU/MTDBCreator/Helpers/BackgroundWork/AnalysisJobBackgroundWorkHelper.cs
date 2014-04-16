#region Namespaces

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
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
        public AnalysisJobBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel)
        {
            this.m_AnalysisJobViewModel = analysisJobViewModel;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            AnalysisJobProcessor analysisJobProcessor = new AnalysisJobProcessor(this.m_AnalysisJobViewModel.Options);
            analysisJobProcessor.ProgressChanged += analysisJobProcessor_ProgressChanged;

            try
            {
                e.Result = analysisJobProcessor.Process(this.m_AnalysisJobViewModel.AnalysisJobItems);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.HostProcessWindow.MainProgressBar.IsIndeterminate = false;

            object[] userStates = e.UserState as object[];

            if (userStates != null)
            {
                int total = Convert.ToInt32(userStates[0].ToString());
                string status = userStates[1].ToString();

                this.HostProcessWindow.MainProgressBar.Value = e.ProgressPercentage;
                this.HostProcessWindow.MainProgressBar.Maximum = total;
                this.HostProcessWindow.Status = status;
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Exception)
            {
                Exception ex = e.Result as Exception;

                ErrorHelper.WriteExceptionTraceInformation(ex);

                MessageBox.Show(String.Format("The following exception has occurred:{0}{0}{1}", Environment.NewLine, ex.Message), Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
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
            get { return this.m_AnalysisJobViewModel.AnalysisJobItems; }
        }

        public ProcessWindow HostProcessWindow { get; set; }

        private void analysisJobProcessor_ProgressChanged(object sender, MtdbProgressChangedEventArgs e)
        {
            AnalysisJobItem analysisJobItem = e.UserObject as AnalysisJobItem;

            if (analysisJobItem != null)
            {
                this.HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Concat("Processing analysis file: ", Path.GetFileName(analysisJobItem.FilePath)) });
            }
        }

        private AnalysisJobViewModel m_AnalysisJobViewModel { get; set; }
    }
}
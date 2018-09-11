using System;
using System.ComponentModel;
using System.Windows;
using MTDBCreator.ViewModels;
using MTDBCreator.Windows;
using MTDBFramework.Data;
using MTDBFramework.Database;
using MTDBFramework.IO;
using MTDBFramework.UI;
using MTDBAccessIO;
using PRISM;

namespace MTDBCreator.Helpers.BackgroundWork
{
    public class MtdbWriterBackgroundWorkHelper : IBackgroundWorkHelper
    {
        protected bool mAbortRequested;

        public MtdbWriterBackgroundWorkHelper(AnalysisJobViewModel analysisJobViewModel, string fileName)
        {
            Database = analysisJobViewModel.Database;
            DatabaseOptions = analysisJobViewModel.Options;
            DatabaseFileName = fileName;
        }

        public void BackgroundWorker_AbortProcessing()
        {
            mAbortRequested = true;
        }

        public void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            mAbortRequested = false;
            try
            {
                ITargetDatabaseWriter targetDatabaseWriter;
                if (DatabaseOptions.DatabaseType != DatabaseType.Access)
                {
                    targetDatabaseWriter = new SqLiteTargetDatabaseWriter();
                }
                else
                {
                    targetDatabaseWriter = new AccessTargetDatabaseWriter();
                }
                targetDatabaseWriter.ProgressChanged += targetDatabaseWriter_ProgressChanged;
                targetDatabaseWriter.Write(Database, DatabaseOptions, DatabaseFileName);
            }
            catch (Exception ex)
            {
                var errMsg = "Exception in MtdbWriterBackgroundWorkHelper_DoWork";
                if (DatabaseOptions.ConsoleMode)
                {
                    ConsoleMsgUtils.ShowError(errMsg, ex);
                }
                else
                {

                    MessageBox.Show(errMsg + ": " + ex.Message +
                                    clsStackTraceFormatter.GetExceptionStackTraceMultiLine(ex),
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        public void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            HostProcessWindow.MainProgressBar.IsIndeterminate = (e.ProgressPercentage == -1);

            if (e.ProgressPercentage >= 0)
            {
                var userStates = e.UserState as object[];

                if (userStates != null)
                {
                    var total = Convert.ToInt32(userStates[0].ToString());
                    var status = userStates[1].ToString();

                    HostProcessWindow.MainProgressBar.Value = e.ProgressPercentage;
                    HostProcessWindow.MainProgressBar.Maximum = total;
                    HostProcessWindow.Status = status;
                }
            }
            else
            {
                HostProcessWindow.Status = e.UserState.ToString();
            }
        }

        public void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HostProcessWindow.Close();

            if (e.Result == null)
            {
                var msg = string.Format("The MTDB has been created successfully.{0}{0}Path:{0}{0}{1}", Environment.NewLine, DatabaseFileName);
                if (DatabaseOptions.ConsoleMode)
                {
                    Console.WriteLine();
                    Console.WriteLine(msg);
                }
                else
                {
                    MessageBox.Show(msg, Application.Current.MainWindow.Tag.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public object Argument => new object[] { Database, DatabaseFileName };

        public object Result => throw new NotSupportedException("No Result in the current BackgroundWorkHelper");

        public ProcessWindow HostProcessWindow { get; set; }

        private void targetDatabaseWriter_ProgressChanged(object sender, MtdbProgressChangedEventArgs e)
        {
            if (e.UserObject == null)
            {
                if (e.Current > 0 && e.Total > 0)
                    HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Writing to MTDB...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });
                else
                    HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, String.Concat("Writing to MTDB: ", DatabaseFileName));

                return;
            }

            var type = (MtdbCreationProgressType)Enum.Parse(typeof(MtdbCreationProgressType), e.UserObject.ToString());

            switch (type)
            {
                case MtdbCreationProgressType.CONSENSUS_TARGET:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Analysis Evidences...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });

                        break;
                    }
                case MtdbCreationProgressType.SEQUENCE:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Sequence Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });

                        break;
                    }
                case MtdbCreationProgressType.PEPTIDE:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Peptide Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total).ToString()) });

                        break;
                    }
                case MtdbCreationProgressType.ANALYSIS_SOURCE:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, new object[] { e.Total.ToString(), String.Format("Processing Analysis Source Information...{0}%", WindowHelper.GetPercentage(e.Current, e.Total)) });

                        break;
                    }
                case MtdbCreationProgressType.COMMIT:
                    {
                        HostProcessWindow.MainBackgroundWorker.ReportProgress(e.Current, String.Concat("Writing to MTDB: ", DatabaseFileName));

                        break;
                    }
            }
        }

        private TargetDatabase Database { get; set; }
        private Options DatabaseOptions { get; set; }
        private string DatabaseFileName { get; set; }
    }
}
